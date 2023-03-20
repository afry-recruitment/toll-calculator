package toll.calculator;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import toll.calculator.models.Car;
import toll.calculator.models.TollTaxReqObject;
import toll.calculator.models.TollTaxResObject;
import toll.calculator.models.Vehicle;
import toll.calculator.services.TollTaxService;

public class Entry {
	public static void main(String[] args) throws ParseException {
		TollTaxResObject tollTaxResObject = new TollTaxResObject();
		// Vehicle v2 = new Emergency();
		Vehicle v2 = new Car();

		List<Date> datesList = new ArrayList<Date>();
		try {
			datesList.add(dateFormat("2013-02-08 16:06:00"));// 18
			datesList.add(dateFormat("2013-02-08 17:05:00"));// 13 59 fee
			datesList.add(dateFormat("2013-02-08 18:04:00"));// 8
			TollTaxReqObject ctcReq = new TollTaxReqObject();
			ctcReq.setVehicle(v2);
			ctcReq.setDates(datesList);
			ctcReq.setCity("got");

			TollTaxService ctcService = new TollTaxService();

			tollTaxResObject=ctcService.getTax(ctcReq);
			
		} catch (ParseException e) {
			tollTaxResObject = new TollTaxResObject("10001", "invalid date", 0, "Invalid date: the expected format is yyyy-MM-dd HH:mm:ss.");
			
		}
		System.out.println(tollTaxResObject.toString());
		

		/**
		 * 06.00T06.29=8 06.30T06.59=13 07.00T07.59=18 08.00T08.29=13 08.30T14.59=8
		 * 15.00T15.29=13 15.30T16.59=18 17.00T17.59=13 18.00T18.29=8 18.30T23.59=0
		 * 00.00T05.59=0
		 */

	}

	private static Date dateFormat(String sDate1) throws ParseException {
		return new SimpleDateFormat("yyyy-mm-dd HH:mm:ss").parse(sDate1);
	}
}
