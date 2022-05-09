package com.afry.tollcalculator;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

import org.junit.jupiter.api.Test;
import org.junit.runner.RunWith;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.http.MediaType;
import org.springframework.test.context.TestPropertySource;
import org.springframework.test.context.junit4.SpringRunner;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.ResultActions;
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders;

import com.afry.tollcalculator.model.CalculateFee;
import com.fasterxml.jackson.databind.ObjectMapper;

@RunWith(SpringRunner.class)
@SpringBootTest(classes = TollCalculatorJavaApplication.class)
@AutoConfigureMockMvc
@TestPropertySource(locations = "classpath:application-test.properties")
class TollCalculatorJavaApplicationTests {

	@Autowired
	private MockMvc mvc;

	@Test
	public void calculateTotalFee() throws Exception {
		ResultActions result = mvc.perform(MockMvcRequestBuilders.post("/api/v1/tollfee/calculate")
				.content(asJsonString(
						new CalculateFee("Car", "2022-01-02T08:15:56", "2022-01-02T07:15:56", "2022-01-02T18:15:56")))
				.contentType(MediaType.APPLICATION_JSON).accept(MediaType.APPLICATION_JSON));

		result.andExpect(status().isOk());
		String tollFee = result.andReturn().getResponse().getContentAsString();
		assertEquals("39", tollFee);
	}

	@Test
	public void calculateTotalFeeWithMultipleFee() throws Exception {
		ResultActions result = mvc.perform(MockMvcRequestBuilders.post("/api/v1/tollfee/calculate")
				.content(asJsonString(new CalculateFee("Car", "2022-01-02T15:15:56")))
				.contentType(MediaType.APPLICATION_JSON).accept(MediaType.APPLICATION_JSON));

		result.andExpect(status().isOk());
		String tollFee = result.andReturn().getResponse().getContentAsString();
		assertEquals("18", tollFee);
	}

	@Test
	public void calculateFeeOverSixty() throws Exception {
		ResultActions result = mvc.perform(MockMvcRequestBuilders.post("/api/v1/tollfee/calculate")
				.content(asJsonString(new CalculateFee("Car", "2022-01-02T15:15:56", "2022-01-02T08:15:56",
						"2022-01-02T07:15:56", "2022-01-02T18:15:56", "2022-01-02T13:15:56")))
				.contentType(MediaType.APPLICATION_JSON).accept(MediaType.APPLICATION_JSON));

		result.andExpect(status().isOk());
		String tollFee = result.andReturn().getResponse().getContentAsString();
		assertEquals("60", tollFee);
	}

	public static String asJsonString(final Object obj) {
		try {
			return new ObjectMapper().writeValueAsString(obj);
		} catch (Exception e) {
			throw new RuntimeException(e);
		}
	}

}
