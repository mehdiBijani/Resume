using System.Collections.Generic;
using Aurora.Domain.Helpers;

namespace Aurora.Domain.Models;

public class Student
{
    public int Id { get; init; }
    public string FullName { get; init; }
    public string? Email { get; init; }

    // Weekly registered sessions expressed as TimeSlots (for clash checking)
    public List<TimeSlot> RegisteredWeeklySlots { get; } = new();

    // Marks for the student (optional place to store)
    public List<Mark> Marks { get; } = new();

    public Student(int id, string fullName, string? email = null)
    {
        if (string.IsNullOrWhiteSpace(fullName)) throw new ValidationException("Student FullName is required");
        Id = id;
        FullName = fullName;
        Email = email;
    }
}