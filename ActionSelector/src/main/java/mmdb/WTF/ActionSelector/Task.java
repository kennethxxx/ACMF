package mmdb.WTF.ActionSelector;

import org.json.*;
import java.util.HashMap;

/*
 * Task Class is used to describe a Task Request.
 * 
 */

public class Task {

	protected String task_id = "";
	protected int status = 0;
	protected String message = "";

	
	public Task() {
		
	}
	
	public String getTaskID() {
		
		return this.task_id;
		
	}
	
	public void setTaskID( String task_id ) {
		
		this.task_id = task_id;
		
	}
	
	public int getStatus() {
		
		return this.status;
		
	}
	
	public void setStatus( int status ) {
		
		this.status = status;
		
	}
	
	public String getMessage() {
		
		return this.message;
		
	}
	
	public void setMessage( String message ) {
	
		this.message = message;
	}

}
