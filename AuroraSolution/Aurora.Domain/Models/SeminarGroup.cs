using System.Collections.Generic;
using Aurora.Domain.Helpers;

namespace Aurora.Domain.Models;

public class SeminarGroup
{
    public string Id { get; init; }
    public int Capacity { get; init; }
    public TimeSlot WeeklySlot { get; init; }
    public List<int> AssignedStudentIds { get; } = new();

    public SeminarGroup(string id, int capacity, TimeSlot weeklySlot)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new ValidationException("SeminarGroup id required");
        if (capacity <= 0) throw new ValidationException("SeminarGroup capacity must be positive");
        Id = id;
        Capacity = capacity;
        WeeklySlot = weeklySlot;
    }

    public int FreeSpace => Capacity - AssignedStudentIds.Count;
}