# GltfValidator

### Overview

This is a small .Net wrapper over Khronos [glTF-Validator](https://github.com/KhronosGroup/glTF-Validator)

The main usage of this library is on unit tests and content pipelines.

Current version uses glTF-Validator is v2.0.0-dev.3.5 windows binaries, but it could be improved to
support more environments also supported by glTF-Validator (linux and mac).

### Usage

Simple reference this package and call:

```c#
using GltfValidator;

var report = ValidationReport.Validate("avocado.gltf");
```


### Credits

Thanks to Khronos for developing [glTF-Validator](https://github.com/KhronosGroup/glTF-Validator).
