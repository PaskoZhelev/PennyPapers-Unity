using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public HashSet<int> treasures;
    public int numSkullsFilled;
    public int numIslandSpacesFilled;

    // end game vars
    public int treasureTotalPoints;
    public List<int> skullPoints;
    public int skullTotalPoints;
    public int FinalScore;

    public Player()
    {
        treasures = new HashSet<int>();
        skullPoints = new List<int>();
        numSkullsFilled = 0;
        numIslandSpacesFilled = 0;
        FinalScore = 0;
        treasureTotalPoints = 0;
        skullTotalPoints = 0;
    }

    public void AddTreasure(int num)
    {
        treasures.Add(num);
    }
}
