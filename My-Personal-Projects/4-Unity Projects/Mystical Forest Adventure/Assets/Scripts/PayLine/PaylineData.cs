using System.Collections.Generic;
using UnityEngine;

public static class PaylineData
{
    public static List<int[]> paylines = new List<int[]>()
    {
        new int[] {0, 0, 0, 0, 0}, // Line 1 - Top row
        new int[] {1, 1, 1, 1, 1}, // Line 2 - Middle row
        new int[] {2, 2, 2, 2, 2}, // Line 3 - Bottom row
        new int[] {0, 1, 2, 1, 0}, // Line 4 - V pattern
        new int[] {2, 1, 0, 1, 2}, // Line 5 - Reverse V
        new int[] {0, 0, 1, 0, 0}, // Line 6
        new int[] {2, 2, 1, 2, 2}, // Line 7
        new int[] {1, 0, 0, 0, 1}, // Line 8
        new int[] {1, 2, 2, 2, 1}, // Line 9
        new int[] {0, 1, 1, 1, 2}, // Line 10
        new int[] {2, 1, 1, 1, 0}, // Line 11
        new int[] {1, 0, 1, 2, 1}, // Line 12
        new int[] {1, 2, 1, 0, 1}, // Line 13
        new int[] {0, 1, 0, 1, 0}, // Line 14
        new int[] {2, 1, 2, 1, 2}, // Line 15
        new int[] {1, 1, 0, 1, 1}, // Line 16
        new int[] {1, 1, 2, 1, 1}, // Line 17
        new int[] {0, 2, 0, 2, 0}, // Line 18
        new int[] {2, 0, 2, 0, 2}, // Line 19
        new int[] {1, 0, 2, 0, 1}  // Line 20
    };
}
