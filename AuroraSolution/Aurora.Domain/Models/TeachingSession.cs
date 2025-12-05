using System;
using Aurora.Domain.Helpers;

namespace Aurora.Domain.Models;

public class TeachingSession
{
    public Guid Id { get; } = Guid.NewGuid();
    public string ModuleCode { get; init; }
    public DateTime StartUtc { get; init; }
    public DateTime EndUtc { get; init; }
    public string? LocationOrLink { get; init; }

    public TeachingSession(string moduleCode, DateTime startUtc, DateTime endUtc, string? locationOrLink = null)
    {
        if (endUtc <= startUtc) throw new ValidationException("TeachingSession EndUtc must be after StartUtc");
        ModuleCode = moduleCode;
        StartUtc = startUtc;
        EndUtc = endUtc;
        LocationOrLink = locationOrLink;
    }
}