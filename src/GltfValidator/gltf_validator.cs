using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text.Json;

namespace GltfValidator
{
    /// <summary>
    /// Wraps Khronos GLTF Validator command line tool.
    /// </summary>
    /// <remarks>
    /// <see href="https://github.com/KhronosGroup/glTF-Validator"/>
    /// LINUX execution path has not been tested!
    /// </remarks>
    static class gltf_validator
    {
        static gltf_validator()
        {
            if (RuntimeInformation.OSArchitecture != Architecture.X64) return;

            ValidatorExePath = System.IO.Path.GetDirectoryName(typeof(gltf_validator).Assembly.Location);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ValidatorExePath = System.IO.Path.Combine(ValidatorExePath, "gltf_validator.exe");
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                ValidatorExePath = System.IO.Path.Combine(ValidatorExePath, "gltf_validator");
            }
        }

        public static string ValidatorExePath { get; set; }

        public static ValidationReport ValidateFile(string gltfFilePath, int timeOut = 10000)
        {
            if (string.IsNullOrWhiteSpace(ValidatorExePath)) return null;

            if (!System.IO.File.Exists(ValidatorExePath)) throw new System.IO.FileNotFoundException(ValidatorExePath);

            if (!System.IO.Path.IsPathRooted(gltfFilePath)) gltfFilePath = System.IO.Path.GetFullPath(gltfFilePath);

            var psi = new System.Diagnostics.ProcessStartInfo(ValidatorExePath);
            psi.Arguments = $"-p -r -a --stdout \"{gltfFilePath}\"";
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;            

            using (var p = System.Diagnostics.Process.Start(psi))
            {
                // To avoid deadlocks, always read the output stream first and then wait.  
                var mainReport = p.StandardOutput.ReadToEnd();

                if (!p.WaitForExit(timeOut)) // wait for a reasonable timeout
                {
                    try { p.Kill(); }
                    catch
                    {
                        throw new OperationCanceledException("Time out.");
                    }
                }                

                if (string.IsNullOrWhiteSpace(mainReport)) return null;

                return ValidationReport.Parse(mainReport);
            }
        }        
    }
}
