import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

import model.DailyVehicleReport;
import model.toll.event.TollEvent;
import model.vehicle.Vehicle;
import model.vehicle.VehicleType;
import service.TollCalculator;

public class TestVehicleReportApp {

	public static void main(String[] args) {
		
		ArrayList<LocalDateTime> dates = new ArrayList<LocalDateTime>();
		dates.add(LocalDateTime.parse("2024-06-03T05:15:00"));
		dates.add(LocalDateTime.parse("2024-06-03T06:15:00"));
		dates.add(LocalDateTime.parse("2024-06-03T07:14:00"));
		dates.add(LocalDateTime.parse("2024-06-03T07:20:00"));
		dates.add(LocalDateTime.parse("2024-06-03T12:15:00"));
		dates.add(LocalDateTime.parse("2024-06-03T12:30:00"));
		dates.add(LocalDateTime.parse("2024-06-03T13:20:00"));
		dates.add(LocalDateTime.parse("2024-06-03T15:20:00"));
		dates.add(LocalDateTime.parse("2024-06-03T15:45:00"));
		
		TollCalculator tollCalculator = new TollCalculator();
		DailyVehicleReport report = tollCalculator.getTollFeeReport(new Vehicle(VehicleType.Car), dates.toArray(LocalDateTime[]::new));


		
		System.out.println("Report Date: "+report.getReportDate());
		System.out.println("Total Amount: "+report.getTotalAmountToPay());
		System.out.println("Total deducted fee amount because daily max amount was reached: "+report.getTotalDiscountForMaxAmount());


		List<TollEvent> chargedTollEvents = report.getChargedTollEvent();
		
		System.out.println("---- Charged Toll Events ----");
		for (TollEvent chargedTollEvent : chargedTollEvents) {
			System.out.println("Time "+chargedTollEvent.getDate());
			System.out.println("Fee "+chargedTollEvent.getTollFee().getAmountToPay());
			System.out.println("Event type: "+chargedTollEvent.getType().name());
			System.out.println("Event description:"+chargedTollEvent.getType().getDescription());
			System.out.println("Interval start:"+chargedTollEvent.getIntervalStart());
			System.out.println("-----------------------------------");
		}

		
		List<TollEvent> noneChargedTollEvents = report.getNoneChargedTollEvent();
		System.out.println("---- None charged Toll Events ----");
		for (TollEvent noneChargedTollEvent : noneChargedTollEvents) {
			System.out.println("Time "+noneChargedTollEvent.getDate());
			System.out.println("Event type: "+noneChargedTollEvent.getType().name());
			System.out.println("Event description: "+noneChargedTollEvent.getType().getDescription());
			System.out.println("Interval start:"+noneChargedTollEvent.getIntervalStart());
			System.out.println("-----------------------------------");

		}
		
		
		//assert noneChargedTollEvents.size() != 4 : " Wrong number of deducted fees";
		//assert noneChargedTollEvents.size() != 5 : " Wrong number of chargeable fees";

		
		//Result
		
//		Report Date: 2024-06-03T05:15
//		Total Amount: 60
//		Total deducted fee amount because daily max amount was reached: 10
//		---- Charged Toll Events ----
//		Time 07:14
//		Fee 18
//		Event type: CHARGEABLE_EVENT_TYPE
//		Event description:The vehicle for this event is not toll free and the event occurred during a chargeable hour.
//		Interval start:2024-06-03T06:15
//		-----------------------------------
//		Time 07:20
//		Fee 18
//		Event type: CHARGEABLE_EVENT_TYPE
//		Event description:The vehicle for this event is not toll free and the event occurred during a chargeable hour.
//		Interval start:2024-06-03T07:20
//		-----------------------------------
//		Time 12:15
//		Fee 8
//		Event type: CHARGEABLE_EVENT_TYPE
//		Event description:The vehicle for this event is not toll free and the event occurred during a chargeable hour.
//		Interval start:2024-06-03T12:15
//		-----------------------------------
//		Time 13:20
//		Fee 8
//		Event type: CHARGEABLE_EVENT_TYPE
//		Event description:The vehicle for this event is not toll free and the event occurred during a chargeable hour.
//		Interval start:2024-06-03T13:20
//		-----------------------------------
//		Time 15:45
//		Fee 8
//		Event type: PARTIAL_DEDUCTED_ON_MAX_DAILY_EVENT_TYPE
//		Event description:The vehicle for this event is not toll free and the event occurred during chargeable hours but maximum daily fee is reached when adding this event
//		Interval start:2024-06-03T15:20
//		-----------------------------------
//		---- None charged Toll Events ----
//		Time 05:15
//		Event type: TOLL_FREE_DAY_TIME_EVENT_TYPE
//		Event description: This event occurred during none chargeable hours.
//		Interval start:2024-06-03T05:15
//		-----------------------------------
//		Time 06:15
//		Event type: DEDUCTED_ON_HOURLY_INTERVAL_EVENT_TYPE
//		Event description: The vehicle for this event is not toll free and the event occurred during chargeable hours but the amount is deducted due to another event whithin the same span of one hour with same or higher fee.
//		Interval start:2024-06-03T06:15
//		-----------------------------------
//		Time 12:30
//		Event type: DEDUCTED_ON_HOURLY_INTERVAL_EVENT_TYPE
//		Event description: The vehicle for this event is not toll free and the event occurred during chargeable hours but the amount is deducted due to another event whithin the same span of one hour with same or higher fee.
//		Interval start:2024-06-03T12:15
//		-----------------------------------
//		Time 15:20
//		Event type: DEDUCTED_ON_HOURLY_INTERVAL_EVENT_TYPE
//		Event description: The vehicle for this event is not toll free and the event occurred during chargeable hours but the amount is deducted due to another event whithin the same span of one hour with same or higher fee.
//		Interval start:2024-06-03T15:20
//		-----------------------------------

		
	}

}
