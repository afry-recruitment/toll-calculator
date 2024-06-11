package model;

import java.time.Duration;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

import model.toll.event.TollEvent;

/**
 * Representation of a vehicles daily toll fee report. Includes deductive toll fees as well that for one or another reason should be excluded.
 */
public class DailyVehicleReport {

	private List<TollEvent> noneChargedTollEvent = new ArrayList<TollEvent>();

	private List<TollEvent> chargedTollEvent = new ArrayList<TollEvent>();

	private int totalDiscountForMaxAmount;

	private LocalDateTime reportDate;

	public int getTotalAmountToPay() {
		int total = 0;

		List<TollEvent> chargedTollEvents = this.getChargedTollEvent();

		for (TollEvent tollEvent : chargedTollEvents) {
			total = total + tollEvent.getTollFee().getAmountToPay();
		}

		return total;
	}

	public int getTotalDiscountForMaxAmount() {
		return totalDiscountForMaxAmount;
	}

	public void setTotalDiscountForMaxAmount(int totalDiscountForReachingMaxAmount) {
		this.totalDiscountForMaxAmount = totalDiscountForReachingMaxAmount;
	}

	public List<TollEvent> getNoneChargedTollEvent() {
		return noneChargedTollEvent;
	}

	public void setNoneChargedTollEvent(List<TollEvent> noneChargedTollEvent) {
		this.noneChargedTollEvent = noneChargedTollEvent;
	}

	public List<TollEvent> getChargedTollEvent() {
		return chargedTollEvent;
	}

	public void setChargedTollEvent(List<TollEvent> chargedTollEvent) {
		this.chargedTollEvent = chargedTollEvent;
	}

	public LocalDateTime getReportDate() {
		return reportDate;
	}

	public void setReportDate(LocalDateTime reportDate) {
		this.reportDate = reportDate;
	}

	
}
