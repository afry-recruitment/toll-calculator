import java.text.ParseException;

import Exception.CarCreatorException;
import Exception.DateException;

// Class only for running the test
public class Tester {
    public static void main(String[] args) throws ParseException, CarCreatorException, DateException {

        ExampleTest test = new ExampleTest();
        test.runTest();
    }
}