package com.assignment.toll_calculator.config;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.PropertyNamingStrategies;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

/**
 * This class is for configuration the serialization options.
 */
@Configuration
public class SerializationConfig {

    /**
     * This bean makes object mapping of the application understand and produce snake case instead of camel case fields.
     * @return - the configured object mapper
     */
    @Bean
    public ObjectMapper objectMapper() {
        return new ObjectMapper().setPropertyNamingStrategy(new PropertyNamingStrategies.SnakeCaseStrategy());
    }

}
