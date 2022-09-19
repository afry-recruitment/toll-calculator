
# Toll fee calculator 1.0
This is Provide Toll Fee Calculator API Data service to API consumers. Tech Stak based on Java / Spring boot to receive  API request and provide response.

### Prior Requirement
•	Java 8 +

•	Maven

•	Postman / Any REST Client For Testing 

### Installation and run application
1. Pull repository for GitHub (From Pull request or fork branch -https://github.com/fbagim/toll-calculator 
2. Execute Maven Build inside project
    > mvn clean install
    
3. To start API application please find jar file under build path - <project>/target/
 
    >  To Start application run command   Java -jar  Tollfeecalculator-1.0.jar
    
    API services will start on localhost with this url http://localhost:9080/tollFeeCalculatorApi/calculator

 ### To access Calculator API
 
 1. Start Postman / Any REST Client.
 
 2. Application secured with basic Auth since you need to set credentials on Postman to access API.
    > User Name -admin , Password – root
    
      ![image](https://user-images.githubusercontent.com/7611920/190954417-37e31327-4798-4b48-9b06-91f90d91bfc2.png)

    To access Calculator end point , its required to use  Api use below payload and send request on POST method to API end point.
    
    > Endpoint - http://localhost:9080/tollFeeCalculatorApi/calculator
    
    > Payload 
    
         {
           "vehicleId":"CNC12122",
            "type":"Car"
          }
 
     > Required Fields – vehicleId (This Should be 8 characters) , type
     
     > Response will be
     
        {
            "tollFee": 18,
            "type": "Car",
            "vehicleId": "CNC12122"
        }
        
       ![image](https://user-images.githubusercontent.com/7611920/190954850-89b78e8a-dd0d-4884-a2f9-fd8f2402d7ce.png)
  
    
    > Payload for Toll Free Vehicle

       {
        "vehicleId":"CNC12122",
         "type":"MotorBike"
       }
    
    > Response 

      CNC12122 is Toll Free Vehicle
 
     ![image](https://user-images.githubusercontent.com/7611920/190955480-c41abd6a-b235-415b-bc04-7290f28c798b.png)
 
    To try out Toll Free Date calculation please change system date to Saturday or Sunday.
 
    > Payload for Toll Free Vehicle

       {
         "vehicleId":"CNC12122",
          "type":"Car"
       }
    > Response
       Today is Toll Free Day
 
     ![image](https://user-images.githubusercontent.com/7611920/190956311-db498f08-a266-4e91-b108-fd247a39af92.png)
 
    ####   Please note these all calculation will be based on real time date and time . internally calculate all inputs according to the requested time.
    ####   Please note these all scarios are coverd in JUnit Test class - 
    toll-calculator/Java/src/test/java/com/afry/test/service/TollCalculatorServiceTest.java 
    
     1.  To try out different toll fees between 8 SEK and 18 SEK, depending on the time of day please change System time and try out results.
     2.  To try out Rush-hour highest fees,  please change System time to hours between 10 to 12 or 16 to 18 - please note this is confoigurable from sql script toll-            calculator/Java/src/main/resources/data-h2.sql
     3.  To try out maximum fee for one day (60 SEK)  please change System time and try out multiple request to API .
     4.  To try out In the case of highest fees in the same hour period please set system time to 6 to 6.30 and 6.30 to 7.00 and make requests to API
 
   
      
      
## Design and Improvements 
  
 ![image](https://user-images.githubusercontent.com/7611920/190962361-bc5354a1-2703-4b72-b4e0-aac6b19d7c5d.png)


 1. All APi calls direct to calculator controller , once after security validations  and request data validations request send to Vehicle factory
   ( used Factory     pattern to generate vehicle) to validate Vehicle object and generate  Vehicle Entity.
 
 2. Calculator service class doing all calculations based on algorithm on time.
 
 3. Vehicle  repositories  persist all vehicles and  toll entries per vehicle . to calculation its calling history records and filter toll entries belongs to same day     calculate .
 
 4. This application based on spring basic Auth security . this can be improved by adding centralized security mechanism with JWT Oauth2 provider .
 
 5. For the production level its need to move H2 data base to SQL or NoSQL scalable  database stores.


 

