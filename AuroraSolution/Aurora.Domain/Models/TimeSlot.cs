namespace Aurora.Domain.Models;

public class TimeSlot
{
    public DayOfWeek Day { get; }
    public TimeSpan Start { get; }
    public TimeSpan End { get; }

    public TimeSlot(DayOfWeek day, TimeSpan start, TimeSpan end)
    {
        if (end <= start)
            throw new ArgumentException("End time must be after start time.");

        Day = day;
        Start = start;
        End = end;
    }

    public bool Overlaps(TimeSlot other)
    {
        if (Day != other.Day)
            return false;

        return Start < other.End && other.Start < End;
    }
}