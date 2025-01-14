package com.toll.calculator.exception;

import java.io.Serial;

public class TollException extends Exception{

	@Serial
	private static final long serialVersionUID = 1L;

	public TollException(String message) {
		super(message);
	}

}
