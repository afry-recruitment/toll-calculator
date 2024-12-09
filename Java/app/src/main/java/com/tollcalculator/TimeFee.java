package com.tollcalculator;

import java.time.LocalTime;

public class TimeFee {
	private LocalTime _start;
	private LocalTime _end;
	private int _cost;

	TimeFee(String start, String end, int cost){
		this._start = LocalTime.parse(start);
		this._end = LocalTime.parse(end);
		this._cost = cost;
	}

	public LocalTime getStart(){
		return this._start;
	}

	public LocalTime getEnd(){
		return this._end;
	}

	public int getCost(){
		return this._cost;
	}

	public boolean isMatch(LocalTime time){
		return (time.equals(this._start) || time.isAfter(this._start)) && time.isBefore(this._end);
	}
}
