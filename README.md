# Cps_dotnet-fx
CPS dotnet framework

## project
- Non-local connection DB:    
First, add a [user - tdtc2022](https://www.cnblogs.com/xiaobin-hlj80/p/5189419.html);    
Then, modify cpsConfig.xml

- Requires installation of [SHA-2](https://tdtc-hrb.github.io/ops-win/posts/sha2-install).

### NuGet
In earlier versions of Visual Studio 2013/2015, you need to configure NuGet.
- NuGet.Config(%USERPROFILE%\AppData\Roaming\NuGet)
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
#### [Restore](https://learn.microsoft.com/en-us/nuget/reference/cli-reference/cli-ref-restore)
down [nuget CLI](https://dist.nuget.org/win-x86-commandline/latest/nuget.exe)
```cmd
cd <ProjectDirectory>
nuget.exe restore Cps_x32.sln
```

#### [reinstalling and updating packages](https://learn.microsoft.com/en-us/nuget/consume-packages/reinstalling-and-updating-packages)
Upgrading the dotnet framework version requires upgrading the PACK information.
```
PMC> Update-Package -reinstall
```

### gen exec
- [.NET Framework Developer pack](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48)
- [Build Tools for Visual Studio 2022](https://visualstudio.microsoft.com/downloads/?cid=learn-onpage-download-cta#build-tools-for-visual-studio-2022)
```
cd <ProjectDirectory>
MsBuild.exe Cps_x32.sln
```
Add the following directories to the system variables:
```
C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin
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
- [How to build .NET 4.6 Framework app without Visual Studio installed?](https://stackoverflow.com/a/32070575)
