<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="AMCSystems" generation="1" functional="0" release="0" Id="0688381d-378e-4089-9429-bce1e5281f92" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="AMCSystemsGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="IPS.Web:HttpIn" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/AMCSystems/AMCSystemsGroup/LB:IPS.Web:HttpIn" />
          </inToChannel>
        </inPort>
        <inPort name="IPS.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/AMCSystems/AMCSystemsGroup/LB:IPS.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Certificate|IPS.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" defaultValue="">
          <maps>
            <mapMoniker name="/AMCSystems/AMCSystemsGroup/MapCertificate|IPS.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </maps>
        </aCS>
        <aCS name="IPS.Web:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/AMCSystems/AMCSystemsGroup/MapIPS.Web:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="IPS.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="">
          <maps>
            <mapMoniker name="/AMCSystems/AMCSystemsGroup/MapIPS.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </maps>
        </aCS>
        <aCS name="IPS.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="">
          <maps>
            <mapMoniker name="/AMCSystems/AMCSystemsGroup/MapIPS.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </maps>
        </aCS>
        <aCS name="IPS.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="">
          <maps>
            <mapMoniker name="/AMCSystems/AMCSystemsGroup/MapIPS.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </maps>
        </aCS>
        <aCS name="IPS.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/AMCSystems/AMCSystemsGroup/MapIPS.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </maps>
        </aCS>
        <aCS name="IPS.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/AMCSystems/AMCSystemsGroup/MapIPS.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </maps>
        </aCS>
        <aCS name="IPS.WebInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/AMCSystems/AMCSystemsGroup/MapIPS.WebInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:IPS.Web:HttpIn">
          <toPorts>
            <inPortMoniker name="/AMCSystems/AMCSystemsGroup/IPS.Web/HttpIn" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:IPS.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput">
          <toPorts>
            <inPortMoniker name="/AMCSystems/AMCSystemsGroup/IPS.Web/Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </toPorts>
        </lBChannel>
        <sFSwitchChannel name="SW:IPS.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp">
          <toPorts>
            <inPortMoniker name="/AMCSystems/AMCSystemsGroup/IPS.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
          </toPorts>
        </sFSwitchChannel>
      </channels>
      <maps>
        <map name="MapCertificate|IPS.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" kind="Identity">
          <certificate>
            <certificateMoniker name="/AMCSystems/AMCSystemsGroup/IPS.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </certificate>
        </map>
        <map name="MapIPS.Web:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/AMCSystems/AMCSystemsGroup/IPS.Web/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapIPS.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" kind="Identity">
          <setting>
            <aCSMoniker name="/AMCSystems/AMCSystemsGroup/IPS.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </setting>
        </map>
        <map name="MapIPS.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" kind="Identity">
          <setting>
            <aCSMoniker name="/AMCSystems/AMCSystemsGroup/IPS.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </setting>
        </map>
        <map name="MapIPS.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" kind="Identity">
          <setting>
            <aCSMoniker name="/AMCSystems/AMCSystemsGroup/IPS.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </setting>
        </map>
        <map name="MapIPS.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/AMCSystems/AMCSystemsGroup/IPS.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </setting>
        </map>
        <map name="MapIPS.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/AMCSystems/AMCSystemsGroup/IPS.Web/Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </setting>
        </map>
        <map name="MapIPS.WebInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/AMCSystems/AMCSystemsGroup/IPS.WebInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="IPS.Web" generation="1" functional="0" release="0" software="D:\VS_Project\本體論系統\AMCSystems20121106-更新MMbyXML\AMCSystems20121030-更新DAUserFriendly\AMCSystems\csx\Debug\roles\IPS.Web" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="HttpIn" protocol="http" portRanges="80" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp" portRanges="3389" />
              <outPort name="IPS.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/AMCSystems/AMCSystemsGroup/SW:IPS.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;IPS.Web&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;IPS.Web&quot;&gt;&lt;e name=&quot;HttpIn&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="CloudDiv" defaultAmount="[2000,2000,2000]" defaultSticky="false" kind="Directory" />
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/AMCSystems/AMCSystemsGroup/IPS.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/AMCSystems/AMCSystemsGroup/IPS.WebInstances" />
            <sCSPolicyUpdateDomainMoniker name="/AMCSystems/AMCSystemsGroup/IPS.WebUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/AMCSystems/AMCSystemsGroup/IPS.WebFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="IPS.WebUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="IPS.WebFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="IPS.WebInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="a5bcdc69-6dda-45d6-9783-0c9e20ea5de8" ref="Microsoft.RedDog.Contract\ServiceContract\AMCSystemsContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="afc1542f-0e34-478e-8a10-2c7f84c9bab1" ref="Microsoft.RedDog.Contract\Interface\IPS.Web:HttpIn@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/AMCSystems/AMCSystemsGroup/IPS.Web:HttpIn" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="d3bb6793-5cc7-49d0-ab71-8de03a37d67e" ref="Microsoft.RedDog.Contract\Interface\IPS.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/AMCSystems/AMCSystemsGroup/IPS.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>