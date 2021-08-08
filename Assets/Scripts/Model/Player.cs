using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int FinalScore;
    public List<int> treasures;
    public int numSkullsFilled;
    public int numIslandSpacesFilled;

    public Player()
    {
        treasures = new List<int>();
        numSkullsFilled = 0;
        numIslandSpacesFilled = 0;
        FinalScore = 0;
    }
}
