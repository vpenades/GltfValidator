using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using CliWrap;

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

            ValidatorExePath = System.AppContext.BaseDirectory;

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
            using(var cs = new CancellationTokenSource(timeOut))
            {
                return ValidateFileAsync(gltfFilePath, cs.Token).ConfigureAwait(false).GetAwaiter().GetResult();

                // return Task.Run(async () => await ValidateFileAsync(gltfFilePath, cs.Token)).Result;
            }
        }        

        public static async Task<ValidationReport> ValidateFileAsync(string gltfFilePath, System.Threading.CancellationToken token)
        {
            var psi = CreateStartInfo(gltfFilePath);

            var stdOutBuffer = new StringBuilder();
            var stdErrBuffer = new StringBuilder();

            var result = await CliWrap.Cli
                .Wrap(psi.FileName)                
                .WithArguments(psi.Arguments)
                .WithValidation(CliWrap.CommandResultValidation.None)
                .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
                .ExecuteAsync(token)
                .ConfigureAwait(false);         

            var mainReport = stdOutBuffer.ToString();

            if (string.IsNullOrWhiteSpace(mainReport)) return null;

            return ValidationReport.Parse(mainReport);
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
