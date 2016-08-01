package mmdb.PCMF.EncapsulationManager;


import javax.ws.rs.ApplicationPath;

import org.glassfish.jersey.server.ResourceConfig;

@ApplicationPath("/")
public class PackageDeployerApplication extends ResourceConfig {
    public PackageDeployerApplication() {
        super(PackageDeployer.class);
    }
}