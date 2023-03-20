package toll.calculator.utils;

import java.io.IOException;
import java.io.InputStream;
import java.util.Arrays;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;
import java.util.Properties;
import java.util.Set;

public class RulesLoader {

	//private String VEHICLE = "vehicles.properties";
	//private String TOLL_TAX = "congetsionTax.properties";
	
	private String VEHICLE = "vehicles.properties";
	private String TOLL_TAX = "tollTax.properties";

	public Set<String> tollFreeVehicles;
	public Set<String> tollFeeDates;
	public Set<String> tollFeeMonth;
	public Set<String> tollFeeDayOfWeek;
	public Set<String> validYears;

	public Map<String, String> tollTax = new HashMap<>();

	private Properties loadProps(String city) {
		Properties properties = new Properties();
		InputStream inputStream = this.getClass().getClassLoader().getResourceAsStream(city + "_" + VEHICLE);
		if (inputStream == null)
			inputStream = this.getClass().getClassLoader().getResourceAsStream("default_" + VEHICLE);
		try {
			properties.load(inputStream);
		} catch (IOException e) {
			e.printStackTrace();
		}
		return properties;
	}

	private Properties loadTaxProps(String city) {
		Properties properties = new Properties();
		InputStream inputStream = this.getClass().getClassLoader().getResourceAsStream(city + "_" + TOLL_TAX);
		if (inputStream == null)
			inputStream = this.getClass().getClassLoader().getResourceAsStream("default_" + TOLL_TAX);

		try {
			properties.load(inputStream);
		} catch (IOException e) {
			e.printStackTrace();
		}
		return properties;
	}

	public Set<String> tollFreeVehicles(String city) {
		String value = loadProps(city).getProperty("tollFreeVehicles");

		if (value != null) {
			String[] split = value.split(",");

			tollFreeVehicles = new HashSet<>(Arrays.asList(split));
		} else {
			tollFreeVehicles = new HashSet<>();
		}
		return tollFreeVehicles;
	}
	public Set<String> validYears(String city) {
		String value = loadProps(city).getProperty("validYears");

		if (value != null) {
			String[] split = value.split(",");

			validYears = new HashSet<>(Arrays.asList(split));
		} else {
			validYears = new HashSet<>();
		}
		return validYears;
	}
	public Set<String> tollFeeMonth(String city) {
		String value = loadProps(city).getProperty("tollFeeMonth");

		if (value != null) {
			String[] split = value.split(",");

			tollFeeMonth = new HashSet<>(Arrays.asList(split));
		} else {
			tollFeeMonth = new HashSet<>();
		}
		return tollFeeMonth;
	}

	public Set<String> tollFeeDayOfWeek(String city) {
		String value = loadProps(city).getProperty("tollFeeDayOfWeek");
		if (value != null) {
			String[] split = value.split(",");

			tollFeeDayOfWeek = new HashSet<>(Arrays.asList(split));
		} else {
			tollFeeDayOfWeek = new HashSet<>();
		}
		return tollFeeDayOfWeek;
	}

	public Set<String> tollFeeDates(String city) {
		String value = loadProps(city).getProperty("tollFeeDates");
		if (value != null) {
			String[] split = value.split(",");

			tollFeeDates = new HashSet<>(Arrays.asList(split));
		} else {
			tollFeeDates = new HashSet<>();
		}
		return tollFeeDates;
	}

	public int getSingleChargeRuleMins(String city) {

		int minues = 0;
		String value = loadProps(city).getProperty("singleChargeRuleMins");
		if (value != null) 
			minues = Integer.parseInt(value);

		return minues;
	}

	public int getMaxAmtPerDay(String city) {
		int amount = 0;
		String value = loadProps(city).getProperty("maxAmtPerDay");
		if (value != null) 
			amount = Integer.parseInt(value);

		return amount;
		
	}

	public Map<String, String> getCongetstionTaxRules(String city) {

		Set<Object> objects = loadTaxProps(city).keySet();

		for (Object object : objects) {
			String str = object.toString().replace(".", "");
			tollTax.put(str, loadTaxProps(city).getProperty(object.toString()));
		}
		return tollTax;
	}

}
