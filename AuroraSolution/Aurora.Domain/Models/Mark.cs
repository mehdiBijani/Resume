using System;
using Aurora.Domain.Helpers;

namespace Aurora.Domain.Models;

public class Mark
{
    public Guid Id { get; } = Guid.NewGuid();
    public int StudentId { get; init; }
    public string ModuleCode { get; init; }
    public string AssessmentName { get; init; }
    public DateTime AssessmentAtUtc { get; init; }
    public double Score { get; init; }
    public bool IsResit { get; init; }

    public Mark(int studentId, string moduleCode, string assessmentName, DateTime atUtc, double score, bool isResit = false)
    {
        if (score < 0 || score > 100) throw new ValidationException("Score must be between 0 and 100");
        StudentId = studentId;
        ModuleCode = moduleCode;
        AssessmentName = assessmentName;
        AssessmentAtUtc = atUtc;
        Score = score;
        IsResit = isResit;
    }
}