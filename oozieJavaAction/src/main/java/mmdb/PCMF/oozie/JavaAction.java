package mmdb.PCMF.oozie;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.lang.reflect.Type;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.HashMap;
import java.util.Properties;

import org.json.JSONObject;

import com.google.common.reflect.TypeToken;
import com.google.gson.Gson;

/**
 * Hello world!
 *
 */
public class JavaAction 
{
	
	private Properties props;
	private String dispatcher_host;
	private String action_id;
	private String workflow_id;
	  
    public static void main( String[] args )
    {
    	JavaAction action = new JavaAction();
		HashMap<String, String> input_para = new HashMap<String, String>();
		input_para.put("task_input", args[2]);
		action.action_id = args[1];
		action.workflow_id = args[0];
    	action.loadProperties();
        action.sendPost(input_para);
        
    }
    
	  private void loadProperties() {
		  props = new Properties();
		  try {
			  
			  props.load(new FileInputStream("JavaAction.properties"));
			  
		  }catch (FileNotFoundException e) { 
			  e.printStackTrace();  
		  }catch (IOException e) {
			  e.printStackTrace();
		  }
		  
		  this.dispatcher_host = getProperties("dispatcher_host");
		  
	  }
	  
	  private String getProperties(String key) {
		  return props.getProperty(key);
	  }    
    
	private String sendPost(HashMap<String, String> input_para) {

	   	String CRLF = "\r\n"; // Line separator required by multipart/form-data.
	   	HttpURLConnection connection = null;
		HashMap<String, String> result = new HashMap<String, String>();
		
	   	try
	   	{
	   		
	   		connection = (HttpURLConnection) new URL(this.dispatcher_host + "/" + action_id + "/" + workflow_id).openConnection();
	   		
	       	connection.setRequestMethod("POST");
	       	connection.setRequestProperty("charset",  "utf-8");
	       	connection.setRequestProperty("Content-type", "application/x-www-form-urlencoded");
	       	

	        String req_para = "";
	        connection.setDoOutput(true);
	        
		    if( !input_para.isEmpty() ) {		   
		        
		        for( Object key : input_para.keySet() ){
		        	
		        	req_para = req_para + key.toString() + "=" + input_para.get(key) + "&";
		        	
		        }
		        req_para = req_para.substring(0, req_para.length()-1);
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
		   	return task_result;
		   	
		   	
	   	} catch(IOException e) {
	   		
	   		e.printStackTrace();
	   		
	   	} catch(Exception e) {
	   		
	   		e.printStackTrace();
	   		
	   	}
		return CRLF;
	   	

	}
	
}
