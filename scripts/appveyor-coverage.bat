@echo off

:: Determine the output folder of the binaries
set OUTPUT_DIR=%CONFIGURATION%
if "%PLATFORM%"=="x86" (
	set OUTPUT_DIR=x86\%CONFIGURATION%
)

if "%PLATFORM%"=="x64" (
	set OUTPUT_DIR=x64\%CONFIGURATION%
)

set TEMP_PLATFORM=%PLATFORM%
set PLATFORM=

:: Install AltCover
nuget install altcover -OutputDirectory altcover -Version 1.6.230


:: Instrument the test assemblies
if "%TEMP_PLATFORM%"=="x86" (
	"C:\Program Files (x86)\dotnet\dotnet.EXE" run^
 --project altcover\altcover.1.6.230\tools\netcoreapp2.0\AltCover\altcover.core.fsproj --configuration %CONFIGURATION% --^
 -i=AdvancedDLSupport.Tests\bin\%OUTPUT_DIR%\netcoreapp2.0 -o=instrumented-adl -x=coverage-adl.xml^
 --assemblyExcludeFilter=.+\.Tests --assemblyExcludeFilter=AltCover.+ --assemblyExcludeFilter=Mono\.DllMap.+

	"C:\Program Files (x86)\dotnet\dotnet.EXE" run^
 --project altcover\altcover.1.6.230\tools\netcoreapp2.0\AltCover\altcover.core.fsproj --configuration %CONFIGURATION% --^
 -i=Mono.DllMap.Tests\bin\%OUTPUT_DIR%\netcoreapp2.0 -o=instrumented-mdl -x=coverage-mdl.xml^
 --assemblyExcludeFilter=.+\.Tests --assemblyExcludeFilter=AltCover.+
) else (
	"C:\Program Files\dotnet\dotnet.EXE" run^
 --project altcover\altcover.1.6.230\tools\netcoreapp2.0\AltCover\altcover.core.fsproj --configuration %CONFIGURATION% --^
 -i=AdvancedDLSupport.Tests\bin\%OUTPUT_DIR%\netcoreapp2.0 -o=instrumented-adl -x=coverage-adl.xml^
 --assemblyExcludeFilter=.+\.Tests --assemblyExcludeFilter=AltCover.+ --assemblyExcludeFilter=Mono\.DllMap.+

	"C:\Program Files\dotnet\dotnet.EXE" run^
 --project altcover\altcover.1.6.230\tools\netcoreapp2.0\AltCover\altcover.core.fsproj --configuration %CONFIGURATION% --^
 -i=Mono.DllMap.Tests\bin\%OUTPUT_DIR%\netcoreapp2.0 -o=instrumented-mdl -x=coverage-mdl.xml^
 --assemblyExcludeFilter=.+\.Tests --assemblyExcludeFilter=AltCover.+

)

:: Copy them to their build directories
copy /y instrumented-adl\* AdvancedDLSupport.Tests\bin\%OUTPUT_DIR%\netcoreapp2.0
copy /y instrumented-mdl\* Mono.DllMap.Tests\bin\%OUTPUT_DIR%\netcoreapp2.0

:: And run coverage
if "%TEMP_PLATFORM%"=="x86" (
	"C:\Program Files (x86)\dotnet\dotnet.EXE" run^
 --project altcover\altcover.1.6.230\tools\netcoreapp2.0\AltCover\altcover.core.fsproj --no-build --configuration %CONFIGURATION% --^
 runner -x "dotnet" -r "AdvancedDLSupport.Tests\bin\%OUTPUT_DIR%\netcoreapp2.0" --^
 test AdvancedDLSupport.Tests --no-build

	"C:\Program Files (x86)\dotnet\dotnet.EXE" run^
 --project altcover\altcover.1.6.230\tools\netcoreapp2.0\AltCover\altcover.core.fsproj --no-build --configuration %CONFIGURATION% --^
 runner -x "dotnet" -r "Mono.DllMap.Tests\bin\%OUTPUT_DIR%\netcoreapp2.0" --^
 test Mono.DllMap.Tests --no-build
) else (
	"C:\Program Files\dotnet\dotnet.EXE" run^
 --project altcover\altcover.1.6.230\tools\netcoreapp2.0\AltCover\altcover.core.fsproj --no-build --configuration %CONFIGURATION% --^
 runner -x "dotnet" -r "AdvancedDLSupport.Tests\bin\%OUTPUT_DIR%\netcoreapp2.0" --^
 test AdvancedDLSupport.Tests --no-build

	"C:\Program Files\dotnet\dotnet.EXE" run^
 --project altcover\altcover.1.6.230\tools\netcoreapp2.0\AltCover\altcover.core.fsproj --no-build --configuration %CONFIGURATION% --^
 runner -x "dotnet" -r "Mono.DllMap.Tests\bin\%OUTPUT_DIR%\netcoreapp2.0" --^
 test Mono.DllMap.Tests --no-build

)
