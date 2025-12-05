namespace Aurora.Domain.Models;

public class ParseResult
{
    public bool Success { get; set; }
    public List<string> Errors { get; set; } = new();

    // Replace Dictionary with a small helper that exposes both indexer and typed properties
    public PartsModel Parts { get; set; } = new();

    public void AddError(string message)
    {
        Success = false;
        Errors.Add(message);
    }

    // Helper class
    public class PartsModel
    {
        private readonly Dictionary<string, string> _dict = new(StringComparer.OrdinalIgnoreCase);

        // Indexer keeps existing parser code working:
        // result.Parts["TYPE"] = "STU"; still works.
        public string? this[string key]
        {
            get => _dict.TryGetValue(key, out var v) ? v : null;
            set
            {
                if (value is null) _dict.Remove(key);
                else _dict[key] = value;
            }
        }

        // Typed convenience properties for tests that use r.Parts.Type
        public string? Type
        {
            get => this["TYPE"];
            set => this["TYPE"] = value;
        }

        public string? Prefix
        {
            get => this["PREFIX"];
            set => this["PREFIX"] = value;
        }

        public string? YearTerm
        {
            get => this["YEARTERM"];
            set => this["YEARTERM"] = value;
        }

        public string? Code
        {
            get => this["CODE"];
            set => this["CODE"] = value;
        }

        public string? Serial
        {
            get => this["SERIAL"];
            set => this["SERIAL"] = value;
        }

        public string? Checksum
        {
            get => this["CHECKSUM"];
            set => this["CHECKSUM"] = value;
        }

        // If any code expects a Dictionary, provide a snapshot
        public Dictionary<string,string> AsDictionary() => new(_dict, StringComparer.OrdinalIgnoreCase);
    }
}