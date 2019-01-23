# ScratchPad
This repository is to hold small utilities used for different projects or to answer questions on stack overflow

-------------------
### DotNetVersionReport
Utility given a directory and wild cards for assemblies returns a report of assemblies and there .Net Versions 

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

### VisBuildTFSUpdate
This Utility  to update the procedure Create_Retrieve_TFS_Base_path_Script in a Visual Build script file. The procedure Create_Retrieve_TFS_Base_path_Script retrieves the path the TFS_EXE location from the system. If the procedure does not exist it will be created, if it does exist then it will be updated to reflect the contents of the procedure found in the source code of this application.
