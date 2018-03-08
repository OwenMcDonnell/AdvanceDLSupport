@echo off
set TEMP_PLATFORM="%PLATFORM%"
set PLATFORM=


dotnet test --configuration %CONFIGURATION% --no-build AdvancedDLSupport.Tests
dotnet test --configuration %CONFIGURATION% --no-build Mono.DllMap.Tests
set PLATFORM="%TEMP_PLATFORM%"
