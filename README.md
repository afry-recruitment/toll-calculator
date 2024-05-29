# Toll fee calculator 2.0
A calculator for vehicle toll fees.

New vehicle toll fee calculator provides a RESTful API for calculating the toll fees. 

Current repositiry language - Python

## Running the code

Recommended way to run this is to use [docker](https://docs.docker.com/)

run 
```
docker build -t tollcalc .
docker run -d --name tollcalc -p 8080:8080 tollcalc
```
to start the API at your local machine at port 8080 (or any other of your choosing)

Navigate to http://localhost:8080/docs to see the Open API spec for the application

## Making further adjustments

In case if the time table for the fee changes, it can be adjusted in the toll_calculator module.

## Development

Activate python virtual env and install requirements-dev.txt  
Install pre-commit for unittests and black styling on commit 

```
pip install -r requirements-dev.txt
pre-commit install
```

![the movie is Hackers, but I haven't seen it, I guess I'll have to](https://i.giphy.com/media/v1.Y2lkPTc5MGI3NjExY29jdzFwYWF1MWU4d3J0ODUyM2YxZjUzamVkb3Fxa29nYjlkZGxjNyZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/aI7y2ZdVImJdhYOjLn/giphy.gif)

## Find below old documentation and task description 

![here we are](https://media.giphy.com/media/FnGJfc18tDDHy/giphy.gif)

# Toll fee calculator 1.0
A calculator for vehicle toll fees.

* Make sure you read these instructions carefully
* The current code base is in Java and C#, but please make sure that you do an implementation in a language **you feel comfortable** in like Javascript, Python, Assembler or [ModiScript](https://en.wikipedia.org/wiki/ModiScript) (please don't choose ModiScript). 
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

Your job is to deliver the code and from now on, you are the responsible go-to-person for this solution. This is a solution you will have to put your name on. 

## Instructions
You can make any modifications or suggestions for modifications that you see fit. Fork this repository and deliver your results via a pull-request. You could also create a gist, for privacy reasons, and send us the link.

## Help I dont know C# or Java
No worries! We accept submissions in other languages as well, why not try it in Go or nodejs.

