First of all, the gif is from the movie Hackers (1995) 

Some changes and fixes in the code;
1.	There was no main() method, which tells the JVM where to start program execution.
To fix that I addede a class called Main, which contains the main() method as well as code to test that the program works well.

2.	In the TollCalculator Class, there were some an issue in the methód getTollFee(Vehicle vehicle, Date... dates).
First, in the case when there are multiple vehicle passes during the day and some of the ocurres in the same hour.
 The program should add only one pass of them which has the highest value. This was working fine if these passes ocure first thing of the day.
 But if there was some passes first and then comes passes that ocure in the same hour, then the program added all the values in that hour.
I made some changes to fix the problem.
3.	I made changes to test if the vehicle is belong to a type that shouls pass free of charge
4.	Other minor changes were done and shown in comments in the code
