using System;

namespace GltfValidator
{

    /// <summary>
    /// Output of glTF-Validator
    /// </summary>
    public partial class ValidationReport
    {
        /// <summary>
        /// An object containing various metrics about the validated asset. May be undefined for
        /// invalid inputs.
        /// </summary>
        public Info Info { get; set; }

        public Issues Issues { get; set; }

        /// <summary>
        /// MIME type of validated asset. Undefined when file format is not recognized.
        /// </summary>
        public object MimeType { get; set; }

        /// <summary>
        /// URI of validated asset.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// UTC timestamp of validation time.
        /// </summary>
        public DateTimeOffset? ValidatedAt { get; set; }

        /// <summary>
        /// Version string of glTF-Validator. Must follow semver syntax.
        /// </summary>
        public string ValidatorVersion { get; set; }
    }

    /// <summary>
    /// An object containing various metrics about the validated asset. May be undefined for
    /// invalid inputs.
    /// </summary>
    public partial class Info
    {
        /// <summary>
        /// Names of glTF extensions required to properly load this asset.
        /// </summary>
        public string[] ExtensionsRequired { get; set; }

        /// <summary>
        /// Names of glTF extensions used somewhere in this asset.
        /// </summary>
        public string[] ExtensionsUsed { get; set; }

        /// <summary>
        /// Tool that generated this glTF model.
        /// </summary>
        public string Generator { get; set; }

        /// <summary>
        /// The minimum glTF version that this asset targets.
        /// </summary>
        public string MinVersion { get; set; }

        public Resource[] Resources { get; set; }

        /// <summary>
        /// The glTF version that this asset targets.
        /// </summary>
        public string Version { get; set; }
    }

    public partial class Resource
    {
        /// <summary>
        /// Byte length of the resource. Undefined when the resource wasn't available.
        /// </summary>
        public long? ByteLength { get; set; }

        /// <summary>
        /// Image-specific metadata.
        /// </summary>
        public Image Image { get; set; }

        public string MimeType { get; set; }
        public string Pointer { get; set; }
        public object Storage { get; set; }

        /// <summary>
        /// URI. Defined only for external resources.
        /// </summary>
        public string Uri { get; set; }
    }

    /// <summary>
    /// Image-specific metadata.
    /// </summary>
    public partial class Image
    {
        public long? Bits { get; set; }
        public long Width { get; set; }
        public long Height { get; set; }
        

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        [System.Text.Json.Serialization.JsonPropertyName("Format")]
        private String _Format;        

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        [System.Text.Json.Serialization.JsonPropertyName("Primaries")]
        private String _Primaries;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        [System.Text.Json.Serialization.JsonPropertyName("Transfer")]
        private String _Transfer;       

        public Format Format => Enum.TryParse<Format>(_Format, true, out var r) ? r : default;
        public Primaries Primaries => Enum.TryParse<Primaries>(_Primaries, true, out var r) ? r : default;
        public Transfer Transfer => Enum.TryParse<Transfer>(_Transfer, true, out var r) ? r : default;
    }

    public partial class Issues
    {
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
        public Message[] Messages { get; set; }
        public long NumErrors { get; set; }
        public long NumHints { get; set; }
        public long NumInfos { get; set; }
        public long NumWarnings { get; set; }

        /// <summary>
        /// Indicates that validation output is incomplete due to too many messages.
        /// </summary>
        public bool Truncated { get; set; }
    }

    public partial class Message
    {
        public string Code { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("Message")]
        public string Text { get; set; }

        /// <summary>
        /// Byte offset in GLB file. Applicable only to GLB issues.
        /// </summary>
        public long? Offset { get; set; }

        /// <summary>
        /// JSON Pointer to the object causing the issue.
        /// </summary>
        public string Pointer { get; set; }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        [System.Text.Json.Serialization.JsonPropertyName("Severity")]
        internal string _Severity = Severity.None.ToString();

        public Severity Severity => Enum.TryParse<Severity>(_Severity, true, out var r) ? r : default;
    }

    public enum Format { Luminance, LuminanceAlpha, Rgb, Rgba };

    public enum Primaries { Custom, Srgb };

    public enum Transfer { Custom, Linear, Srgb };

    public enum Severity { Error, Warning, Information, Hint, None };
}
