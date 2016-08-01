package mmdb.PCMF.EncapsulationManager;

import java.io.*;
import java.lang.reflect.Type;

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
import org.glassfish.jersey.message.internal.Utils;
import org.apache.commons.io.FileUtils;


@Path("/operator")
public class PackageDeployer {
    
	private String _AppDir = "/home/hduser/pcmf/package/";
	private String _TomcatHome = "/usr/tomcat";
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
        File mergedPath = new File(this._AppDir, fileName);
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
        File mergedPath = new File(this._AppDir, fileName);
        FileUtils.deleteQuietly(mergedPath);
        return Response.ok("OK").build();
    }

    @POST
    @Produces(MediaType.APPLICATION_OCTET_STREAM)
    @Path("deploy/{fileName}")
    public Response delpoy(@PathParam("fileName") String fileName, InputStream is ) throws IOException {
 
    	if(checkJAR(fileName)) {
    		FileOutputStream os = new FileOutputStream(new File( this._AppDir + fileName));		
    		IOUtils.copy(is,os);
            return Response.ok("true")
            		.build();
    	}else {
    		return Response.serverError().build();
    	}
		
		

    }
    
    private boolean checkJAR(String name) {
    	
    	String uppercase = name.toUpperCase();
    	return uppercase.endsWith(".JAR");
    }
    
    private void restartTomcat() {
    	
    	Process p;
    	try {
    		
    		p = Runtime.getRuntime().exec( "./" + this._TomcatHome + "/bin/shutdown.sh"); // shut down tomcat server
    		p.waitFor();
    		p = Runtime.getRuntime().exec( "./" + this._TomcatHome + "/bin/startup.sh");  // startup the tomcat server 
    		
    	} catch (Exception e) {
    		e.printStackTrace();
    	}
    	
    	
    }

}
