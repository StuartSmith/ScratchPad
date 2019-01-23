
## Utility to list all assemblies and their .Net Version

A Utility that is given a directory and assembly wild cards, reports all assemblies and there associated dotnet versions. 

```
DotNetVersionReport C:\inetpub\wwwroot\ASM\All\bin Entriq*.dll Paymedia*.dll
```

Output should look as follows:
```
fileName=PayMedia.Security.Interfaces.dll FrameworkVersion=".NET Framework 4.7.2"
fileName=PayMedia.Security.Licensing.dll FrameworkVersion=".NET Framework 4.7.2"
fileName=PayMedia.Testing.StepDefinitions.dll FrameworkVersion=".NET Framework 4.7.2"
fileName=PayMedia.Utilities.CodeGen.dll FrameworkVersion=".NET Framework 4.7.2"
fileName=PayMedia.Utilities.CodeGen.UnitTests.dll FrameworkVersion=".NET Framework 4.7.2"
```

