@echo off

set GETTIMEKEY=powershell get-date -format "{yyyyMMddHHmm}"
for /f %%i in ('%GETTIMEKEY%') do set TIMEKEY=%%i

set VERSIONSUFFIX=dev.3.5.%TIMEKEY%

echo Building %VERSIONSUFFIX%

dotnet build -c:Release --version-suffix %VERSIONSUFFIX% /p:Authors=vpenades GltfValidator.sln
dotnet pack -c:Release --version-suffix %VERSIONSUFFIX% /p:Authors=vpenades GltfValidator.sln

pause