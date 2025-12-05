using System;
using System.Collections.Generic;
using System.Linq;
using Aurora.Domain.Models;

namespace Aurora.Domain.Services;

public record AssessmentItem(string Name, DateTime DateUtc, double Weight, List<double> Attempts);
public record ModuleInput(string ModuleCode, int Credit, List<AssessmentItem> Items);

public record TermResult(double Average, DateTime MinAssessmentUtc, DateTime MaxAssessmentUtc, string Decision);

public class TermResultCalculator
{
    /// <summary>
    /// Calculate term result:
    /// - For each assessment use best attempt (resit if better)
    /// - Module mark = weighted average of items (weights must sum to 1.0)
    /// - Term average = credit-weighted average of module marks
    /// - Return rounded average (1 decimal, AwayFromZero), date range (min/max UTC), decision Pass if >=40.0 else Refer
    /// </summary>
    public TermResult Calculate(List<ModuleInput> modules)
    {
        if (modules == null || !modules.Any()) throw new ArgumentException("No modules provided");

        DateTime? min = null;
        DateTime? max = null;

        double weightedSum = 0.0;
        double totalCredits = 0.0;

        foreach (var m in modules)
        {
            if (m.Items == null || !m.Items.Any()) continue; // ignore modules with no assessments

            var weightSum = m.Items.Sum(i => i.Weight);
            if (Math.Abs(weightSum - 1.0) > 1e-6) throw new ArgumentException($"Weights for module {m.ModuleCode} must sum to 1.0");

            double moduleMark = 0.0;
            foreach (var it in m.Items)
            {
                if (it.Attempts == null || !it.Attempts.Any()) throw new ArgumentException($"Assessment {it.Name} in module {m.ModuleCode} has no attempts");
                var best = it.Attempts.Max();
                moduleMark += best * it.Weight;

                if (!min.HasValue || it.DateUtc < min) min = it.DateUtc;
                if (!max.HasValue || it.DateUtc > max) max = it.DateUtc;
            }

            weightedSum += moduleMark * m.Credit;
            totalCredits += m.Credit;
        }

        if (totalCredits <= 0) throw new ArgumentException("Total credits must be positive");

        var averageRaw = weightedSum / totalCredits;
        var averageRounded = Math.Round(averageRaw, 1, MidpointRounding.AwayFromZero);

        var decision = averageRounded >= 40.0 ? "Pass" : "Refer";

        return new TermResult(averageRounded, min!.Value, max!.Value, decision);
    }
}
