package mmdb.PCMF.EncapsulationManager;


import javax.ws.rs.ApplicationPath;

import org.glassfish.jersey.media.multipart.MultiPartFeature;
import org.glassfish.jersey.server.ResourceConfig;

@ApplicationPath("/")
public class ActionEncapsulationApplication extends ResourceConfig {
    public ActionEncapsulationApplication() {
        super(ActionEncapsulation.class);
    }
}