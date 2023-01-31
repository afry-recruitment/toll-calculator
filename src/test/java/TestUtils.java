import com.toll.calculator.Utils;
import org.junit.jupiter.api.*;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

@TestInstance(TestInstance.Lifecycle.PER_CLASS)
public class TestUtils {
    private SimpleDateFormat sdf;

    @BeforeAll
    void setUp() {
        sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss.SSS");
    }

    @AfterAll
    void tearDown() {
        sdf = null;
    }

    @Test
    @DisplayName("Ensure that all dates in list are for a specific day")
    void testAllDatesInListAreSpecificDate() throws ParseException {
        List<Date> dateList = new ArrayList<>();
        dateList.add(sdf.parse("2022-01-02 17:00:00.000"));
        dateList.add(sdf.parse("2022-01-02 18:00:00.000"));
        dateList.add(sdf.parse("2022-01-02 19:00:00.000"));
        Assertions.assertTrue(Utils.isDatesWithinSameDay(dateList));
    }

    @Test
    @DisplayName("Ensure that all dates in list are not for a specific day")
    void testAllDatesInListAreNotSpecificDate() throws ParseException {
        List<Date> dateList = new ArrayList<>();
        dateList.add(sdf.parse("2022-04-02 17:00:00.000"));
        dateList.add(sdf.parse("2022-02-09 18:00:00.000"));
        dateList.add(sdf.parse("2022-01-03 19:00:00.000"));
        Assertions.assertFalse(Utils.isDatesWithinSameDay(dateList));
    }
}
