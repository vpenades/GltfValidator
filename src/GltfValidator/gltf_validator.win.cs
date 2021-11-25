using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GltfValidator
{
    /// <summary>
    /// Wraps Khronos GLTF Validator command line tool.
    /// </summary>
    /// <remarks>
    /// <see href="https://github.com/KhronosGroup/glTF-Validator"/>    
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
            var psi = CreateStartInfo(gltfFilePath);

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

        public static async Task<ValidationReport> ValidateFileAsync(string gltfFilePath, System.Threading.CancellationToken token)
        {
            var psi = CreateStartInfo(gltfFilePath);

            using (var p = System.Diagnostics.Process.Start(psi))
            {
                // To avoid deadlocks, always read the output stream first and then wait.
                var mainReport = await p.StandardOutput.ReadToEndAsync();

                await Task.Run(p.WaitForExit, token);

                if (string.IsNullOrWhiteSpace(mainReport)) return null;

                return ValidationReport.Parse(mainReport);
            }
        }

        public static async Task<ValidationReport> ValidateFileAsyncProcessX(string gltfFilePath, System.Threading.CancellationToken token)
        {
            var psi = CreateStartInfo(gltfFilePath);

            Cysharp.Diagnostics.ProcessX.AcceptableExitCodes = new int[] { 0 };

            var lines = await Cysharp.Diagnostics.ProcessX
                .StartAsync(psi)
                .ToTask(token);            

            return ValidationReport.Parse(string.Join("\r\n", lines));
        }


        private static System.Diagnostics.ProcessStartInfo CreateStartInfo(string gltfFilePath)
        {
            if (string.IsNullOrWhiteSpace(ValidatorExePath)) return null;

            if (!System.IO.File.Exists(ValidatorExePath)) throw new System.IO.FileNotFoundException(ValidatorExePath);

            if (!System.IO.Path.IsPathRooted(gltfFilePath)) gltfFilePath = System.IO.Path.GetFullPath(gltfFilePath);

            var psi = new System.Diagnostics.ProcessStartInfo(ValidatorExePath);
            psi.Arguments = $"-p -r -a -t -o \"{gltfFilePath}\"";
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;

            return psi;
        }
    }
}
