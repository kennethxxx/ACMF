<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="WorkerController" generation="1" functional="0" release="0" Id="359a2855-6f8a-439a-9446-ed6cb20065e9" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="WorkerControllerGroup" generation="1" functional="0" release="0">
      <settings>
        <aCS name="Certificate|WorkerRoleController:Mycert" defaultValue="">
          <maps>
            <mapMoniker name="/WorkerController/WorkerControllerGroup/MapCertificate|WorkerRoleController:Mycert" />
          </maps>
        </aCS>
        <aCS name="WorkerRoleController:DriverOption" defaultValue="">
          <maps>
            <mapMoniker name="/WorkerController/WorkerControllerGroup/MapWorkerRoleController:DriverOption" />
          </maps>
        </aCS>
        <aCS name="WorkerRoleController:PrivateWorkerRoleControllerConfig" defaultValue="">
          <maps>
            <mapMoniker name="/WorkerController/WorkerControllerGroup/MapWorkerRoleController:PrivateWorkerRoleControllerConfig" />
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
        <aCS name="WorkerRoleController:StoragesKey" defaultValue="">
          <maps>
            <mapMoniker name="/WorkerController/WorkerControllerGroup/MapWorkerRoleController:StoragesKey" />
          </maps>
        </aCS>
        <aCS name="WorkerRoleController:StoragesName" defaultValue="">
          <maps>
            <mapMoniker name="/WorkerController/WorkerControllerGroup/MapWorkerRoleController:StoragesName" />
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
        <map name="MapCertificate|WorkerRoleController:Mycert" kind="Identity">
          <certificate>
            <certificateMoniker name="/WorkerController/WorkerControllerGroup/WorkerRoleController/Mycert" />
          </certificate>
        </map>
        <map name="MapWorkerRoleController:DriverOption" kind="Identity">
          <setting>
            <aCSMoniker name="/WorkerController/WorkerControllerGroup/WorkerRoleController/DriverOption" />
          </setting>
        </map>
        <map name="MapWorkerRoleController:PrivateWorkerRoleControllerConfig" kind="Identity">
          <setting>
            <aCSMoniker name="/WorkerController/WorkerControllerGroup/WorkerRoleController/PrivateWorkerRoleControllerConfig" />
          </setting>
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
        <map name="MapWorkerRoleController:StoragesKey" kind="Identity">
          <setting>
            <aCSMoniker name="/WorkerController/WorkerControllerGroup/WorkerRoleController/StoragesKey" />
          </setting>
        </map>
        <map name="MapWorkerRoleController:StoragesName" kind="Identity">
          <setting>
            <aCSMoniker name="/WorkerController/WorkerControllerGroup/WorkerRoleController/StoragesName" />
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
          <role name="WorkerRoleController" generation="1" functional="0" release="0" software="C:\研究所\WorkerController\WorkerController\csx\Debug\roles\WorkerRoleController" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="768" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="DriverOption" defaultValue="" />
              <aCS name="PrivateWorkerRoleControllerConfig" defaultValue="" />
              <aCS name="ScalingConfigurationBlobContainer" defaultValue="" />
              <aCS name="ScalingConfigurationStorageConnectionString" defaultValue="" />
              <aCS name="StoragesKey" defaultValue="" />
              <aCS name="StoragesName" defaultValue="" />
              <aCS name="WorkerRoleControllerConfig" defaultValue="" />
              <aCS name="WRScalingRule" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;WorkerRoleController&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;WorkerRoleController&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0Mycert" certificateStore="My" certificateLocation="User">
                <certificate>
                  <certificateMoniker name="/WorkerController/WorkerControllerGroup/WorkerRoleController/Mycert" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="Mycert" />
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