# Cps_dotnet-fx
CPS dotnet framework

- Dotnet framework    
[v4.5.2 Developer pack](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net452)

## NuGet
Update [NuGet.Tools.vsix](https://nugetteam.gallerycdn.vsassets.io/extensions/nugetteam/nugetpackagemanagerforvisualstudio2013/2.12.0.817/1488284100949/105933/11/NuGet.Tools.vsix)
```
2.8 -> 2.12
```
[Visual Studio 2013 NuGet package management not working](https://stackoverflow.com/a/63574949):
```Package Manager Console
PM > [Net.ServicePointManager]::SecurityProtocol=[Net.ServicePointManager]::SecurityProtocol-bOR [Net.SecurityProtocolType]::Tls12
```

### packages
Put the following nuget packages into the Offline folder:

- portable.bouncycastle
```
1.9.0
```
- google.protobuf
```
3.21.12
```
- entityframework
```
6.4.4
```
- mysql.data
```
8.0.32.1
```
- mysql.data.entityframework
```
8.0.32
```

## Unsupported features
supported in EF core, but not in EF.

### Decimal precision and scale
```c#
TypeName = "decimal(9, 3)")
```

# Ref
- [Entity Framework 6 Support](https://dev.mysql.com/doc/connector-net/en/connector-net-entityframework60.html)
