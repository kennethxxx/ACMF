package mmdb.PCMF.EncapsulationManager;

import javax.ws.rs.ApplicationPath;

import org.glassfish.jersey.media.multipart.MultiPartFeature;
import org.glassfish.jersey.server.ResourceConfig;

@ApplicationPath("/")
public class ActionApplication extends ResourceConfig {
    public ActionApplication() {
        super(ActionResource.class);
    }
}