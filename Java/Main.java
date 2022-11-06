
import java.util.Calendar;
import java.util.Date;

/*There was no main() method, which tells the JVM where to start program execution.
To fix that I added a class called Main, which contains the main() method 
as well as code to test that the program works well.*/


public class Main {
	
public static void main(String[] args) { //the main() method, to start executing the program
	
		
/*	Testing the program by creating a Car object to insert it as an argument
 *  to a TollCalculator object with random time*/
	  
        Car car1 = new Car(); //Create an instance of the Car class
       
        //car1.setType("Tractor");
		Calendar cal = Calendar.getInstance();
		cal.set(Calendar.HOUR_OF_DAY,7);
		cal.set(Calendar.MINUTE,10);
		cal.set(Calendar.SECOND,0);
		cal.set(Calendar.MILLISECOND,0);
		Date d = cal.getTime();
		//System.out.println(d.toString());
		
		Calendar cal1 = Calendar.getInstance();
		cal1.set(Calendar.HOUR_OF_DAY,8);
		cal1.set(Calendar.MINUTE,20);
		cal1.set(Calendar.SECOND,0);
		cal1.set(Calendar.MILLISECOND,0);
		Date d1 = cal1.getTime();
		//System.out.println(d1.toString());
		
		Calendar cal2 = Calendar.getInstance();
		cal2.set(Calendar.HOUR_OF_DAY,15);
		cal2.set(Calendar.MINUTE,55);
		cal2.set(Calendar.SECOND,0);
		cal2.set(Calendar.MILLISECOND,0);
		Date d2 = cal2.getTime();
				
		Calendar cal3 = Calendar.getInstance();
	        cal3.set(Calendar.HOUR_OF_DAY,16);
	        cal3.set(Calendar.MINUTE,50); 
	        cal3.set(Calendar.SECOND,0);
	        cal3.set(Calendar.MILLISECOND,0);
		Date d3 = cal3.getTime();
		  
		
		TollCalculator test = new TollCalculator(); //Create an instance of TollCalculator class
		
		//String s = TollCalculator.TollFreeVehicles.DIPLOMAT.toString();
		
		System.out.println(car1.type); 
						
		System.out.println(test.getTollFee(car1,d,d1, d2,d3 )); //Print the result
		
	}

}
