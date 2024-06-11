package exception;

public class DailyVehicleReportException extends RuntimeException {

	private static final long serialVersionUID = 1L;


	public DailyVehicleReportException(String message, Throwable cause) {
		super(message, cause);
	}

	public DailyVehicleReportException(String message) {
		super(message);
	}

	public DailyVehicleReportException(Throwable cause) {
		super(cause);
	}
	

}
