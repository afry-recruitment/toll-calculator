import com.toll.controller.TollCalculator;
import com.toll.model.Car;
import com.toll.model.Vehicle;

import java.util.Calendar;
import java.util.Date;

public class TestTollCalculator {
    public static void main(String[] args){
        TollCalculator calc=new TollCalculator();
        Vehicle veh=new Car();
        veh.setNumber("100");
        Calendar c1 = Calendar.getInstance();


        c1.set(2022,8,21,8,15);

        // creating a date object with specified time.
        Date dateOne = c1.getTime();

        System.out.println("Date initially: "
                + dateOne);
        System.out.println("get toll 1st time: "+calc.getTollFee(veh, dateOne)+ " SEK");


        Calendar c2 = Calendar.getInstance();


        c1.set(2022,8,21,8,18);

        // creating a date object with specified time.
        Date dateOne1 = c1.getTime();

        System.out.println("Date initially: " + dateOne1);
        System.out.println("get toll 2nd time: "+calc.getTollFee(veh, dateOne1)+ " SEK");

        Calendar c3 = Calendar.getInstance();


        c1.set(2022,8,21,23,17);

        // creating a date object with specified time.
        Date dateOne2 = c1.getTime();

        System.out.println("Date initially: "
                + dateOne2);


        System.out.println("get toll 3rd time: "+calc.getTollFee(veh, dateOne2)+ " SEK");
    }

}
