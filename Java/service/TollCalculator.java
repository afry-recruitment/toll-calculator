package service;

import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.ZoneId;
import java.time.ZonedDateTime;
import java.util.Arrays;
import java.util.Calendar;
import java.util.GregorianCalendar;
import java.util.Objects;

import exception.DailyVehicleReportException;
import model.DailyVehicleReport;
import model.toll.TollFee;
import model.toll.TollFeeType;
import model.toll.event.TollEvenType;
import model.toll.event.TollEvent;
import model.vehicle.Vehicle;

public class TollCalculator {

	private static final int MAX_DAILY_AMOUNT = 60;

	/**
	 * Calculate the total toll fee for one day
	 *
	 * @param vehicle - the vehicle
	 * @param dates   - LocalDateTime dates. Toll event dates for the given vehicle
	 *                for One day.
	 * @return - The total toll fee report this vehicle based on the date given as
	 *         param to this method
	 */
	public DailyVehicleReport getTollFeeReport(Vehicle vehicle, LocalDateTime... dates) {

		DailyVehicleReport report = new DailyVehicleReport();

		/*
		 * 60 min Intervals, where only one and the highest fee are valid, shall be
		 * non-overlapping. A new interval begins with the next valid fee that occurs
		 * after the last interval has elapsed.
		 */

		if (dates == null || dates.length == 0) {
			throw new DailyVehicleReportException("Dates can not be null or empty");
		}

		if (vehicle == null) {
			throw new DailyVehicleReportException("Vehicle can not be null");
		}

		// dates need to be in ascending order for non-overlapping intervals
		Arrays.sort(dates);

		// Sanity check that first and last date is whitin the same day
		if (dates[0].getDayOfYear() != dates[dates.length - 1].getDayOfYear()) {
			throw new DailyVehicleReportException("Dates needs to be the same day");
		}

		// Set the date of this report to the first event
		report.setReportDate(dates[0]);

		TollEvent higestTollForIntervall = null;

		// All dates will be added to the report as event that can be toll free or
		// chargeable events
		for (int i = 0; i < dates.length; i++) {
			LocalDateTime date = dates[i];

			TollEvent tollEvent = createTollEvent(date, vehicle, report,
					Objects.nonNull(higestTollForIntervall) ? higestTollForIntervall.getIntervalStart() : date);

			// ignore toll free events but add them to report and continue with next
			// iteration
			if (tollEvent.getType().isTollFree()) {
				report.getNoneChargedTollEvent().add(tollEvent);
				// initiate the first highest reference for the first interval.
			} else if (higestTollForIntervall == null) {
				higestTollForIntervall = tollEvent;
			}
			// check if this fee is within a started hourly interval.
			else if (!date.isBefore(higestTollForIntervall.getIntervalStart())
					&& (date.isBefore(higestTollForIntervall.getIntervalStart().plusMinutes(60)))) {

				int tollTypeFee = tollEvent.getTollFee().getType().getFee();
				// Set as highest fee for this interval, if not highest we deduct this event fee.
				if (tollTypeFee > higestTollForIntervall.getTollFee().getType().getFee()) {
					// Previous highest Toll Fee for this interval needs to be deducted because the recent one is higher
					this.deductIntervalEventFee(higestTollForIntervall, report);
					higestTollForIntervall = tollEvent;
				} else {
					// There is an event with higher fee in current interval so deduct this event
					// fee.
					this.deductIntervalEventFee(tollEvent, report);

				}
			} // a new interval can begin and we can add the previous event to the report.
			else {
				report.getChargedTollEvent().add(higestTollForIntervall);
				tollEvent.setIntervalStart(date);
				higestTollForIntervall = tollEvent;
			}

			// add to report if this is the last date in the array and no further fees to compare
			// against
			if (higestTollForIntervall != null && (dates.length - 1) == i) {
				report.getChargedTollEvent().add(higestTollForIntervall);
			}

		}

		return report;
	}

	/**
	 * 
	 * create TollPassEvent for given day time and vehicle type. For Those that are
	 * not toll free event types we set the toll fee as well.
	 * 
	 * @param date    - Time of the toll event
	 * @param vehicle - Vehicle object containing a vehicle type that might be toll
	 *                free.
	 * @param report  - Daily report for a vehicles toll events
	 * @return Return TollPassEvent
	 */
	private TollEvent createTollEvent(final LocalDateTime dateTime, Vehicle vehicle, DailyVehicleReport report,
			LocalDateTime intervalStart) {
		LocalTime time = dateTime.toLocalTime();

		if (report.getTotalAmountToPay() >= TollCalculator.MAX_DAILY_AMOUNT) {
			report.setTotalDiscountForMaxAmount(
					report.getTotalDiscountForMaxAmount() + TollFeeType.getFeeTypeFromDate(time).getFee());
			return new TollEvent(vehicle, time, TollEvenType.FULLY_DEDUCTED_ON_MAX_DAILY_EVENT_TYPE, intervalStart);
		} else if (isTollFreeDate(dateTime)) {
			return new TollEvent(vehicle, time, TollEvenType.TOLL_FREE_DAY_EVENT_TYPE, intervalStart);
		} else if (vehicle.getVehicleType().isTollFree()) {
			return new TollEvent(vehicle, time, TollEvenType.TOLL_FREE_VEHICLE_EVENT_TYPE, intervalStart);
		} else {
			// Get the fee type for this day time.
			TollFeeType feeType = TollFeeType.getFeeTypeFromDate(time);
			if (feeType.getFee() == 0) {
				return new TollEvent(vehicle, time, TollEvenType.TOLL_FREE_DAY_TIME_EVENT_TYPE, intervalStart);
			} else {
				int totalDailyFee = feeType.getFee() + report.getTotalAmountToPay();
				// Partly deduct this fee because max daily amount is reached
				if (totalDailyFee >= TollCalculator.MAX_DAILY_AMOUNT) {
					int partialFee = TollCalculator.MAX_DAILY_AMOUNT - report.getTotalAmountToPay();
					TollEvent tollEvent = new TollEvent(vehicle, time,
							TollEvenType.PARTIAL_DEDUCTED_ON_MAX_DAILY_EVENT_TYPE, intervalStart);
					tollEvent.setTollFee(new TollFee(feeType, partialFee));
					report.setTotalDiscountForMaxAmount(totalDailyFee - TollCalculator.MAX_DAILY_AMOUNT);
					return tollEvent;
				} else {
					// Set a full qualified toll fee
					TollEvent tollEvent = new TollEvent(vehicle, time, TollEvenType.CHARGEABLE_EVENT_TYPE,
							intervalStart);
					tollEvent.setTollFee(new TollFee(feeType, feeType.getFee()));
					return tollEvent;
				}

			}

		}

	}

	/**
	 * This method is the same as in the beginning of the exercise.
	 * 
	 * @param date
	 * @return
	 */
	private Boolean isTollFreeDate(LocalDateTime date) {

		Calendar calendar = GregorianCalendar.from(ZonedDateTime.of(date, ZoneId.systemDefault()));
		int year = calendar.get(Calendar.YEAR);
		int month = calendar.get(Calendar.MONTH);
		int day = calendar.get(Calendar.DAY_OF_MONTH);

		int dayOfWeek = calendar.get(Calendar.DAY_OF_WEEK);
		if (dayOfWeek == Calendar.SATURDAY || dayOfWeek == Calendar.SUNDAY)
			return true;

		if (year == 2024) {
			if (month == Calendar.JANUARY && day == 1 || month == Calendar.MARCH && (day == 28 || day == 29)
					|| month == Calendar.APRIL && (day == 1 || day == 30)
					|| month == Calendar.MAY && (day == 1 || day == 8 || day == 9)
					|| month == Calendar.JUNE && (day == 5 || day == 6 || day == 21) || month == Calendar.JULY
					|| month == Calendar.NOVEMBER && day == 1
					|| month == Calendar.DECEMBER && (day == 24 || day == 25 || day == 26 || day == 31)) {
				return true;
			}
		}
		return false;
	}

	/**
	 * 
	 * Set the toll event type to be deductive due to another higher interval fee,
	 * and add this event to the report's list of none chargeable toll events.
	 * 
	 * @param tollEvent
	 * @param report
	 */
	private void deductIntervalEventFee(TollEvent tollEvent, DailyVehicleReport report) {

		tollEvent.setType(TollEvenType.DEDUCTED_ON_HOURLY_INTERVAL_EVENT_TYPE);
		tollEvent.getTollFee().setAmountToPay(0);
		report.getNoneChargedTollEvent().add(tollEvent);
	}

}
