using Aurora.Domain.Models;
using System.Text.RegularExpressions;

namespace Aurora.Domain.Services;

public class UniversityCodeParser
{
    private static readonly HashSet<string> AllowedTypes = new() { "STU", "MOD", "STF" };

    public ParseResult Parse(string input)
    {
        var result = new ParseResult();
        result.Success = true;

        if (string.IsNullOrWhiteSpace(input))
        {
            result.AddError("Input is empty.");
            return result;
        }

        string normalized = input.Trim().ToUpperInvariant();

        // ساختار: AU-TYPE-YEARTERM-CODE-NNNN-CC
        var parts = normalized.Split('-');

        if (parts.Length != 6)
        {
            result.AddError("Invalid format. Expected 6 hyphen-separated parts.");
            return result;
        }

        string prefix = parts[0];
        string type = parts[1];
        string yearTerm = parts[2];
        string code = parts[3];
        string serial = parts[4];
        string checksum = parts[5];

        // ذخیره جهت تست
        result.Parts["PREFIX"] = prefix;
        result.Parts["TYPE"] = type;
        result.Parts["YEARTERM"] = yearTerm;
        result.Parts["CODE"] = code;
        result.Parts["SERIAL"] = serial;
        result.Parts["CHECKSUM"] = checksum;

        // 1) Prefix check
        if (prefix != "AU")
            result.AddError("Prefix must be AU.");

        // 2) TYPE
        if (!AllowedTypes.Contains(type))
            result.AddError("TYPE must be STU, MOD, or STF.");

        // 3) YearTerm → YEAR(4) + TERM(1)
        if (yearTerm.Length != 5 || !yearTerm.All(char.IsDigit))
        {
            result.AddError("YearTerm must be 5 digits (YYYYT).");
        }
        else
        {
            int year = int.Parse(yearTerm.Substring(0, 4));
            int term = int.Parse(yearTerm.Substring(4, 1));

            if (year < 1900 || year > 2100)
                result.AddError("Year out of valid range.");

            if (term < 1 || term > 3)
                result.AddError("Term must be 1–3.");
        }

        // 4) CODE → 3–6 letters
        if (!Regex.IsMatch(code, @"^[A-Z]{3,6}$"))
            result.AddError("CODE must be 3–6 uppercase letters.");

        // 5) SERIAL → 4 digits
        if (!Regex.IsMatch(serial, @"^\d{4}$"))
            result.AddError("Serial must be 4 digits.");

        // 6) CHECKSUM → 2 digits
        if (!Regex.IsMatch(checksum, @"^\d{2}$"))
            result.AddError("Checksum must be 2 digits.");

        // 7) Checksum logic
        if (result.Errors.Count == 0)
        {
            string clean = prefix + type + yearTerm + code + serial;
            int sum = 0;
            foreach (char ch in clean)
            {
                if (char.IsDigit(ch)) sum += ch - '0';
                else if (char.IsLetter(ch)) sum += (int)(ch - 'A') + 10;
            }
            int expected = int.Parse(checksum);
            if (sum % 97 != expected)
                result.AddError("Checksum mismatch.");
        }

        result.Success = result.Errors.Count == 0;
        return result;
    }
}
