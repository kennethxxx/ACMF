package mmdb.PCMF.EncapsulationManager;

import java.io.*;
import java.lang.reflect.Type;

import com.google.common.reflect.TypeToken;
import com.google.gson.Gson;

import javax.ws.rs.*;
import javax.ws.rs.core.*;

import java.net.HttpURLConnection;
import java.net.URL;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.Properties;

import org.apache.oozie.client.OozieClient;
import org.apache.oozie.client.OozieClientException;
import org.apache.oozie.client.WorkflowJob;

import org.apache.commons.io.IOUtils;
import org.glassfish.jersey.media.multipart.FormDataContentDisposition;
import org.glassfish.jersey.media.multipart.FormDataParam;
import org.apache.commons.io.FileUtils;

@Path("/operator")
public class ActionResource {

	private String _baseDir = "/tmp/file-accept/";
	DBC dbc = new DBC();

    @GET 
    @Path("getIt")
    public Response getIt() {
    	
    	return Response.ok("aaa")
    			.header("Access-Control-Allow-Origin", "*")
    			.header("Access-Control-Allow-Methods", "GET, POST, DELETE, PUT")
    			.build();
    }
    
    @GET 
    @Path("retrieve/{fileName}")
    public Response retrieve(@PathParam("fileName") String fileName) throws IOException { 
        File mergedPath = new File(this._baseDir, fileName);
        return Response.ok(mergedPath)
    			.header("Access-Control-Allow-Origin", "*")
    			.header("Access-Control-Allow-Methods", "GET, POST, DELETE, PUT")
    			.build();
    }
    
    @DELETE
    @Path("delete/{fileName}")
    public Response delete(@PathParam("fileName") String fileName) throws IOException {
        File mergedPath = new File(this._baseDir, fileName);
        FileUtils.deleteQuietly(mergedPath);
        return Response.ok("OK")
    			.header("Access-Control-Allow-Origin", "*")
    			.header("Access-Control-Allow-Methods", "GET, POST, DELETE, PUT")
        		.build();
    }


    @GET
    @Path("getAction")
    public Response getAction() throws IOException {
    	
    	return Response.ok(dbc.getAction())
    			.header("Access-Control-Allow-Origin", "*")
    			.header("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS, HEAD")
    			.header("Access-Control-Allow-Credentials", "true")
    			.header("Access-Control-Allow-Headers", "origin, content-type, accept, authorization")
    			.build();
    	
    }
    
    @POST
    @Path("insert")
    @Consumes(MediaType.MULTIPART_FORM_DATA) 
    public Response insert(@FormDataParam("action_info") String action_info,
    		@FormDataParam("file") InputStream is,
    		@FormDataParam("file") FormDataContentDisposition fileDetail ) throws IOException{
        
        OutputStream os = null;
        try {
            System.out.println("get the request: " + action_info);
        	if(checkWAR(fileDetail.getFileName())){
        		
	        	fileDetail.getType();
	        	String fileName = fileDetail.getFileName();
	            File mergedPath = new File(this._baseDir, fileName);
	            os = new FileOutputStream(mergedPath);
	            IOUtils.copy(is, os);
	            
				Type mapType = new TypeToken<HashMap<String, String>>(){}.getType();  
				HashMap<String, String> input_para = new Gson().fromJson(action_info, mapType);
	            dbc.newAction(input_para);
	            ArrayList<String> hosts = dbc.getWorkerHost(input_para.get("action_owner"));
	            for(String host : hosts ){
	            	
	            	copyFileToWorker("http://" + host + ":8181/operator/deploy/"+fileDetail.getFileName(), mergedPath);
	            }
	            
	            
        	}else {
        		
        		return Response.serverError().entity("file type is wrong")
            			.header("Access-Control-Allow-Origin", "*")
            			.header("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS, HEAD")
            			.header("Access-Control-Allow-Credentials", "true")
            			.header("Access-Control-Allow-Headers", "origin, content-type, accept, authorization")            			
        				.build();
        		
        	}
        
        } catch (IOException ex) {
            
            ex.printStackTrace();
    		return Response.serverError()
        			.header("Access-Control-Allow-Origin", "*")
        			.header("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS, HEAD")
        			.header("Access-Control-Allow-Credentials", "true")
        			.header("Access-Control-Allow-Headers", "origin, content-type, accept, authorization")            			
    				.build();
            
        } finally {
            
            if (is != null){
                try {
                    is.close();
                } catch(IOException e) {
                    e.printStackTrace();
                }
            }
            
            if (os != null) {
                try {
                    os.close();
                } catch(IOException e) {
                    e.printStackTrace();
                }
            }
            
        }
        
        return Response.ok("OK")
    			.header("Access-Control-Allow-Origin", "*")
    			.header("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS, HEAD")
    			.header("Access-Control-Allow-Credentials", "true")
    			.header("Access-Control-Allow-Headers", "origin, content-type, accept, authorization")    			
        		.build();
        
    }

    @GET
    @Path("getProcess")
    public Response getProcess() throws IOException {
    	
    	return Response.ok(dbc.getProcessLog())
    			.header("Access-Control-Allow-Origin", "*")
    			.header("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS, HEAD")
    			.header("Access-Control-Allow-Credentials", "true")
    			.header("Access-Control-Allow-Headers", "origin, content-type, accept, authorization")
    			.build();
    	
    }    
    
    @POST
    @Path("processSubmit")
    @Consumes(MediaType.MULTIPART_FORM_DATA) 
    public Response processSubmit(
    		@FormDataParam("file") InputStream is,
    		@FormDataParam("file") FormDataContentDisposition fileDetail ) throws IOException, OozieClientException, InterruptedException{
    	
    		Date now = new Date();
    		SimpleDateFormat ft = new SimpleDateFormat("yyyyMMddhhmmss");
    		String wfNum = ft.format(now);
    		
        	String fileName = fileDetail.getFileName();
            File mergedPath = new File(this._baseDir, fileName);
            FileOutputStream os = new FileOutputStream(mergedPath);
            IOUtils.copy(is, os);
            
            String process_id = dbc.newProcess();
            
            Process p;
            p = Runtime.getRuntime().exec("hadoop fs -cp /user/root/jobTemplate /user/root/" + wfNum);
            p.waitFor();
            p = Runtime.getRuntime().exec("hadoop fs -put " + mergedPath + " /user/root/"+ wfNum);
            p.waitFor();
    	
    		OozieClient wc = new OozieClient("http://node1:11000/oozie");
    		Properties conf = wc.createConfiguration();
    		conf.setProperty(OozieClient.APP_PATH, "hdfs://node1:8020/user/root/"+ wfNum);
    		
    		conf.setProperty("jobTracker", "node1:8021");
    		conf.setProperty("nameNode", "hdfs://node1:8020");
    		conf.setProperty("queueName", "default");
    		conf.setProperty("processId", process_id);
    		String jobId = wc.run(conf);
    		dbc.updateProcessBusyStatus(process_id);
    		System.out.println("Workflow job submitted");
    		
    	    // wait until the workflow job finishes printing the status every 10 seconds
    	    while (wc.getJobInfo(jobId).getStatus() == WorkflowJob.Status.RUNNING) {
    	        System.out.println("Workflow job running ...");
    	        Thread.sleep(10 * 1000);
    	    }
    	
    	    // print the final status of the workflow job
    	    System.out.println("Workflow job completed ...");
    	    System.out.println(wc.getJobInfo(jobId));
    	    String result = dbc.getProcessResultFromActionLogger(process_id);
    	    dbc.updateProcessFinishedStatusAndSetResult(process_id, result);
    		
    	     return Response.ok("OK")
    	    			.header("Access-Control-Allow-Origin", "*")
    	    			.header("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS, HEAD")
    	    			.header("Access-Control-Allow-Credentials", "true")
    	    			.header("Access-Control-Allow-Headers", "origin, content-type, accept, authorization")    			
    	        		.build();
    	
    
    }
    
    private boolean checkWAR(String name) {
    	
    	String uppercase = name.toUpperCase();
    	return uppercase.endsWith(".WAR");
    }

    private static String copyFileToWorker(String host, File tmpFile) {
    	
	   	String CRLF = "\r\n";
	   	HttpURLConnection connection = null;
		HashMap<String, String> result = new HashMap<String, String>();
		
	   	try
	   	{
	   		
	   		connection = (HttpURLConnection) new URL(host).openConnection();
	   		
	       	connection.setRequestMethod("POST");
	       	connection.setRequestProperty("charset",  "utf-8");
	       	connection.setRequestProperty("Content-type", "application/octet-stream");
	       	
	        connection.setDoOutput(true);
	        
	        DataInputStream is = new DataInputStream(new FileInputStream(tmpFile));
	       	DataOutputStream wr = new DataOutputStream(connection.getOutputStream());
	       	IOUtils.copy(is, wr);
	       	wr.flush();
	       	wr.close();
		    
		    System.out.println("Response Code :" + connection.getResponseCode());
	       	InputStream res = connection.getInputStream();
	       	BufferedReader rd = new BufferedReader(new InputStreamReader(res));
	       	
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
	   	return "";
	   	
    }    
    
}

