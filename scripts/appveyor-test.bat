@echo off
if "%PLATFORM%"=="x86" (
  "C:\Program Files (x86)\dotnet\dotnet.EXE" test --configuration %CONFIGURATION% --no-build AdvancedDLSupport.Tests
  "C:\Program Files (x86)\dotnet\dotnet.EXE" test --configuration %CONFIGURATION% --no-build Mono.DllMap.Tests
) else (
  "C:\Program Files\dotnet\dotnet.EXE" test --configuration %CONFIGURATION% --no-build AdvancedDLSupport.Tests
  "C:\Program Files\dotnet\dotnet.EXE" test --configuration %CONFIGURATION% --no-build Mono.DllMap.Tests
)
