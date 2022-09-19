package com.afry.test.service;

import java.text.SimpleDateFormat;
import java.util.*;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.Mockito;
import org.mockito.MockitoAnnotations;
import org.mockito.junit.jupiter.MockitoExtension;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.test.util.ReflectionTestUtils;

import com.afry.test.dao.RushHourRepository;
import com.afry.test.model.*;

/**
 * RushHourServiceTest to cover and test RushHourService methods
 */
@ExtendWith(MockitoExtension.class)
public class RushHourServiceTest {
    RushHourService rushHourService;
    @MockBean
    RushHourRepository rushHourRepository;
    SimpleDateFormat formatter;
    List<RushHour> rushHoursList;

    @BeforeEach
    public void init() {
        MockitoAnnotations.openMocks(this);
        rushHourService = new RushHourService();

        rushHoursList = new ArrayList();
        RushHour rushHour1 = new RushHour();
        rushHour1.setFromHour(12);
        rushHour1.setToHour(13);
        RushHour rushHour2 = new RushHour();
        rushHour2.setFromHour(16);
        rushHour2.setToHour(18);

        rushHoursList.add(rushHour1);
        rushHoursList.add(rushHour2);

        rushHourRepository = Mockito.mock(RushHourRepository.class);
        ReflectionTestUtils.setField(rushHourService, "rushHourRepository", rushHourRepository);
    }

    @Test
    @DisplayName("Test Get All Rush Hours")
    void testGetRushHours() throws Exception {
        Mockito.when(rushHourService.getRushHours()).thenReturn(rushHoursList);
        List<RushHour> rushHours = rushHourService.getRushHours();
        Assertions.assertNotNull(rushHours);
        Assertions.assertEquals(2, rushHours.size());
    }
}
