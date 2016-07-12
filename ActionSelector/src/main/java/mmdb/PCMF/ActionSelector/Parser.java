package mmdb.PCMF.ActionSelector;

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
@Path("/parser/")
public class Parser {
    
    /** 
     * @param  action_id : the action that will be called.
     * @return boolean
     */
	
    @GET 
    @Path("/submit/{action_id}")
    @Produces("text/plain")
    public String submit( @PathParam("action_id") String action_id, 
    		@PathParam("w_task_id") String w_task_id,
    		@PathParam("task_input") String task_input ) {

    	ActionTask task;
    	
    	try{
    		
    		task = this.logToLogger(action_id, w_task_id, task_input );
        
    	}catch(Exception e){
        	
    		// not found action exception
    	
    	}
		return action_id;
        
    }
    
    private ActionTask logToLogger(String action_id, String w_task_id, String task_input ) throws ActionNotFoundException {
    	
    	DBC logger = new DBC();
    	
    	if( logger.actionExistedCheck(action_id) ){
    		
    	//	ActionTask task = logger.newActionTask(action_id, w_task_id, task_input);
    //		return task;
    		
    	}else {
    	
    		throw new ActionNotFoundException("This action isn't found in Action Table.");
    		
    	}
		return null;
    	
    }
    
 
}
