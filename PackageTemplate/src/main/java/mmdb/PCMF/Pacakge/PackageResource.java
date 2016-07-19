package mmdb.PCMF.Pacakge;

import java.io.*;
import javax.ws.rs.*;
import javax.ws.rs.core.Response;

@Path("/services")
public class PackageResource {
	
    @GET 
    @Path("getIt")
    @Produces("text/html")
    public Response getIt(@QueryParam("get") String get) throws IOException {
    	
    	return Response.status(200).entity("I got " + get).build();
    }
    
    @POST 
    @Path("getItPost")
    @Produces("text/html")
    public Response getItPost(@QueryParam("get") String get) throws IOException {
    	
    	return Response.status(200).entity("I got " + get).build();
    }    
       
}
