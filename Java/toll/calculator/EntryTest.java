package toll.calculator;

import static org.junit.jupiter.api.Assertions.assertEquals;

import java.net.URL;
import java.net.http.HttpHeaders;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import org.junit.jupiter.api.Test;

import toll.calculator.models.Car;
import toll.calculator.models.Military;
import toll.calculator.models.TollTaxReqObject;
import toll.calculator.models.TollTaxResObject;
import toll.calculator.models.Vehicle;
import toll.calculator.services.TollTaxService;

class EntryTest {

	@Test
	void test() {
		System.out.println("This is the testcase in this class");
		String str1 = "This is the testcase in this class";
		assertEquals("This is the testcase in this class", str1);
	}

	// When wrong year given ( apart from 2013)
	@Test
	public void whenInvalidYear() throws Exception {

		Vehicle v2 = new Car();

		List<Date> datesList = new ArrayList<Date>();

		datesList.add(dateFormat("2012-02-08 16:06:00"));

		TollTaxReqObject ctcReq = new TollTaxReqObject();
		ctcReq.setVehicle(v2);
		ctcReq.setDates(datesList);
		ctcReq.setCity("got");

		TollTaxService ctcService = new TollTaxService();

		TollTaxResObject tollTaxResObject = ctcService.getTax(ctcReq);

		assertEquals("10000", tollTaxResObject.getErrorCode());
	}

	// DAY_OF_WEEK is SUNDAY time entry - 2013-01-06 SUNDAY
	@Test
	public void whenDayOfWeekSunday() throws Exception {
		Vehicle v2 = new Car();

		List<Date> datesList = new ArrayList<Date>();

		datesList.add(dateFormat("2013-01-06 10:00:00"));

		TollTaxReqObject ctcReq = new TollTaxReqObject();
		ctcReq.setVehicle(v2);
		ctcReq.setDates(datesList);
		ctcReq.setCity("got");

		TollTaxService ctcService = new TollTaxService();

		TollTaxResObject tollTaxResObject = ctcService.getTax(ctcReq);
		assertEquals(0, tollTaxResObject.getTotalTaxFee());
		// assertEquals("10000", tollTaxResObject.getErrorCode());

	}

	// DAY_OF_WEEK is Saturday time entry - 2013-01-05 Saturday
	@Test
	public void whenDayOfWeekSaturday() throws Exception {

		Vehicle v2 = new Car();

		List<Date> datesList = new ArrayList<Date>();

		datesList.add(dateFormat("2013-01-05 10:00:00"));

		TollTaxReqObject ctcReq = new TollTaxReqObject();
		ctcReq.setVehicle(v2);
		ctcReq.setDates(datesList);
		ctcReq.setCity("got");

		TollTaxService ctcService = new TollTaxService();

		TollTaxResObject tollTaxResObject = ctcService.getTax(ctcReq);
		assertEquals(0, tollTaxResObject.getTotalTaxFee());
		// assertEquals("10000", tollTaxResObject.getErrorCode());

	}

	@Test
	public void whenHoliday() throws Exception {

		Vehicle v2 = new Car();

		List<Date> datesList = new ArrayList<Date>();

		datesList.add(dateFormat("2013-01-01 10:00:00"));

		TollTaxReqObject ctcReq = new TollTaxReqObject();
		ctcReq.setVehicle(v2);
		ctcReq.setDates(datesList);
		ctcReq.setCity("got");

		TollTaxService ctcService = new TollTaxService();

		TollTaxResObject tollTaxResObject = ctcService.getTax(ctcReq);
		assertEquals(0, tollTaxResObject.getTotalTaxFee());
		// assertEquals("10000", tollTaxResObject.getErrorCode());

	}

	@Test
	public void whenMilitaryVehiclePass() throws Exception {

		Vehicle v2 = new Military();

		List<Date> datesList = new ArrayList<Date>();

		datesList.add(dateFormat("2013-01-07 10:00:00"));

		TollTaxReqObject ctcReq = new TollTaxReqObject();
		ctcReq.setVehicle(v2);
		ctcReq.setDates(datesList);
		ctcReq.setCity("got");

		TollTaxService ctcService = new TollTaxService();

		TollTaxResObject tollTaxResObject = ctcService.getTax(ctcReq);
		assertEquals(0, tollTaxResObject.getTotalTaxFee());
		// assertEquals("10000", tollTaxResObject.getErrorCode());

	}

	// Month of July entry - 2013-07-20 10:00:00
	@Test
	public void testJulyEntry() throws Exception {

		Vehicle v2 = new Car();

		List<Date> datesList = new ArrayList<Date>();

		datesList.add(dateFormat("2013-07-20 10:00:00"));

		TollTaxReqObject ctcReq = new TollTaxReqObject();
		ctcReq.setVehicle(v2);
		ctcReq.setDates(datesList);
		ctcReq.setCity("got");

		TollTaxService ctcService = new TollTaxService();

		TollTaxResObject tollTaxResObject = ctcService.getTax(ctcReq);
		assertEquals(0, tollTaxResObject.getTotalTaxFee());
		// assertEquals("10000", tollTaxResObject.getErrorCode());

	}

	// when week day - 2013-01-14 09:00:00 Saturday
	@Test
	public void whenWeekday09AM() throws Exception {
		Vehicle v2 = new Car();

		List<Date> datesList = new ArrayList<Date>();

		datesList.add(dateFormat("2013-01-14 09:00:00"));

		TollTaxReqObject ctcReq = new TollTaxReqObject();
		ctcReq.setVehicle(v2);
		ctcReq.setDates(datesList);
		ctcReq.setCity("got");

		TollTaxService ctcService = new TollTaxService();

		TollTaxResObject tollTaxResObject = ctcService.getTax(ctcReq);
		assertEquals(8, tollTaxResObject.getTotalTaxFee());
		// assertEquals("10000", tollTaxResObject.getErrorCode());

	}

	// when week day time entry - 2013-01-07 16:00:00 Saturday
	@Test
	public void whenWeekday16PM() throws Exception {

		Vehicle v2 = new Car();

		List<Date> datesList = new ArrayList<Date>();

		datesList.add(dateFormat("2013-01-07 15:00:00"));

		TollTaxReqObject ctcReq = new TollTaxReqObject();
		ctcReq.setVehicle(v2);
		ctcReq.setDates(datesList);
		ctcReq.setCity("got");

		TollTaxService ctcService = new TollTaxService();

		TollTaxResObject tollTaxResObject = ctcService.getTax(ctcReq);
		assertEquals(13, tollTaxResObject.getTotalTaxFee());
		// assertEquals("10000", tollTaxResObject.getErrorCode());

	}

	@Test
	public void whenWeekday1659AM() throws Exception {

		Vehicle v2 = new Car();

		List<Date> datesList = new ArrayList<Date>();

		datesList.add(dateFormat("2013-01-07 16:59:00"));

		TollTaxReqObject ctcReq = new TollTaxReqObject();
		ctcReq.setVehicle(v2);
		ctcReq.setDates(datesList);
		ctcReq.setCity("got");

		TollTaxService ctcService = new TollTaxService();

		TollTaxResObject tollTaxResObject = ctcService.getTax(ctcReq);
		assertEquals(18, tollTaxResObject.getTotalTaxFee());
		// assertEquals("10000", tollTaxResObject.getErrorCode());

	}

	@Test
	public void whenWeekDay165959AM() throws Exception {

		Vehicle v2 = new Car();

		List<Date> datesList = new ArrayList<Date>();

		datesList.add(dateFormat("2013-01-07 16:59:59"));

		TollTaxReqObject ctcReq = new TollTaxReqObject();
		ctcReq.setVehicle(v2);
		ctcReq.setDates(datesList);
		ctcReq.setCity("got");

		TollTaxService ctcService = new TollTaxService();

		TollTaxResObject tollTaxResObject = ctcService.getTax(ctcReq);
		assertEquals(18, tollTaxResObject.getTotalTaxFee());
		// assertEquals("10000", tollTaxResObject.getErrorCode());

	}

	// when the two entries in 60min
	@Test
	public void whenTwoEntriesWithIn60M() throws Exception {
		Vehicle v2 = new Car();

		List<Date> datesList = new ArrayList<Date>();

		datesList.add(dateFormat("2013-01-07 16:30:59"));

		TollTaxReqObject ctcReq = new TollTaxReqObject();
		ctcReq.setVehicle(v2);
		ctcReq.setDates(datesList);
		ctcReq.setCity("got");

		TollTaxService ctcService = new TollTaxService();

		TollTaxResObject tollTaxResObject = ctcService.getTax(ctcReq);
		assertEquals(18, tollTaxResObject.getTotalTaxFee());
		// assertEquals("10000", tollTaxResObject.getErrorCode());

	}

	@Test
	public void whenTwoEntriesNotIn60M() throws Exception {

		Vehicle v2 = new Car();

		List<Date> datesList = new ArrayList<Date>();

		datesList.add(dateFormat("2013-01-07 15:30:59"));
		datesList.add(dateFormat("2013-01-07 16:59:59"));

		TollTaxReqObject ctcReq = new TollTaxReqObject();
		ctcReq.setVehicle(v2);
		ctcReq.setDates(datesList);
		ctcReq.setCity("got");

		TollTaxService ctcService = new TollTaxService();

		TollTaxResObject tollTaxResObject = ctcService.getTax(ctcReq);
		assertEquals(36, tollTaxResObject.getTotalTaxFee());
		// assertEquals("10000", tollTaxResObject.getErrorCode());

	}

	@Test
	public void testMaxAmtPerVehicle() throws Exception {

		Vehicle v2 = new Car();

		List<Date> datesList = new ArrayList<Date>();

		datesList.add(dateFormat("2013-01-07 06:20:59"));
		datesList.add(dateFormat("2013-01-07 07:25:59"));
		datesList.add(dateFormat("2013-01-07 08:27:59"));
		datesList.add(dateFormat("2013-01-07 09:35:59"));

		datesList.add(dateFormat("2013-01-07 10:45:59"));
		datesList.add(dateFormat("2013-01-07 12:45:59"));

		datesList.add(dateFormat("2013-01-07 15:30:59"));
		datesList.add(dateFormat("2013-01-07 16:59:59"));

		TollTaxReqObject ctcReq = new TollTaxReqObject();
		ctcReq.setVehicle(v2);
		ctcReq.setDates(datesList);
		ctcReq.setCity("got");

		TollTaxService ctcService = new TollTaxService();

		TollTaxResObject tollTaxResObject = ctcService.getTax(ctcReq);
		assertEquals(60, tollTaxResObject.getTotalTaxFee());
		// assertEquals("10000", tollTaxResObject.getErrorCode());

	}

	@Test
	public void whenTollFreeTime() throws Exception {

		Vehicle v2 = new Car();

		List<Date> datesList = new ArrayList<Date>();

		datesList.add(dateFormat("2013-01-09 23:00:00"));

		TollTaxReqObject ctcReq = new TollTaxReqObject();
		ctcReq.setVehicle(v2);
		ctcReq.setDates(datesList);
		ctcReq.setCity("got");

		TollTaxService ctcService = new TollTaxService();

		TollTaxResObject tollTaxResObject = ctcService.getTax(ctcReq);
		assertEquals(0, tollTaxResObject.getTotalTaxFee());
		// assertEquals("10000", tollTaxResObject.getErrorCode());

	}

	private static Date dateFormat(String sDate1) throws ParseException {
		return new SimpleDateFormat("yyyy-mm-dd HH:mm:ss").parse(sDate1);
	}
}
