package test;

import static org.junit.jupiter.api.Assertions.assertTrue;

import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;

public class JunitTest {
	@Test
	@DisplayName("It is just a test, to be sure that test is working")
	void demoTestMethod() {
		assertTrue(true);
	}

}
