package mmdb.PCMF.EncapsulationManager.PackageDeployer;

import java.io.*;
import javax.ws.rs.*;
import javax.ws.rs.core.*;

import org.apache.commons.io.IOUtils;
import org.apache.commons.io.FileUtils;


@Path("/operator")
public class PackageDeployResource {
    
	private String _AppDir = "/usr/tomcat/webapps/";
	private String _TomcatHome = "/usr/tomcat";
//	DBC dbc = new DBC();
	
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
 
    	if(checkWAR(fileName)) {
    		
    		FileOutputStream os = new FileOutputStream(new File( this._AppDir + fileName));		
    		IOUtils.copy(is,os);
    		restartTomcat();
            return Response.ok("true")
            		.build();
    	}else {
    		return Response.serverError().build();
    	}

    }
    
    private boolean checkWAR(String name) {
    	
    	String uppercase = name.toUpperCase();
    	return uppercase.endsWith(".WAR");
    }
    
    private void restartTomcat() {
    	
    	Process p;
    	try {
    		
    		p = Runtime.getRuntime().exec( "sh " + this._TomcatHome + "/bin/shutdown.sh"); // shut down tomcat server
    		p.waitFor();
    		p = Runtime.getRuntime().exec( "sh " + this._TomcatHome + "/bin/startup.sh");  // startup the tomcat server 
    		p.waitFor();
    		
    	} catch (Exception e) {
    		e.printStackTrace();
    	}
    	
    	
    }

}
