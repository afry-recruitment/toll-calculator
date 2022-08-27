
package Rayar.AFRY.com;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;

public class Afrysolution {

	public static void main(String[] args) throws ParseException {

		TollCalculator tollcal = new TollCalculator();
		
		
		//can implement to take input by console
		/*Scanner sc = new Scanner(System.in);
		String date =sc.nextLine(); */
		
		//Example Test data
		String d1 = "Fri Aug 26 15:20:11 CEST 2022";
		String d2 = "Fri Aug 26 10:19:11 CEST 2022";
		String d3 = "Fri Aug 26 13:30:11 CEST 2022";
		String d4 = "Fri Aug 26 16:29:11 CEST 2022";
		String d5 = "Fri Aug 26 17:29:11 CEST 2022";

		Date date1 = new SimpleDateFormat("EEE MMM d HH:mm:ss z yyyy").parse(d1);
		Date date2 = new SimpleDateFormat("EEE MMM d HH:mm:ss z yyyy").parse(d2);
		Date date3 = new SimpleDateFormat("EEE MMM d HH:mm:ss z yyyy").parse(d3);
		Date date4 = new SimpleDateFormat("EEE MMM d HH:mm:ss z yyyy").parse(d4);
		Date date5 = new SimpleDateFormat("EEE MMM d HH:mm:ss z yyyy").parse(d5);
		
		int totalFee = tollcal.getTollFee(new Diplomat(), date1,date2,date3,date4,date5);

		System.out.println("Total toll for a day " + totalFee);

	}

}
