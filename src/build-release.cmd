@echo off

dotnet build -c:Release /p:Authors=vpenades GltfValidator.sln
dotnet pack -c:Release /p:Authors=vpenades GltfValidator.sln

pause