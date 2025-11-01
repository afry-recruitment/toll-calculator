package com.assignment.toll_calculator.utils;

import lombok.AccessLevel;
import lombok.NoArgsConstructor;
import org.apache.commons.io.IOUtils;
import org.springframework.core.io.ClassPathResource;
import tools.jackson.core.type.TypeReference;
import tools.jackson.databind.ObjectMapper;

import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.util.List;

/**
 * This class contains all the file related functions supporting the solution.
 */
@NoArgsConstructor(access = AccessLevel.PRIVATE)
public class FileUtils {

    /**
     * Load a list from file
     *
     * @param path - the location of the file
     * @return - the list of items from the file
     */
    public static List<String> loadAsList(String path) {
        String data;

        try {
            ClassPathResource resource = new ClassPathResource(path);
            data = IOUtils.toString(resource.getInputStream(), StandardCharsets.UTF_8);
        } catch (IOException e) {
            throw new RuntimeException(e);
        }

        return new ObjectMapper().readValue(data, new TypeReference<>() {});
    }

}
