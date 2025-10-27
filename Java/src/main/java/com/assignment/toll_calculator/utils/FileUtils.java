package com.assignment.toll_calculator.utils;

import lombok.AccessLevel;
import lombok.NoArgsConstructor;
import org.apache.commons.io.IOUtils;
import tools.jackson.core.type.TypeReference;
import tools.jackson.databind.ObjectMapper;

import java.io.FileInputStream;
import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.util.List;

@NoArgsConstructor(access = AccessLevel.PRIVATE)
public class FileUtils {

    public static final String HOLIDAYS_FILE_PATH = "src/main/resources/holidays.json";

    public static List<String> loadAsList(String path) {
        String data;

        try {
            FileInputStream fis = new FileInputStream(path);
            data = IOUtils.toString(fis, StandardCharsets.UTF_8);
        } catch (IOException e) {
            throw new RuntimeException(e);
        }

        return new ObjectMapper().readValue(data, new TypeReference<>() {});
    }

}
