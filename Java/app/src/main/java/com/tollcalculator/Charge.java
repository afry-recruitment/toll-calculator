package com.tollcalculator;

import java.util.Date;

public class Charge {
	private int _cost;
	private Date _timestamp;

	Charge(Date timestamp, int cost){
		this._timestamp = timestamp;
		this._cost = cost;
	}

	public Date getTimestamp(){
		return _timestamp;
	}

	public int getCost(){
		return _cost;
	}
}
