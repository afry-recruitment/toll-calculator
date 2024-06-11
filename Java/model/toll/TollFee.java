package model.toll;

public class TollFee {

	private TollFeeType type;
	private int amountToPay;
	
	public TollFee(TollFeeType type, int amountToPay) {
		this.type = type;
		this.amountToPay = amountToPay;
	}
	public TollFeeType getType() {
		return type;
	}
	public void setType(TollFeeType type) {
		this.type = type;
	}
	public int getAmountToPay() {
		return amountToPay;
	}
	public void setAmountToPay(int amountToPay) {
		this.amountToPay = amountToPay;
	}

}
