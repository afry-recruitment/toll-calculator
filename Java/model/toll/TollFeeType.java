package model.toll;

import java.time.LocalTime;

/**
 * Constants for fees for durations during the day. 
 * 
 * Time is set FROM (17:00) time to BEFORE (18:00) time. (not FROM (17:00) to UNTIL(17:59)).
 * Unambiguous conditions in time interval removed from old code. 
 */
public enum TollFeeType {
	
	TOLL_FREE_HOURS_FEE_TYPE_PM(0, "18:30:00" ,"00:00:00" ),
	TOLL_FREE_HOURS_FEE_TYPE_AM(0, "00:00:00" ,"06:00:00" ),
	FIRST_FEE_TYPE(8, "06:00:00" ,"06:30:00" ),
	SECOND_FEE_TYPE(13, "06:30:00" ,"07:00:00" ),
	THIRD_FEE_TYPE(18, "07:00:00" ,"08:00:00" ),
	FOURTH_FEE_TYPE(13, "08:00:00" ,"08:30:00" ),
	SIXTH_FEE_TYPE(8, "08:30:00" ,"15:00:00" ),
	SEVENTH_FEE_TYPE(13, "15:00:00" ,"15:30:00" ),
	EIGTH_FEE_TYPE(18, "15:30:00" ,"17:00:00" ),
	NINTH_FEE_TYPE(13, "17:00:00" ,"18:00:00" ),
	TENTH_FEE_TYPE(8, "18:00:00" ,"18:30:00" ),
	;
	
	
	private int fee;
	
	private LocalTime startLocalTime;
	
	private LocalTime endLocalTime;
	

	/**
	 * 
	 * 
	 * @param Fee for this intervals
	 * @param The start time to parse such as "hh:mm:ss", not null
	 * @param The end time to parse such as "hh:mm:ss", not null
	 */
	TollFeeType(int fee, CharSequence start,  CharSequence end ){
		
		this.startLocalTime = LocalTime.parse(start);
		this.endLocalTime = LocalTime.parse(end);
		this.fee = fee;
	}

	public int getFee() {
		return fee;
	}

	public LocalTime getStartLocalTime() {
		return startLocalTime;
	}
	public LocalTime getEndLocalTime() {
		return endLocalTime;
	}

	/**
	 * Get the fee type for a LocalTime representing a time during a day.
	 * @param eventTime
	 * @return the TollFeeType for this day time. 
	 */
	public static TollFeeType getFeeTypeFromDate(LocalTime eventTime) {
		
		TollFeeType[] values = TollFeeType.values();
		
		for (int i = 0; i < values.length; i++) {	
			if(  ! eventTime.isBefore( values[i].getStartLocalTime()) && eventTime.isBefore( values[i].getEndLocalTime())) {
				return values[i];
			}
		}	
		return null;
	}
}
