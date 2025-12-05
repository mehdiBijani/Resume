using System;
using Aurora.Domain.Models;
using Aurora.Domain.Helpers;
using Xunit;

public class ModelValidationTests
{
    [Fact]
    public void MarkScoreOutOfRange_Throws()
    {
        Assert.Throws<ValidationException>(() => new Mark(1, "M", "A", DateTime.UtcNow, 150));
    }

    [Fact]
    public void TimeSlotInvalid_Throws()
    {
        Assert.Throws<ArgumentException>(() => new TimeSlot(DayOfWeek.Monday, TimeSpan.FromHours(10), TimeSpan.FromHours(9)));
    }
}