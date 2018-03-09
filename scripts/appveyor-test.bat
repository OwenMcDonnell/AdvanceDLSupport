@echo off
if "%PLATFORM%"=="x86" (
  "C:\Program Files (x86)\dotnet\dotnet.EXE" test --configuration %CONFIGURATION% --no-build AdvancedDLSupport.Tests
  "C:\Program Files (x86)\dotnet\dotnet.EXE" test --configuration %CONFIGURATION% --no-build Mono.DllMap.Tests
) 
if "%PLATFORM%"=="x64" (
  "C:\Program Files\dotnet\dotnet.EXE" test --configuration %CONFIGURATION% --no-build AdvancedDLSupport.Tests
  "C:\Program Files\dotnet\dotnet.EXE" test --configuration %CONFIGURATION% --no-build Mono.DllMap.Tests
)
if "%PLATFORM%"=="Any cpu" (
  "C:\Program Files\dotnet\dotnet.EXE" test --configuration %CONFIGURATION% --no-build AdvancedDLSupport.Tests -o AdvancedDLSupport.tests/bin/Debug/netcoreapp2.0/
  "C:\Program Files\dotnet\dotnet.EXE" test --configuration %CONFIGURATION% --no-build Mono.DllMap.Tests -o Mono.DllMap.Tests/bin/Debug/netcoreapp2.0/
)
