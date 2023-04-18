# Cps_dotnet-fx
CPS dotnet framework

- Dotnet framework    
[v4.6.2 Developer pack](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net462)

- IDE    
[Visual Studio 2017 (v15.9)](https://aka.ms/vs/15/release/vs_Community.exe) / 
[Visual Studio 2019 (v16.11)](https://aka.ms/vs/16/release/vs_Community.exe) / 
[Visual Studio 2022 (v17.x)](https://aka.ms/vs/17/release/vs_Community.exe)

## NuGet
> C:\Users\tdtc\AppData\Roaming\NuGet

### New
Put the following nuget packages into the Offline folder:
- google.protobuf:3.21.12    
System.Memory:4.5.5
- K4os.Compression.LZ4.Streams:1.3.5
```
K4os.Compression.LZ4:1.3.5
K4os.Hash.xxHash:1.0.8
```
- mysql.data:8.0.33
```
portable.bouncycastle:1.9.0
Google.Protobuf:3.21.12
K4os.Compression.LZ4.Streams:1.3.5
Portable.BouncyCastle:1.9.0
System.Buffers:4.5.1
System.Runtime.CompilerServices.Unsafe:6.0.0
```
- mysql.data.entityframework:8.0.33
```
entityframework:6.4.4
mysql.data:8.0.33
```

#### About dependency package update
Every update of mysql.data will update google.protobuf.

Other dependent package versions will not change.

### [Restore](https://learn.microsoft.com/en-us/nuget/reference/cli-reference/cli-ref-restore)
down [nuget CLI](https://dist.nuget.org/win-x86-commandline/latest/nuget.exe)
```cmd
nuget.exe restore C:\Users\tdtc\Documents\Cps_dotnet-fx\Cps_x32.sln
```

### [reinstalling and updating packages](https://learn.microsoft.com/en-us/nuget/consume-packages/reinstalling-and-updating-packages)
Upgrading the dotnet frame version requires upgrading the PACK information.
```
PMC> Update-Package -reinstall
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
