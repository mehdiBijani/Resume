using System.Collections.Generic;
using Aurora.Domain.Helpers;

namespace Aurora.Domain.Models;

public class Module
{
    public string Code { get; init; }
    public string Title { get; init; }
    public int Capacity { get; init; }
    public int Credit { get; init; }

    public List<SeminarGroup> SeminarGroups { get; } = new();

    public Module(string code, string title, int capacity, int credit)
    {
        if (string.IsNullOrWhiteSpace(code)) throw new ValidationException("Module code is required");
        if (capacity <= 0) throw new ValidationException("Module capacity must be positive");
        if (credit <= 0) throw new ValidationException("Module credit must be positive");
        Code = code;
        Title = title;
        Capacity = capacity;
        Credit = credit;
    }
}