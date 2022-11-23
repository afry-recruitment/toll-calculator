import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.Calendar;
import java.util.Date;

import calculations.TollCalculator;
import typeOfVehicle.Car;
import typeOfVehicle.Diplomat;
import typeOfVehicle.Emergency;
import typeOfVehicle.Foreign;
import typeOfVehicle.Military;
import typeOfVehicle.Motorbike;
import typeOfVehicle.Tractor;

/**
 * 
 * @author danijela
 * this can be improved in a way that we have the possibility to enter the date and time ourselves,
 *  as we do with the type of vehicle. now the program allows entering the road only 6 times. 
 *  The program can be improved on the way that we can enter the number of entrances ourselves.
 *  
 *  
 * Since the vehicle class returns only the vehicle type, instead of the class, we could perhaps use only an enum
 *
 *structure should be /src/main/packages../classes   and src/test/packages../classes
 *To improve documentations, javadoc jar should be included into path
 */
public class Main {

	public static void main(String[] args) {
		Motorbike motorbike = new Motorbike();
		Tractor tractor = new Tractor();
		Emergency emergency = new Emergency();
		Diplomat diplomat = new Diplomat();
		Foreign foregin = new Foreign();
		Military military = new Military();
		Car car = new Car();

		TollCalculator tollCalculatorCa = new TollCalculator();
		Calendar dateNow = Calendar.getInstance();
		String vehicle = null;

		Date dateStart = null, entry2 = null, entry3 = null, entry4 = null, entry5 = null, dateEnd = null;

		// first entry
		//dateNow.add(Calendar.HOUR, -6);
		dateNow.add(Calendar.HOUR, 3);
		
		dateStart = dateNow.getTime();
		 System.out.println(("First entry" + dateStart));

		// second entry
		dateNow.add(Calendar.MINUTE,22);
		entry2 = dateNow.getTime();
		// System.out.println((entry2));

		// 3 entry
		dateNow.add(Calendar.MINUTE, 40);
		entry3 = dateNow.getTime();
		// System.out.println((entry3));

		// 4 entry
		dateNow.add(Calendar.HOUR, 5);
		entry4 = dateNow.getTime();
		// System.out.println((entry4));
//
		// 5 entry
		dateNow.add(Calendar.MINUTE, 55);
		entry5 = dateNow.getTime();
		// System.out.println((entry5));

		// last entry
		dateNow.add(Calendar.MINUTE, 59);
		// dateNow.add(Calendar.HOUR, 15);
		dateEnd = dateNow.getTime();
		// System.out.println("Last entry : " + dateEnd + "\n");

		// take care if some of entries do not exist
		if(dateStart==null) {
				System.out.println("You didn't use the road today!");
				System.exit(0);}
		if (entry2 == null)
			entry2 = dateStart;
		if (entry3 == null)
			entry3 = entry2;
		if (entry4 == null)
			entry4 = entry3;
		if (entry5 == null)
			entry5 = entry4;
		if (dateEnd == null)
			dateEnd = entry5;
		
		//System.out.println("empty time"+"\n"+dateStart+"\n"+entry2+"\n"+entry3+"\n"+entry4+"\n"+entry5+"\n"+dateEnd);

//		

		System.out.println("Vehicle to choose: \n" + "			1.Car\n" + "			2.Motorbike\n"
				+ "			3.Military\n" + "			4.Tractor\n" + "			5.Emergency\n"
				+ "			6.Diplomat\n" + "			7.Foregin\n" + "Write \"Q\" to exit!");

		BufferedReader br = new BufferedReader(new InputStreamReader(System.in));
		
		try {
			try {
				do {
					System.out.println("\nWrite type of vehicle:\n");
					vehicle = (br.readLine());
					if (vehicle.equalsIgnoreCase("car")) {
						tollCalculatorCa.getTollFee(car, dateStart, entry2, entry3, entry4, entry5, dateEnd);
					} else if (vehicle.equalsIgnoreCase("military"))
						tollCalculatorCa.getTollFee(military, dateStart, entry2, entry3, entry4, entry5, dateEnd);
					else if (vehicle.equalsIgnoreCase("motorbike"))
						tollCalculatorCa.getTollFee(motorbike, dateStart, entry2, entry3, entry4, entry5, dateEnd);
					else if (vehicle.equalsIgnoreCase("tractor"))
						tollCalculatorCa.getTollFee(tractor, dateStart, entry2, entry3, entry4, entry5, dateEnd);
					else if (vehicle.equalsIgnoreCase("emergency"))
						tollCalculatorCa.getTollFee(emergency, dateStart, entry2, entry3, entry4, entry5, dateEnd);
					else if (vehicle.equalsIgnoreCase("diplomat"))
						tollCalculatorCa.getTollFee(diplomat, dateStart, entry2, entry3, entry4, entry5, dateEnd);
					else if (vehicle.equalsIgnoreCase("foregin"))
						tollCalculatorCa.getTollFee(foregin, dateStart, entry2, entry3, entry4, entry5, dateEnd);
					else {
						if (vehicle.equalsIgnoreCase("Q")) {
							System.out.println("Thank you for using our program ang see you again!");
						} else {
							System.out.println("THat vehicle do not exist.\n");
						}
					}
				} while (!vehicle.equalsIgnoreCase("Q"));
			} catch (IOException e) {
				e.printStackTrace();
			}
		} catch (NumberFormatException nfe) {
			System.out.println(nfe);
		}

	}

}
