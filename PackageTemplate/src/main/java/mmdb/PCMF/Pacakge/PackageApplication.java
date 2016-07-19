package mmdb.PCMF.Pacakge;


import javax.ws.rs.ApplicationPath;
import org.glassfish.jersey.server.ResourceConfig;

@ApplicationPath("/")
public class PackageApplication extends ResourceConfig {
    public PackageApplication() {
        super(PackageResource.class);
    }
}