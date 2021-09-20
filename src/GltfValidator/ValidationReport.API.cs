﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GltfValidator
{
    /// <summary>
    /// Represents the report generated by glTF validator.
    /// </summary>
    /// <remarks>
    /// <see href="https://github.com/KhronosGroup/glTF-Validator/blob/main/docs/validation.schema.json">Schema.</see>
    /// <para>
    /// c# schema generators
    /// https://quicktype.io/csharp
    /// https://github.com/microsoft/jschema (outdated)
    /// </para>    
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{ToDebuggerDisplayString(),nq}")]
    partial class ValidationReport
    {
        #region debug

        private string ToDebuggerDisplayString()
        {
            var text = System.IO.Path.GetFileName(Uri);

            if (HasUnsupportedExtensions) return text + " Has unsupported extensions";            

            text = GetSymbol(this.Severity) + " " + text;

            return text;
        }

        internal static string GetSymbol(Severity severity)
        {
            switch(severity)
            {
                case Severity.Error: return "⛔"; // ❎
                case Severity.Warning: return "⚠";
                case Severity.Information: return "ℹ";
                case Severity.Hint: return "ℹ"; // ☑✔
                case Severity.None: return "🆗"; // ✅🆗
            }

            throw new NotImplementedException();
        }

        #endregion

        #region validation

        public static ValidationReport Validate(string filePath)
        {
            return gltf_validator.ValidateFile(filePath);
        }

        #endregion

        #region serialization

        public static ValidationReport Parse(string reportJson)
        {
            var options = new JsonSerializerOptions();

            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.PropertyNameCaseInsensitive = true;
            options.IgnoreNullValues = true;
            options.IncludeFields = true;

            var report = JsonSerializer.Deserialize<ValidationReport>(reportJson, options);

            report.Issues._FixMissingSeverity();

            return report;
        }
        
        public override string ToString()
        {
            var options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.IgnoreReadOnlyFields = true;
            options.IgnoreReadOnlyProperties = true;
            return JsonSerializer.Serialize(this, options);
        }

        #endregion

        #region properties

        public bool HasUnsupportedExtensions => Issues.Messages.Any(item => item.Text == "UNSUPPORTED_EXTENSION");

        public Severity Severity => Issues.Severity;        
        
        #endregion
    }

    [System.Diagnostics.DebuggerDisplay("{Severity}")]
    partial class Issues
    {
        public Severity Severity
        {
            get
            {
                if (this.NumErrors > 0) return Severity.Error;
                if (this.NumWarnings > 0) return Severity.Warning;
                if (this.NumInfos > 0) return Severity.Information;
                if (this.NumHints > 0) return Severity.Hint;
                return Severity.None;
            }
        }

        /// <summary>
        /// For some reason, gltf_validator is not serializing "severity" field
        /// </summary>
        internal void _FixMissingSeverity()
        {
            if (this.Messages == null) this.Messages = Array.Empty<Message>();

            if (this.Messages.All(item => item.Severity != Severity.None)) return;

            // we assume messages appear in order of severity

            var nume = this.NumErrors;
            var numw = this.NumWarnings;
            var numi = this.NumInfos;
            var numh = this.NumHints;

            foreach(var msg in this.Messages)
            {
                if (0 < nume--) { msg._Severity = "error"; continue; }
                if (0 < numw--) { msg._Severity = "warning"; continue; }
                if (0 < numi--) { msg._Severity = "information"; continue; }
                if (0 < numh--) { msg._Severity = "hint"; continue; }
            }
        }
    }

    [System.Diagnostics.DebuggerDisplay("{Generator} {Version}")]
    partial class Info
    { }

    [System.Diagnostics.DebuggerDisplay("{Uri}")]
    partial class Resource
    { }

    [System.Diagnostics.DebuggerDisplay("{ToDebuggerDisplayString(),nq}")]
    partial class Message
    {
        #region debug

        private string ToDebuggerDisplayString()
        {
            return ValidationReport.GetSymbol(Severity) + " " + Code + ": " + Text;
        }

        #endregion

    }

    [System.Diagnostics.DebuggerDisplay("{Width}x{Height}x{Format}")]
    partial class Image
    { }
}