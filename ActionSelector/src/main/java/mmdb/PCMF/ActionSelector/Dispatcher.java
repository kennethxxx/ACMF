package mmdb.PCMF.ActionSelector;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;

import java.net.URL;
import java.util.HashMap;

import org.apache.log4j.BasicConfigurator;
import org.apache.log4j.Logger;
import org.json.JSONObject;

public class Dispatcher {
	
	private DBC dbc;
	private ActionTask task;
	private Logger logger;
	
	public Dispatcher( ActionTask _task ) {
		
		BasicConfigurator.configure();
		logger = Logger.getLogger(Dispatcher.class);
		this.task = _task;
		this.dbc = new DBC();
		
	}
	
	private HashMap<String, String> callActionOwner( ActionTask task ) {
	    	
		   	logger.info("Ready to send request to " + task.getPath());
		   	logger.info("Use Method: " + task.getMethod());
		   	HashMap<String, String> result = null;
		   	switch(task.getMethod()) {
		   	
		   		case "POST":
		   			result = sendPost(task);
		   			break;
		   			
		   		case "GET":
		   			result = sendGet(task);
		   			break;
		   	
		   	}
		   	
		
		   	// Request is lazily fired whenever you need to obtain information about response.
		   		return result;
	    	
	    }
	    
	private HashMap<String, String> sendPost(ActionTask task) {

	   	String CRLF = "\r\n"; // Line separator required by multipart/form-data.
	   	HttpURLConnection connection = null;
		HashMap<String, String> result = new HashMap<String, String>();
		
	   	try
	   	{
	   		
	   		connection = (HttpURLConnection) new URL(task.getPath()).openConnection();
	   		
	       	connection.setRequestMethod("POST");
	       	connection.setRequestProperty("charset",  "utf-8");
	       	connection.setRequestProperty("Content-type", task.getContentType());
	       	
	        HashMap<String, String> input_para = task.getInputPara();
	        System.out.println("Para = " + new JSONObject(input_para).toString());
	        String req_para = "";
	        connection.setDoOutput(true);
	        
	    	if( !input_para.isEmpty() ) {		   
	        
		        for( Object key : input_para.keySet() ){
		        	
		        	req_para = req_para + key.toString() + "=" + input_para.get(key) + "&";
		        	
		        }
		        req_para = req_para.substring(0, req_para.length()-1);
		        logger.info("Task " + task.getTaskID() + " will send para " + req_para);
		       	DataOutputStream wr = new DataOutputStream(connection.getOutputStream());
		       	wr.writeBytes(req_para);
		       	wr.flush();
		       	wr.close();
	       
		    }    
		    
		    
		    System.out.println("Response Code :" + connection.getResponseCode());
	       	InputStream is = connection.getInputStream();
	       	BufferedReader rd = new BufferedReader(new InputStreamReader(is));
	       	
	       	StringBuilder response = new StringBuilder();
	       	String line;
	       	
	       	while((line = rd.readLine()) != null){
	       		
	       		response.append(line);
	       		response.append(CRLF);
	       		
	       	}

		   	int responseCode = ((HttpURLConnection) connection).getResponseCode();
		   	
		   	result.put("responseCode", Integer.toString(responseCode));
		   	result.put("response", response.toString());
		   	
		   	String task_result = response.toString();	
		   	
		   	dbc.logExecutingResult(task.getTaskID(), result);
		   	logger.info("Finish the task: " + task.getTaskID());
		   	
	   	} catch(IOException e) {
	   		
	   		logger.info("the task " + task.getTaskID() + " happen Exception: " + e.toString());
	   		e.printStackTrace();
	   		
	   	} catch(Exception e) {
	   		
	   		logger.info("the task " + task.getTaskID() + " happen Exception: " + e.toString());
	   		e.printStackTrace();
	   		
	   	}
	   	
	   	return result;

	}
	
	private HashMap<String, String> sendGet(ActionTask task) {

	   	String CRLF = "\r\n"; // Line separator required by multipart/form-data.
	   	HttpURLConnection connection = null;
		HashMap<String, String> result = new HashMap<String, String>();
		
	   	try
	   	{
	   		
	        HashMap<String, String> input_para = task.getInputPara();
	        String req_para = "";
	        StringBuilder url = new StringBuilder();
	        
		    if( !input_para.isEmpty() ) {		   
		        
		        for( Object key : input_para.keySet() ){
		        	
		        	req_para = req_para + key.toString() + "=" + input_para.get(key) + "&";
		        	
		        }
		        url.append(task.getPath());
		        url.append("?");
		        url.append(req_para);

		    }   	        
	   		
	   		connection = (HttpURLConnection) new URL(url.toString()).openConnection();
	   		connection.setDoOutput(true);
	       	connection.setRequestMethod("GET");
	       	connection.setRequestProperty("charset",  "utf-8");
	       	connection.setRequestProperty("Content-type", task.getContentType());
	       	
		    System.out.println("Response Code :" + connection.getResponseCode());
	       	InputStream is = connection.getInputStream();
	       	BufferedReader rd = new BufferedReader(new InputStreamReader(is));
	       	
	       	StringBuilder response = new StringBuilder();
	       	String line;
	       	
	       	while((line = rd.readLine()) != null){
	       		
	       		response.append(line);
	       		response.append(CRLF);
	       		
	       	}

		   	int responseCode = ((HttpURLConnection) connection).getResponseCode();
		   	
		   	result.put("responseCode", Integer.toString(responseCode));
		   	result.put("response", response.toString());
		   	
		   	dbc.logExecutingResult(task.getTaskID(), result);
		   	logger.info("Finish the task: " + task.getTaskID());
		   	
	   	} catch(IOException e) {
	   		
	   		logger.info("the task " + task.getTaskID() + " happen Exception: " + e.toString());
	   		e.printStackTrace();
	   		
	   	} catch(Exception e) {
	   		
	   		logger.info("the task " + task.getTaskID() + " happen Exception: " + e.toString());
	   		e.printStackTrace();
	   		
	   	}
	   	
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
	    	
	    	task = buildActionPathAndSetWorkerID(task);
	    	try {
				task.setMethod(dbc.getActionMethod(task.getActionID()));
				task.setContentType(dbc.getActionContentType(task.getActionID()));
			} catch (ActionNotFoundException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			return task;
	    	
	    }
	    
	    private ActionTask buildActionPathAndSetWorkerID(ActionTask task) {
	    
	    	String host = null;
	    	String action = null;
	    	String prefix = null;
	    	HashMap<String, String> hostInfo = null;
	    	
	    	try{
	    		
		    	hostInfo = this.dbc.getWorkerHost(task.getActionID());
		    	host = hostInfo.get("host");
		    	action = this.dbc.getActionName(task.getActionID());
		    	prefix = this.dbc.getPrefix(task.getActionID());
		    	task.setWorkerID(hostInfo.get("worker_id"));
		    	
	    	} catch (ActionNotFoundException e ) {
	    		
	    		e.printStackTrace();
	    		
	    	}
	    	task.setPath( "http://" + host + ":8080/"+ prefix + "/" + action);
	    	
	    	return task;
	    	
	    }
	    
	    public void run() {
	    	
	    	while( !canProcessTask(task.action_id) );
	    	try{
	    		
	    		task  = initTask(task);
	    		dbc.updateTaskBusyStatus(task.getTaskID());
	    		dbc.updateWorkerBusyStatus(task.getWorkerID(), 1); // start
		    	callActionOwner(task);
		    	dbc.updateWorkerBusyStatus(task.getWorkerID(), 0); // end
		    	
	    	}catch (Exception e) {
	    		
	    		e.printStackTrace();
	    		
	    	}
	    	
	    }
	
}
