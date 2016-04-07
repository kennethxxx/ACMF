
package mmdb.WTF.ActionSelector;

import javax.ws.rs.GET;
import javax.ws.rs.Path;
import javax.ws.rs.Produces;
import javax.ws.rs.PathParam;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.sql.*;
import java.util.HashMap;

/** Example resource class hosted at the URI path "/myresource"
 */
@Path("/scheduler/")
public class Scheduler {
    
    /** 
     * @param  action_id : the action that will be called.
     * @return boolean
     */
	
    @GET 
    @Path("/submit/{action_id}")
    @Produces("text/plain")
    public String submit( @PathParam("action_id") String action_id, @PathParam("w_task_id") String w_task_id ) {

    	ActionTask task;
    	
    	try{
    		
    		task = this.logToLogger(action_id, w_task_id);
    			
    	}catch(Exception e){
    	
    		// not found action exception
    	
    	}
    	
        if ( this.canProcessTask( action_id ) ) {
        	
        	HashMap result;
        	task = this.buildActionPath( task );
        	result = this.callActionOwner( task );
        	
        	// return result
        	
        }else {
        	
        	return "false";
        	
        }
		return action_id;
        
    }
    
    private ActionTask logToLogger(String action_id, String w_task_id ) throws ActionNotFoundException {
    	
    	DBC logger = new DBC();
    	
    	if( DBC.actionExistedCheck(action_id) ){
    		
    		ActionTask task = DBC.newActionTask(action_id, w_task_id);
    		return task;
    		
    	}else {
    	
    		throw new ActionNotFoundException("This action isn't found in Action Table.");
    		
    	}
    	
    }
    
    public HashMap callActionOwner( ActionTask task ) {
    	
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
	       	connection.setRequestProperty("Content-Type", "application/octet-stream");
	       	connection.setDoOutput(true);
	       	
	        HashMap input_para = task.getInputPara();
	        String req_para = "";
	        
	        for( Object key : input_para.keySet() ){
	        	
	        	req_para = req_para + key.toString() + "=" + input_para.get(key) + "&";
	        	
	        }
	       	
	       	connection.connect();
	       	
	       	DataOutputStream wr = new DataOutputStream(connection.getOutputStream());
	       	wr.writeBytes(req_para);
	       	wr.flush();
	       	wr.close();
	       	
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
		   	
  	    
	   	} catch(Exception e) {
	   		
	   		e.printStackTrace();
	   		
	   	}
	
	   	// Request is lazily fired whenever you need to obtain information about response.
	   		return result;
    	
    }
    
    private boolean canProcessTask( String action_id ) {
    	
    	DBC dbc = new DBC(); // connect with Worker Table
    	return dbc.checkIdleWorker(action_id); // find the worker whose state equals to 0.

    }
    
    private ActionTask buildActionPath(ActionTask task) {
    	
    	DBC dbc = new DBC();
    	String host = dbc.getWorkerHost(task.getActionID());
    	String action = dbc.getActionName(task.getActionID());
    	task.setPath(host+action);
    	
    	return task;
    	
    }
}
