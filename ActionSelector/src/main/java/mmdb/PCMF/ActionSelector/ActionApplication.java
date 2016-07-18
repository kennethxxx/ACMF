package mmdb.PCMF.ActionSelector;


import javax.ws.rs.ApplicationPath;

import org.glassfish.jersey.server.ResourceConfig;

@ApplicationPath("/")
public class ActionApplication extends ResourceConfig {
    public ActionApplication() {
        super(Parser.class);
    }
}