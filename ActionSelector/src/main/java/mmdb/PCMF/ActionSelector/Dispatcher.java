package mmdb.PCMF.ActionSelector;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.HashMap;

public class Dispatcher {
	
	private DBC dbc;
	private ActionTask task;
	
	public Dispatcher( ActionTask _task ) {
		
		this.task = _task;
		this.dbc = new DBC();
		
	}
	
	private HashMap callActionOwner( ActionTask task ) {
	    	
		   	System.out.println(task.getPath());
		   	String charset = "UTF-8";
		   	String CRLF = "\r\n"; // Line separator required by multipart/form-data.
		   	HttpURLConnection connection;
			HashMap<String, String> result = new HashMap<String, String>();
		
		   	try
		   	{
		   		
		   		connection = (HttpURLConnection) new URL(task.getPath()).openConnection();
		   		connection.setDoOutput(true);
		       	connection.setRequestMethod(task.getMethod());
		       	connection.setRequestProperty("Content-Type", "text/plain");
		       	connection.setDoOutput(true);
		       	
		        HashMap input_para = task.getInputPara();
		        
			    if( !input_para.isEmpty() ) {
			    	
			        String req_para = "";
			        
			        for( Object key : input_para.keySet() ){
			        	
			        	req_para = req_para + key.toString() + "=" + input_para.get(key) + "&";
			        	
			        }
			       	
			       	connection.connect();
			       	System.out.println(req_para);
			       	DataOutputStream wr = new DataOutputStream(connection.getOutputStream());
			       	wr.writeBytes(req_para);
			       	wr.flush();
			       	wr.close();
		       	
		        }
		       	
		       	InputStream is = connection.getInputStream();
		       	BufferedReader rd = new BufferedReader(new InputStreamReader(is));
		       	
		       	StringBuilder response = new StringBuilder();
		       	String line;
		       	
		       	while((line = rd.readLine()) != null){
		       		
		       		response.append(line);
		       		response.append(CRLF);
		       		
		       	}
		       	
		       	response.toString();
			   	int responseCode = ((HttpURLConnection) connection).getResponseCode();
			   	
			   	result.put("responseCode", Integer.toString(responseCode));
			   	result.put("response", response.toString());
			   	
			   	System.out.println(result.toString());
			   	
	  	    
		   	} catch(Exception e) {
		   		
		   		e.printStackTrace();
		   		
		   	}
		
		   	// Request is lazily fired whenever you need to obtain information about response.
		   		return result;
	    	
	    }
	    
	    private boolean canProcessTask( String action_id ) {
	    	
	    	boolean check = false;
	    	
	    	try {
	    	
	    		check = this.dbc.checkIdleWorker(action_id); // find the worker whose state equals to 0.
	    		
	    	}catch(ActionNotFoundException e) {
	    		
	    		e.printStackTrace();
	    		
	    	}
	    	
			return check;

	    }
	    
	    private ActionTask initTask(ActionTask task) {
	    	
	    	task = buildActionPath(task);
	    	try {
				task.setMethod(dbc.getActionMethod(task.action_id));
			} catch (ActionNotFoundException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			return task;
	    	
	    }
	    
	    private ActionTask buildActionPath(ActionTask task) {
	    
	    	String host = null;
	    	String action = null;
	    	
	    	try{
	    		
		    	host = this.dbc.getWorkerHost(task.getActionID());
		    	action = this.dbc.getActionName(task.getActionID());
		    	
	    	} catch (ActionNotFoundException e ) {
	    		
	    		e.printStackTrace();
	    		
	    	}
	    	task.setPath( "http://" + host + ":7782/MachineToolDataCollector/RESTful-Interface/file/" + action);
	    	
	    	return task;
	    	
	    }
	    
	    public void run() {
	    	
	    	while( !canProcessTask(task.action_id) );
	    	try{

	    		task  = initTask(task);
		    	callActionOwner(task);
		    	
	    	}catch (Exception e) {
	    		
	    		e.printStackTrace();
	    		
	    	}
	    	
	    }
	
}
