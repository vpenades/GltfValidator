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
            throw new NotSupportedException();
        }

        public static async Task<ValidationReport> ValidateFileAsync(string gltfFilePath, System.Threading.CancellationToken token)
        {
            throw new NotSupportedException();
        }

        public static async Task<ValidationReport> ValidateFileAsyncProcessX(string gltfFilePath, System.Threading.CancellationToken token)
        {
            throw new NotSupportedException();
        }
    }
}
