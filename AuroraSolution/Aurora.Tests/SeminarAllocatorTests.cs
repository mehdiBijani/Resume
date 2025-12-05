using System;
using System.Collections.Generic;
using Aurora.Domain.Models;
using Aurora.Domain.Services;
using Xunit;

public class SeminarAllocatorTests
{
    [Fact]
    public void StudentWithNoClashGetsAssigned()
    {
        var module = new Module("CS101", "Intro", 30, 10);
        var g1 = new SeminarGroup("G1", 2, new TimeSlot(DayOfWeek.Monday, TimeSpan.FromHours(9), TimeSpan.FromHours(10)));
        module.SeminarGroups.Add(g1);

        var s = new Student(1, "S1");
        var allocator = new SeminarAllocator();

        var res = allocator.Allocate(module, new List<Student> { s });
        Assert.Contains(1, res.Assignments["G1"]);
        Assert.Empty(res.Unplaced);
    }

    [Fact]
    public void StudentWithClashIsUnplaced()
    {
        var module = new Module("CS101", "Intro", 30, 10);
        var g1 = new SeminarGroup("G1", 2, new TimeSlot(DayOfWeek.Monday, TimeSpan.FromHours(9), TimeSpan.FromHours(10)));
        module.SeminarGroups.Add(g1);

        var s = new Student(1, "S1");
        s.RegisteredWeeklySlots.Add(new TimeSlot(DayOfWeek.Monday, new TimeSpan(9,30,0), new TimeSpan(10,30,0)));

        var allocator = new SeminarAllocator();
        var res = allocator.Allocate(module, new List<Student> { s });
        Assert.Empty(res.Assignments["G1"]);
        Assert.Contains(1, res.Unplaced);
    }

    [Fact]
    public void CapacityLimitsCauseUnplaced()
    {
        var module = new Module("CS101", "Intro", 30, 10);
        var g1 = new SeminarGroup("G1", 1, new TimeSlot(DayOfWeek.Monday, TimeSpan.FromHours(9), TimeSpan.FromHours(10)));
        module.SeminarGroups.Add(g1);

        var s1 = new Student(1, "S1");
        var s2 = new Student(2, "S2");

        var allocator = new SeminarAllocator();
        var res = allocator.Allocate(module, new List<Student> { s1, s2 });

        Assert.Equal(1, res.Assignments["G1"].Count);
        Assert.Single(res.Unplaced);
    }
}