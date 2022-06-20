package main;

import java.time.*;

import vehicles.*;

public class Main {

	public static void main(String[] args) {
		TollCalculator a = new TollCalculator(2022);

		Car c = new Car();
		
		LocalDateTime[] b = {LocalDateTime.parse("2022-06-17T08:20:00"),
							LocalDateTime.parse("2022-06-17T11:00:20"),
							LocalDateTime.parse("2022-06-17T12:00:15"),
							LocalDateTime.parse("2022-06-17T12:02:19"),
							LocalDateTime.parse("2022-06-17T15:10:22"),
							LocalDateTime.parse("2022-06-17T15:10:28"),
							LocalDateTime.parse("2022-06-17T16:12:22"),
							LocalDateTime.parse("2022-06-17T16:13:24"),
							LocalDateTime.parse("2022-06-17T17:14:30"),
							LocalDateTime.parse("2022-06-17T17:14:35"),
							LocalDateTime.parse("2022-06-17T17:14:39")};
					
		System.out.println(a.getTotalDailyFee(c, b));
		
		//a.getAPIfields();
		
	}

}
