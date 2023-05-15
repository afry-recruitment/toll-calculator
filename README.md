#  Toll Fee Calculator

Welcome to the Toll Fee Calculator. [Requirement document](ASSIGNMENT.md)

## Implementation

### 1 - Entity relationship diagram
   [ER-diagram](ER-diagram.png)

### 2 - Multitenancy
    The application is built as per the Multitenancy architecture. With the same instance, we will be able to configure N number of cities.

### 3 - Data are configurable
    Almost all the properties moved to the database. So any changes in the city can be modified easily.

### 4 - No tax for days before the public holiday and holiday month
    Additionally added before holiday and holiday month also no tax day or month which is again configurable.

### 5 - Change in tax charge pattern
    The tax charge for 18:30–05:59 is configued as 18:30 to 11:59 and 00:00 to 05:59 for better calculation.

### 6 - Extended support for all years (2013)
    The tax calculator will not serve only for 2013, but it also serves any data belonging to any year.

## Requirements

For building and running the application you need:

- [JDK 17](https://www.oracle.com/java/technologies/downloads/#java17)
- [Maven 3](https://maven.apache.org)
- if you want to use mysql local database then installed mysql (https://dev.mysql.com/downloads/mysql/) but currently toll fee application configured to free cloud database.

## Running the application locally

There are several ways to run a Spring Boot application on your local machine. One way is to execute the `main` method in the `com.tollcalculator.TollCalculatorApplication` class from your IDE.

Alternatively you can use the [Spring Boot Maven plugin](https://docs.spring.io/spring-boot/docs/current/reference/html/build-tool-plugins-maven-plugin.html) like so:

```shell
mvn spring-boot:run
```

## Postman collection

-[Import Postman Collection](Toll-Fee-Calculator.postman_collection.json)

## Swagger: API Documentation

-[Swagger API Endpoint](http://localhost:8080/swagger-ui/index.html)