<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="WorkerController" generation="1" functional="0" release="0" Id="fc33ff0c-9e29-4433-8aa0-e94963e814f0" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="WorkerControllerGroup" generation="1" functional="0" release="0">
      <settings>
        <aCS name="Certificate|WorkerRoleController:mmdb" defaultValue="">
          <maps>
            <mapMoniker name="/WorkerController/WorkerControllerGroup/MapCertificate|WorkerRoleController:mmdb" />
          </maps>
        </aCS>
        <aCS name="WorkerRoleController:ScalingConfigurationBlobContainer" defaultValue="">
          <maps>
            <mapMoniker name="/WorkerController/WorkerControllerGroup/MapWorkerRoleController:ScalingConfigurationBlobContainer" />
          </maps>
        </aCS>
        <aCS name="WorkerRoleController:ScalingConfigurationStorageConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/WorkerController/WorkerControllerGroup/MapWorkerRoleController:ScalingConfigurationStorageConnectionString" />
          </maps>
        </aCS>
        <aCS name="WorkerRoleController:WorkerRoleControllerConfig" defaultValue="">
          <maps>
            <mapMoniker name="/WorkerController/WorkerControllerGroup/MapWorkerRoleController:WorkerRoleControllerConfig" />
          </maps>
        </aCS>
        <aCS name="WorkerRoleController:WRScalingRule" defaultValue="">
          <maps>
            <mapMoniker name="/WorkerController/WorkerControllerGroup/MapWorkerRoleController:WRScalingRule" />
          </maps>
        </aCS>
        <aCS name="WorkerRoleControllerInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/WorkerController/WorkerControllerGroup/MapWorkerRoleControllerInstances" />
          </maps>
        </aCS>
      </settings>
      <maps>
        <map name="MapCertificate|WorkerRoleController:mmdb" kind="Identity">
          <certificate>
            <certificateMoniker name="/WorkerController/WorkerControllerGroup/WorkerRoleController/mmdb" />
          </certificate>
        </map>
        <map name="MapWorkerRoleController:ScalingConfigurationBlobContainer" kind="Identity">
          <setting>
            <aCSMoniker name="/WorkerController/WorkerControllerGroup/WorkerRoleController/ScalingConfigurationBlobContainer" />
          </setting>
        </map>
        <map name="MapWorkerRoleController:ScalingConfigurationStorageConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/WorkerController/WorkerControllerGroup/WorkerRoleController/ScalingConfigurationStorageConnectionString" />
          </setting>
        </map>
        <map name="MapWorkerRoleController:WorkerRoleControllerConfig" kind="Identity">
          <setting>
            <aCSMoniker name="/WorkerController/WorkerControllerGroup/WorkerRoleController/WorkerRoleControllerConfig" />
          </setting>
        </map>
        <map name="MapWorkerRoleController:WRScalingRule" kind="Identity">
          <setting>
            <aCSMoniker name="/WorkerController/WorkerControllerGroup/WorkerRoleController/WRScalingRule" />
          </setting>
        </map>
        <map name="MapWorkerRoleControllerInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/WorkerController/WorkerControllerGroup/WorkerRoleControllerInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="WorkerRoleController" generation="1" functional="0" release="0" software="D:\研究所\WorkerController\WorkerController\csx\Release\roles\WorkerRoleController" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="768" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="ScalingConfigurationBlobContainer" defaultValue="" />
              <aCS name="ScalingConfigurationStorageConnectionString" defaultValue="" />
              <aCS name="WorkerRoleControllerConfig" defaultValue="" />
              <aCS name="WRScalingRule" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;WorkerRoleController&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;WorkerRoleController&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0mmdb" certificateStore="My" certificateLocation="User">
                <certificate>
                  <certificateMoniker name="/WorkerController/WorkerControllerGroup/WorkerRoleController/mmdb" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="mmdb" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/WorkerController/WorkerControllerGroup/WorkerRoleControllerInstances" />
            <sCSPolicyUpdateDomainMoniker name="/WorkerController/WorkerControllerGroup/WorkerRoleControllerUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/WorkerController/WorkerControllerGroup/WorkerRoleControllerFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="WorkerRoleControllerUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="WorkerRoleControllerFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="WorkerRoleControllerInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
</serviceModel>