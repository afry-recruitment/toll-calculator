package com.afry.test.tollcalculator;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.context.properties.EnableConfigurationProperties;

import com.afry.test.tollcalculator.config.TollConfig;

@SpringBootApplication
@EnableConfigurationProperties(TollConfig.class)
public class TollCalculatorApplication {

	public static void main(String[] args) {
		SpringApplication.run(TollCalculatorApplication.class, args);
	}

}
