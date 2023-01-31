import com.toll.calculator.DateHandler;
import org.junit.jupiter.api.*;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.time.LocalDate;
import java.util.Date;

import static org.junit.jupiter.api.Assertions.*;

@TestInstance(TestInstance.Lifecycle.PER_CLASS)
public class TestDateHandler {
    private SimpleDateFormat sdf;
    private Date date;

    @BeforeAll
    void setUp() throws ParseException {
        sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss.SSS");
        date = sdf.parse("2022-01-02 17:00:00.000");
    }

    @AfterAll
    void tearDown() {
        sdf = null;
        date = null;
    }

    @Test
    @DisplayName("Be able to convert date to its corresponding localdate type")
    void testConversionOfDateToCorrespondingLocalDate() {
        LocalDate localDate = DateHandler.getLocalDateFromDate(date);
        assertEquals(localDate.toString(), "2022-01-02");
    }

    @Test
    @DisplayName("Ensure conversion of date to localdate is nothing else than expected")
    void testConversionOfDateIsNotAnythingElseThanExpected() {
        LocalDate localDate = DateHandler.getLocalDateFromDate(date);
        assertNotEquals(localDate.toString(), "1995-01-03");
    }
}
