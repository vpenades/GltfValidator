using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GltfValidator
{
    
    static class gltf_validator
    {
        public static string ValidatorExePath { get; set; }

        public static ValidationReport ValidateFile(string gltfFilePath, int timeOut = 10000)
        {
            return _ThrowMessage();
        }

        public static async Task<ValidationReport> ValidateFileAsync(string gltfFilePath, System.Threading.CancellationToken token)
        {
            return await Task.FromResult(_ThrowMessage());
        }

        public static async Task<ValidationReport> ValidateFileAsyncProcessX(string gltfFilePath, System.Threading.CancellationToken token)
        {
            return await Task.FromResult(_ThrowMessage());
        }

        private static ValidationReport _ThrowMessage()
        {
            throw new NotSupportedException("Supported platforms: net5.0 and net6.0 on Windows");
        }
    }
}
