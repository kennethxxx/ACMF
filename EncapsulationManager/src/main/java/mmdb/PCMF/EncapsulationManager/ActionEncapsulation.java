package mmdb.PCMF.EncapsulationManager;

import java.io.*;
import java.lang.reflect.Type;

import org.glassfish.jersey.media.multipart.FormDataContentDisposition;
import org.glassfish.jersey.media.multipart.FormDataParam;

import com.google.common.reflect.TypeToken;
import com.google.gson.Gson;

import javax.ws.rs.*;
import javax.ws.rs.core.*;

import java.math.BigInteger;
import java.net.HttpURLConnection;
import java.net.URL;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;

import org.apache.commons.io.IOUtils;
import org.apache.commons.io.FileUtils;


@Path("/operator")
public class ActionEncapsulation {
    
	private String _baseDir = "/tmp/file-accept/";
	DBC dbc = new DBC();
	
	
    @GET 
    @Path("getIt")
    @Produces("text/plain")
    public String getIt(String fileName) throws IOException {
    	
    	return "aaa";
    }
    
    /**
     * Response the specific file
     * @param fileName 
     * @return file content
     * @throws IOException
     */
    @GET 
    @Path("retrieve/{fileName}")
    
    public Response retrieve(@PathParam("fileName") String fileName) throws IOException { 
        File mergedPath = new File(this._baseDir, fileName);
        return Response.ok(mergedPath).build();
    }
    
    /**
     * delete the specific file
     * @param fileName
     * @return file deleting status
     * @return IOException
     * 
     */
    
    @DELETE
    @Path("delete/{fileName}")
    public Response delete(@PathParam("fileName") String fileName) throws IOException {
        File mergedPath = new File(this._baseDir, fileName);
        FileUtils.deleteQuietly(mergedPath);
        return Response.ok("OK").build();
    }

    /**
     * upload the specific file
     * @param fileName 
     * @param is
     * @return file uploading status
     * @throws IOException
     */
    @PUT
    @Path("insert")
    @Consumes(MediaType.MULTIPART_FORM_DATA) 
    public Response insert(@FormDataParam("action_info") String action_info,
    		@FormDataParam("file") InputStream is,
    		@FormDataParam("file") FormDataContentDisposition fileDetail ) throws IOException{
        
        OutputStream os = null;
        try {
            
        	if(checkJAR(fileDetail.getFileName())){
        		
	        	fileDetail.getType();
	        	String fileName = fileDetail.getFileName() + new Date().getTime();
	        	String tmpName = md5(fileName);
	            File mergedPath = new File(this._baseDir, tmpName);
	            os = new FileOutputStream(mergedPath);
	            IOUtils.copy(is, os);
	            
				Type mapType = new TypeToken<HashMap<String, String>>(){}.getType();  
				HashMap<String, String> input_para = new Gson().fromJson(action_info, mapType);
	            dbc.newAction(input_para);
	            ArrayList<String> hosts = dbc.getWorkerHost(input_para.get("action_info"));
	            for(String host : hosts ){
	            	copyFileToWorker(host, mergedPath);
	            }
	            
	            
        	}else {
        		
        		return Response.serverError().entity("file type is wrong").build();
        		
        	}
        
        } catch (IOException ex) {
            
            ex.printStackTrace();
            Response.status(500).build();
        } catch (NoSuchAlgorithmException e){
        	
        	e.printStackTrace();
        	Response.status(500).build();
        	
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
        
        return Response.ok("OK").build();
        
    }
    
    private String md5(String text) throws NoSuchAlgorithmException {
    	
    	MessageDigest md = MessageDigest.getInstance("MD5");
    	byte[] messageDiget = md.digest(text.getBytes());
    	BigInteger number = new BigInteger(1, messageDiget);
    	String tmpName = number.toString(16);

        while (tmpName.length() < 32) {
        	tmpName = "0" + tmpName;
        }
        
        return tmpName;
    	
    	
    }
    
    private boolean checkJAR(String name) {
    	
    	String uppercase = name.toUpperCase();
    	return uppercase.endsWith(".JAR");
    }

    private void copyFileToWorker(String host, File tmpFile) {
    	
	   	String CRLF = "\r\n";
	   	HttpURLConnection connection = null;
		HashMap<String, String> result = new HashMap<String, String>();
		
	   	try
	   	{
	   		
	   		connection = (HttpURLConnection) new URL(host).openConnection();
	   		
	       	connection.setRequestMethod("POST");
	       	connection.setRequestProperty("charset",  "utf-8");
	       	connection.setRequestProperty("Content-type", "application/x-www-form-urlencoded");
	       	
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

		   	
	   	} catch(IOException e) {

	   		e.printStackTrace();
	   		
	   	} catch(Exception e) {

	   		e.printStackTrace();
	   		
	   	}
	   	
    	
    }
    
}
