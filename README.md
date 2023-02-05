![here we are](https://media.giphy.com/media/FnGJfc18tDDHy/giphy.gif)

# Submission comments

Maven is used as a package manager to allow for JUnit testing. The actual logic does not require
Maven nor any extra packages as I was unsure if that was allowed. JAVA 19 is used, however one could
downgrade as long as java Records and Return-Switch are supported. Not really required, but made
for some cleaner code (=

I assume one hour has passed the moment the minuit difference >=60. So passing once at 07:00 and
then again 08:00 "sadly" results in two tolls being paid.
Furthermore, foreign vehicles are not exempted anymore, but trailers has been added.
Sadly, the calculator only works fully for the year 2023. Transportstyrelsen had no clear API to get
specific exemption date info continuously (from what I could tell).

# Toll fee calculator 1.0

A calculator for vehicle toll fees.

* Make sure you read these instructions carefully
* The current code base is in Java and C#, but please make sure that you do an implementation in a
  language **you feel comfortable** in like Javascript, Python, Assembler
  or [ModiScript](https://en.wikipedia.org/wiki/ModiScript) (please don't choose ModiScript).
* No requirement but bonus points if you know what movie is in the gif

## Background

Our city has decided to implement toll fees in order to reduce traffic congestion during rush hours.
This is the current draft of requirements:

* Fees will differ between 8 SEK and 18 SEK, depending on the time of day
* Rush-hour traffic will render the highest fee
* The maximum fee for one day is 60 SEK
* A vehicle should only be charged once an hour
    * In the case of multiple fees in the same hour period, the highest one applies.
* Some vehicle types are fee-free
* Weekends and holidays are fee-free

## Your assignment

The last city-developer quit recently, claiming that this solution is production-ready.
You are now the new developer for our city - congratulations!

Your job is to deliver the code and from now on, you are the responsible go-to-person for this
solution. This is a solution you will have to put your name on.

## Instructions

You can make any modifications or suggestions for modifications that you see fit. Fork this
repository and deliver your results via a pull-request. You could also create a gist, for privacy
reasons, and send us the link.

## Help I dont know C# or Java

No worries! We accept submissions in other languages as well, why not try it in Go or nodejs.

