package mmdb.PCMF.Pacakge;

import java.io.*;
import javax.ws.rs.*;
import javax.ws.rs.core.Response;

import com.sun.jersey.multipart.FormDataParam;

@Path("/services")
public class PackageResource {
	
    @GET 
    @Path("getIt")
    @Produces("text/html")
    public Response getIt(@QueryParam("get") String get) throws IOException, InterruptedException {
    	
    	Thread.sleep(10000);
    	return Response.status(200).entity("I got " + get).build();
    }
    
    @POST 
    @Path("getItPost")
    @Produces("application/x-www-form-urlencoded")
    public Response getItPost(@FormParam("get") String get,
    						  @FormParam("get2") String get2) throws IOException {
    	
    	return Response.status(200).entity("I got " + get + "and got + " + get2).build();
    }    
       
}
