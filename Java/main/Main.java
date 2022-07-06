package main;

import java.time.*;
import java.util.HashSet;

import vehicles.*;

public class Main {

	public static void main(String[] args) {
		TollCalculator a = new TollCalculator(2022);

		Car c = new Car();
		
		LocalDateTime[] b = {LocalDateTime.parse("2022-06-06T08:20:00"),
							LocalDateTime.parse("2022-06-06T11:00:20"),
							LocalDateTime.parse("2022-06-06T12:00:15"),
							LocalDateTime.parse("2022-06-06T15:10:28")};
		
		LocalDateTime[] k = {};
					
		System.out.println(a.getTotalDailyFee(c, b));
		
		
		
	}

}
