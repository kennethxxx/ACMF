package mmdb.PCMF.ActionSelector;

import javax.ws.rs.FormParam;
import javax.ws.rs.GET;
import javax.ws.rs.POST;
import javax.ws.rs.Path;
import javax.ws.rs.Produces;
import javax.ws.rs.QueryParam;
import javax.ws.rs.PathParam;
import org.apache.log4j.Logger;
import org.apache.log4j.PropertyConfigurator;

/** Example resource class hosted at the URI path "/myresource"
 */
@Path("/parser/")
public class Parser {
    
    /** 
     * @param  action_id : the action that will be called.
     * @return boolean
     */
	
    @POST
    @Path("/submit/{action_id}/{w_task_id}/")
    @Produces("text/plain")
    public String submit( @PathParam("action_id") String action_id, 
    		@PathParam("w_task_id") String w_task_id,
    		@QueryParam("task_input") String task_input ) {

    	ActionTask task;

		PropertyConfigurator.configure("../../log4j.properties");
		Logger logger = Logger.getLogger(Parser.class);
    	
    	try{
    		
    		logger.info("Get a task. Action_id = " + action_id );
    		task = this.logToLogger(action_id, w_task_id, task_input );
    		logger.info("log a task. Task_id = " + task.getTaskID());
        	Dispatcher dispatcher = new Dispatcher(task);
        	dispatcher.run();
        	logger.info("Run a task. Task_id = " + task.getTaskID());
        	
    	}catch(Exception e){
        	
    		// not found action exception
    	
    	}
		return action_id;
        
    }
    
    private ActionTask logToLogger(String action_id, String w_task_id, String task_input ) throws ActionNotFoundException {
    	
    	DBC logger = new DBC();
    	ActionTask task = null;
    	
    	if( logger.actionExistedCheck(action_id) ){
    		
    		task = logger.newActionTask(action_id, w_task_id, task_input);
    		return task;
    		
    	}else {
    	
    		throw new ActionNotFoundException("This action isn't found in Action Table.");
    		
    	}
    	
    }
    
 
}
