# Cps_dotnet-fx
CPS dotnet framework

- Dotnet framework    
[v4.5.2 Developer pack](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net452)

- IDE    
[Visual Studio 2013 Update 5](http://download.microsoft.com/download/A/F/9/AF95E6F8-2E6E-49D0-A48A-8E918D7FD768/vs2013.5.iso) ~ 
[Visual Studio 2019 (v16.11)](https://aka.ms/vs/16/release/vs_Community.exe)

## NuGet
> C:\Users\tdtc\AppData\Roaming\NuGet
NuGet.Config:
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <config>
    <add key="http_proxy" value="http://192.168.3.19:8580" />
    <add key="https_proxy" value="http://192.168.3.19:8580" />
  </config>
  <packageRestore>
    <add key="enabled" value="True" />
    <add key="automatic" value="True" />
  </packageRestore>
  <packageSources>
    <add key="nuget.org" value="https://www.nuget.org/api/v2/" />
    <add key="NuGet V3" value="https://api.nuget.org/v3/index.json" />
  </packageSources>
  <activePackageSource>
    <add key="nuget.org" value="https://www.nuget.org/api/v2/" />
  </activePackageSource>
</configuration>
```

### New
Update [NuGet.Tools.vsix](https://dist.nuget.org/visualstudio-2013-vsix/v2.12.0/NuGet.Tools.vsix)
and 
[Visual Studio 2013 NuGet package management not working](https://stackoverflow.com/a/63574949):
```Package Manager Console
PM > [Net.ServicePointManager]::SecurityProtocol=[Net.ServicePointManager]::SecurityProtocol-bOR [Net.SecurityProtocolType]::Tls12
```
Put the following nuget packages into the Offline folder:
- portable.bouncycastle:1.9.0
- google.protobuf:3.21.12
- entityframework:6.4.4
- mysql.data:8.0.32.1
- mysql.data.entityframework:8.0.32

### [Restore](https://learn.microsoft.com/en-us/nuget/reference/cli-reference/cli-ref-restore)
down [nuget CLI](https://dist.nuget.org/win-x86-commandline/latest/nuget.exe)
```cmd
nuget.exe config -set http_proxy=http://my.proxy.address:port
nuget.exe config -set https_proxy=http://my.proxy.address:port
nuget.exe restore C:\Users\tdtc\Documents\Cps_dotnet-fx\Cps_x32.sln
```

## Unsupported features
supported in EF core, but not in EF.

### Decimal precision and scale
```c#
TypeName = "decimal(9, 3)"
```

# Ref
- [Entity Framework 6 Support](https://dev.mysql.com/doc/connector-net/en/connector-net-entityframework60.html)
- [Common NuGet configurations](https://learn.microsoft.com/en-us/nuget/consume-packages/configuring-nuget-behavior)
