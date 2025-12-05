using System.Collections.Generic;
using System.Linq;
using Aurora.Domain.Models;

namespace Aurora.Domain.Services;

public record AllocationResult(Dictionary<string, List<int>> Assignments, List<int> Unplaced);

public class SeminarAllocator
{
    /// <summary>
    /// Allocate students into seminar groups of a module.
    /// Returns mapping groupId -> studentIds and list of unplaced studentIds.
    /// Heuristic: students with fewest possible groups are allocated first; among groups choose one with most free space.
    /// </summary>
    public AllocationResult Allocate(Module module, List<Student> students)
    {
        var assignments = module.SeminarGroups.ToDictionary(g => g.Id, g => new List<int>());
        var unplaced = new List<int>();

        // Precompute compatible groups per student
        var studentOptions = new Dictionary<int, List<SeminarGroup>>();

        foreach (var s in students.OrderBy(s => s.Id))
        {
            var options = module.SeminarGroups
                .Where(g => !s.RegisteredWeeklySlots.Any(slot => slot.Overlaps(g.WeeklySlot)))
                .ToList();
            studentOptions[s.Id] = options;
        }

        // Order students by increasing number of options (limited-first), deterministic tie by Id
        var order = studentOptions.OrderBy(kv => kv.Value.Count).ThenBy(kv => kv.Key).Select(kv => kv.Key).ToList();

        foreach (var studentId in order)
        {
            var options = studentOptions[studentId];
            if (!options.Any())
            {
                unplaced.Add(studentId);
                continue;
            }

            // choose group with most free space, tie-break by Id for determinism
            var chosen = options.OrderByDescending(g => g.FreeSpace).ThenBy(g => g.Id).FirstOrDefault(g => g.FreeSpace > 0);
            if (chosen is null)
            {
                unplaced.Add(studentId);
                continue;
            }

            chosen.AssignedStudentIds.Add(studentId);
            assignments[chosen.Id].Add(studentId);

            // reflect that student now has this weekly slot (so future allocations consider this)
            var studentObj = students.First(s => s.Id == studentId);
            studentObj.RegisteredWeeklySlots.Add(chosen.WeeklySlot);
        }

        return new AllocationResult(assignments, unplaced);
    }
}
