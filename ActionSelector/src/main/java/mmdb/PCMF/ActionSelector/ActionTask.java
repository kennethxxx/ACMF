package mmdb.PCMF.ActionSelector;

import java.util.HashMap;
import org.json.*;

public class ActionTask extends Task {

	protected String action_id = "";
	protected String path = "";
	protected String w_task_id = "";
	protected String method = "";
	protected String contentType = "";
	protected String prefix = "";	
	protected String worker_id = "";
	protected HashMap<String, String> input_para; 
	
	public ActionTask( String action_info ) {
		
		JSONObject info = new JSONObject(action_info);		
		
		this.setTaskID(info.getString("task_id"));	
		this.setActionID(info.getString("action_id"));
		this.setStatus(info.getInt("status"));
		this.setMessage(info.getString("message"));
		
	}
	
	public ActionTask(){
		
	}
	
	public String getActionID() {
		
		return this.action_id;
	}
	
	public void setActionID( String action_id ) {
		
		this.action_id = action_id;
		
	}
	
	public String getWorkerID() {
		
		return this.worker_id;
	}
	
	public void setWorkerID( String worker_id ) {
		
		this.worker_id = worker_id;
		
	}	

	public String getPath() {
		
		return this.path;
	}
	
	public void setPath( String path ) {
		
		this.path = path;
		
	}	
	
	public HashMap getInputPara() {
		
		return this.input_para;
		
	}
	
	public String getMethod() {
		
		return this.method;
	}
	
	public void setMethod( String method ) {
		
		this.method = method;
		
	}
	
	public String getPrefix() {
		
		return this.prefix;
	}
	
	public void setPrefix( String prefix ) {
		
		this.prefix = prefix;
		
	}
	
	public String getContentType() {
		
		return this.contentType;
	}
	
	public void setContentType( String contentType ) {
		
		this.contentType = contentType;
		
	}	
	
	public void setInputPara( HashMap input_para ) {
	
		this.input_para = input_para;
	}
	
	public String getWTaskID() {
		
		return this.w_task_id;
	}
	
	public void setWTaskID( String w_task_id ) {
		
		this.w_task_id = w_task_id;
		
	}		
	
	
}
