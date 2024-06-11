package model.toll.event;

/**
 * Enum for TollEvent types
 */
public enum TollEvenType {
	
	   CHARGEABLE_EVENT_TYPE("The vehicle for this event is not toll free and the event occurred during a chargeable hour.", false ),
	   DEDUCTED_ON_HOURLY_INTERVAL_EVENT_TYPE("The vehicle for this event is not toll free and the event occurred during chargeable hours but the amount is deducted due to another event whithin with a higher fee in the same interval.", false),
	   PARTIAL_DEDUCTED_ON_MAX_DAILY_EVENT_TYPE("The vehicle for this event is not toll free and the event occurred during chargeable hours but maximum daily amount was reached and the fee was partialy deducted", false),
	   FULLY_DEDUCTED_ON_MAX_DAILY_EVENT_TYPE("The vehicle for this event is not toll free and the event occurred during chargeable hours but maximum daily fee is already reached", true),
	   TOLL_FREE_DAY_TIME_EVENT_TYPE("This event occurred during none chargeable hours.", true),
	   TOLL_FREE_VEHICLE_EVENT_TYPE("The vehicle in this event is toll free.", true),
	   TOLL_FREE_DAY_EVENT_TYPE("This event occurred during a toll free day", true);

	
	   String description;
	   
	   boolean tollFree;
	   	   
	   TollEvenType(String description, boolean tollFree) {
		   this.description = description;
		   this.tollFree = tollFree;
	   }

		public String getDescription() {
			return description;
		}
	
		public boolean isTollFree() {
			return tollFree;
		}
	
	   
	   
}
