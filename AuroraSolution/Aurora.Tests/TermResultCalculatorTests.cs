using System;
using System.Collections.Generic;
using Aurora.Domain.Services;
using Xunit;

public class TermResultCalculatorTests
{
    [Fact]
    public void ResitBetter_UsedInCalculation()
    {
        var calc = new TermResultCalculator();
        var module = new ModuleInput("M1", 10, new List<AssessmentItem>
        {
            new AssessmentItem("A1", new DateTime(2025,1,1,0,0,0, DateTimeKind.Utc), 1.0, new List<double>{ 30.0, 70.0 })
        });

        var res = calc.Calculate(new List<ModuleInput>{ module });
        Assert.Equal(70.0, res.Average);
        Assert.Equal("Pass", res.Decision);
    }

    [Fact]
    public void WeightedAverageAndDates()
    {
        var calc = new TermResultCalculator();
        var m1 = new ModuleInput("M1", 10, new List<AssessmentItem>
        {
            new AssessmentItem("A1", new DateTime(2025,1,1,0,0,0, DateTimeKind.Utc), 0.5, new List<double>{ 50 }),
            new AssessmentItem("A2", new DateTime(2025,1,2,0,0,0, DateTimeKind.Utc), 0.5, new List<double>{ 70 })
        });
        var m2 = new ModuleInput("M2", 20, new List<AssessmentItem>
        {
            new AssessmentItem("B1", new DateTime(2025,1,3,0,0,0, DateTimeKind.Utc), 1.0, new List<double>{ 40 })
        });

        var res = calc.Calculate(new List<ModuleInput>{ m1, m2 });
        // module1 mark = (50*0.5 + 70*0.5)=60; module2=40; credit-weighted average = (60*10 + 40*20)/30 = (600+800)/30=1400/30=46.666... => 46.7
        Assert.Equal(46.7, res.Average);
        Assert.Equal(new DateTime(2025,1,1,0,0,0, DateTimeKind.Utc), res.MinAssessmentUtc);
        Assert.Equal(new DateTime(2025,1,3,0,0,0, DateTimeKind.Utc), res.MaxAssessmentUtc);
    }

    [Fact]
    public void RoundingBoundary_AwayFromZero()
    {
        var calc = new TermResultCalculator();
        var m = new ModuleInput("M", 10, new List<AssessmentItem>
        {
            new AssessmentItem("A", new DateTime(2025,1,1, 0, 0, 0, DateTimeKind.Utc), 1.0, new List<double>{ 39.95 })
        });
        var res = calc.Calculate(new List<ModuleInput>{ m });
        // 39.95 rounded away from zero => 40.0
        Assert.Equal(40.0, res.Average);
    }
}
