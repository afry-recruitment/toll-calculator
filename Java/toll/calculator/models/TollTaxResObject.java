package toll.calculator.models;

public class TollTaxResObject {

    public TollTaxResObject() {
		super();
		// TODO Auto-generated constructor stub
	}
	@Override
	public String toString() {
		return "TollTaxResObject [errorCode=" + errorCode + ", errorMsg=" + errorMsg + ", totalTaxFee=" + totalTaxFee
				+ ", message=" + message + "]";
	}
	public TollTaxResObject(String errorCode, String errorMsg, int totalTaxFee, String message) {
		super();
		this.errorCode = errorCode;
		this.errorMsg = errorMsg;
		this.totalTaxFee = totalTaxFee;
		this.message = message;
	}
	private String errorCode;
    private String errorMsg;    
    private int totalTaxFee;
    private String message;
	public String getErrorCode() {
		return errorCode;
	}
	public void setErrorCode(String errorCode) {
		this.errorCode = errorCode;
	}
	public String getErrorMsg() {
		return errorMsg;
	}
	public void setErrorMsg(String errorMsg) {
		this.errorMsg = errorMsg;
	}
	public int getTotalTaxFee() {
		return totalTaxFee;
	}
	public void setTotalTaxFee(int totalTaxFee) {
		this.totalTaxFee = totalTaxFee;
	}
	public String getMessage() {
		return message;
	}
	public void setMessage(String message) {
		this.message = message;
	}
    
    
    
}
