1. Id types: Student.Id is int for deterministic testing.
2. TimeSlot comparisons are weekly slots; RegisteredWeeklySlots are used for clash detection.
3. University code parsing: accepts either 7-part explicit form AU-TYPE-YEAR-TERM-CODE-NNNN-CC or 6-part compact AU-TYPE-YEARTERM-CODE-NNNN-CC where YEARTERM is YYYYt.
4. Checksum interpretation: letters A-Z map to 10..35; digits map to their numeric value; we sum those mapped numbers and take result mod 97.
5. Term rounding uses MidpointRounding.AwayFromZero.
6. Modules with no assessment items are ignored in average calculation.
7. We require assessment item weights within a module to sum to 1.0, otherwise throw an error.
8. Year range accepted: 1900..(currentYear+1).
9. Seminar allocation uses "limited-first" heuristic and picks the group with the most free space to be fair and deterministic.
