package unittests;

import static org.junit.Assert.*;
import java.util.Date;
import org.junit.Test;

import datamodel.SampleData;

public class SampleDataTest {
    
    @Test
    public void testMoreThanOneParkingSessions() {
        
        Date[] dates = new SampleData().getParkingSessions();
        assertTrue(dates.length > 0);
    }
}
