package calculations;

import java.util.ArrayList;
import java.util.Date;


/**
 * 
 * @author danijela
 * This class return amount of total fee per day.
 * Method returnFee is controlling is rush hour or not
 * Method checkIsTotalFeeBigerThanMaximum is controlling amount of total fee. If total fee based on
 * number of entries is bigger than 60sek, than method fix that is charged only 60sek
 * 
 * This class can be improved by using GregorianCalendar.getInstance() methods instead of Date method 
 * that are depreciate
 * 
 *
 */
public class CalculateTotalFee {
	public int returnTotalFee(Date[] dates2) {

		ArrayList<Integer> totalMinutes = new ArrayList<Integer>();
		ArrayList<Double> hours = new ArrayList<Double>();

		// make two list that take care about minutes and hours wher vehicles entries the
		// road
		for (int k = 0; k < dates2.length; k++) {
			totalMinutes.add(dates2[k].getMinutes() + dates2[k].getHours() * 60);
			hours.add((double) (dates2[k].getHours()) + (dates2[k].getMinutes() * 0.5 / 30));

		}

		int newFee;
		// list of fee, every time when vehicle get to road it add fee to this list
		ArrayList<Integer> totalFee = new ArrayList<Integer>();
			if(returnFee(hours.get(1))>8)
			totalFee.add(returnFee(hours.get(1)));
			else
				totalFee.add(returnFee(hours.get(0)));

		//System.out.println("Total minuts on the road"+totalMinutes);
	//	System.out.println("nummber of entery " + totalMinutes.size());

		int i = 0, j = 1, x = 0;

		// check is time between two entries under 60minutes
		while (x < totalMinutes.size() - 1) {
			if ((totalMinutes.get(j) - totalMinutes.get(i)) < 60) {
				x++;
				j++;
				//System.out.println(j);
			} else {
				// if difference between entries is bigger then 60 minutes
				x++;
				i =x;
				j = x + 1;
				
			//	System.out.println(i+"     "+j+"    "+x);
				// take care of amount of fee.
				// if amount of fee is bigger in another entry
				// because the rush hour starts)
				// then program use that biger fee				
				totalFee.add(returnFee(hours.get(i)));
			}
		}

		System.out.println(totalFee);

		//get ampunt of total fee
		int sum = 0;
		for (int t = 0; t < totalFee.size(); t++) {
			sum = sum + totalFee.get(t);

		}

		System.out.format("Total fee is: %d", sum);

		// if totalFee is > 60 sek, only 60sek is returned
		return checkIsTotalFeeBigerThanMaximum(sum);

	}
	//control is totalFee bigger then 60sek
	private int checkIsTotalFeeBigerThanMaximum(int sum) {
		if (sum > 60) {
			System.out.println("\n Return totalfee= " + 60);
			return 60;
		} else {
			System.out.println("\n Return totalfee= " + sum);
			return sum;
		}
		
	}

	// control is it rush hour
	//because we have only two prices 8 and 18 we do not need to use big if/else statements
	private int returnFee(double hours) {

		return (((7.0 < hours && hours < 9.0) || (16.50 < hours && hours < 18.0)) ? 18 : 8);
	}

}