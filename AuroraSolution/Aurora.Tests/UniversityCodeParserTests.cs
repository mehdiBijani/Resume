using System;
using Aurora.Domain.Services;
using Xunit;

public class UniversityCodeParserTests
{
    [Fact]
    public void ValidCode_Passes()
    {
        var parser = new UniversityCodeParser();
        // Build a simple valid code with checksum computed by the parser logic
        var type = "STU";
        var year = DateTime.UtcNow.Year.ToString("D4");
        var term = "1";
        var code = "ABC";
        var serial = "0001";
        var prefix = "AU";
        var yearTerm = year + term;

        var clean = prefix + type + yearTerm + code + serial;

        long sum = 0;
        foreach (var ch in clean)
        {
            if (char.IsDigit(ch)) sum += ch - '0';
            else sum += 10 + (ch - 'A');
        }

        var cc = (int)(sum % 97);

        var id = $"AU-{type}-{yearTerm}-{code}-{serial}-{cc:00}";

        var r = parser.Parse(id);
        Assert.True(r.Success);
        Assert.NotNull(r.Parts);
        Assert.Equal(type, r.Parts!.Type);
    }

    [Fact]
    public void BadChecksum_Fails()
    {
        var parser = new UniversityCodeParser();
        var id = $"AU-STU-20251-ABC-0001-99"; // wrong cc
        var r = parser.Parse(id);
        Assert.False(r.Success);
        Assert.Contains(r.Errors, e => e.Contains("Checksum"));
    }

    [Fact]
    public void BadType_Fails()
    {
        var parser = new UniversityCodeParser();
        var id = $"AU-XYZ-20251-ABC-0001-00";
        var r = parser.Parse(id);
        Assert.False(r.Success);
        Assert.Contains(r.Errors, e => e.Contains("TYPE"));
    }
}