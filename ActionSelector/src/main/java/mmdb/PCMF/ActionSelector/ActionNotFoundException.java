package mmdb.PCMF.ActionSelector;

public class ActionNotFoundException extends Exception {
	
	public ActionNotFoundException() {
		
		super();
		
	}
	
	public ActionNotFoundException(String message) {
		
		super(message);
		
	}
	
	public ActionNotFoundException(String message, Throwable cause){
		
		super(message, cause);
		
	}
	
	public ActionNotFoundException(Throwable cause) {
		
		super(cause);
		
	}

}
