# GltfValidator
![GitHub](https://img.shields.io/github/license/vpenades/GltfValidator)
[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/GltfValidator)](https://www.nuget.org/packages/GltfValidator)

### Overview

This is a small .Net wrapper over Khronos [glTF-Validator](https://github.com/KhronosGroup/glTF-Validator)

Notice that it wraps the command line executable provided by gltf-Validator.

The nuget package includes the gltf-validator command line executable which is invoked as a process under the hood.

### Project status

Current version uses `glTF-Validator v2.0.0-dev.3.8`

Right now, only windows platform is supported, but the project could be improved to cover linux and mac (help needed).

### Usage

The main usage of this library is for unit tests and content pipelines.

Simply reference this package and call:

```c#
using GltfValidator;

var report = ValidationReport.Validate("avocado.gltf");
```


### Credits

Thanks to Khronos for developing [glTF-Validator](https://github.com/KhronosGroup/glTF-Validator).
