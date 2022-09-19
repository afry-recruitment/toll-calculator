package com.afry.test;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.PropertySource;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;

/**
 * TollCalculatorApplication - Provide Toll Gate API services
 * Bind With H2 Database and initiate Database TollCalculatorApplication start.
 * Secured with Spring boot WebSecurity
 */
@PropertySource("classpath:application-h2.properties")
@SpringBootApplication
@EnableWebSecurity
public class TollCalculatorApplication {
	/**
	 * SpringApplication -Entry point to start API services
	 * @param args
	 */
	public static void main(String[] args) 
	{
		SpringApplication.run(TollCalculatorApplication.class, args);
	}
}
