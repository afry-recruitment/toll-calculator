package calculations;

import java.util.*;
import typeOfVehicle.TollFreeVehicles;
import typeOfVehicle.Vehicle;


public class TollCalculator {

	/**
	 * Return the total toll fee for one day
	 *
	 * @param vehicle - the vehicle
	 * @param dates   - date and time of all passes,
	 * 			if the next day is included, program does not calculate those fees
	 * @return - If it is weekend(actual year) or holiday(for 2022) then return fee=0.
	 * 			It call calculateTotalFee.returnTotalFee to get and then return fee for that day
	 */
	public int getTollFee(Vehicle vehicle, Date... dates) {

		IsTollFreeDate isFreeDate = new IsTollFreeDate();
		CalculateTotalFee calculateTotalFee = new CalculateTotalFee();

		for (int i = 0; i < dates.length; i++) {
			System.out.println("Number of entries: " + (i + 1) + ". , date and time: " + dates[i]);
		}

		String typVehicle = vehicle.getType();
		System.out.format("Typ of vehicle: %s \n\n ", typVehicle);

		Date startUse = dates[0];

		int totalFee = 0;

		// if vehicle is in enum then totalFee=0
		for (TollFreeVehicles enumValue : TollFreeVehicles.values()) {
			// System.out.println(enumValue);
			if (enumValue.getType().equalsIgnoreCase(vehicle.getType())) {
				System.out.format("%s is free of payment. Fee is %d: ", vehicle.getType(), totalFee);
				return totalFee = 0;
			}
		}

		// if it is weekend or holiday, fee=0
		if (isFreeDate.isTollFreeDate(startUse).equals(true)) {
			System.out.println("It is weekend or holiday! It is free.");
			return totalFee = 0;
		}

		return calculateTotalFee.returnTotalFee(dates);

	}
}
