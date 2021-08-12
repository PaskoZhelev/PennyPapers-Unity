using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundTreasure
{
    public int row;
    public int column;
    public int Number;

    public FoundTreasure(int row, int column, int Number)
    {
        this.row = row;
        this.column = column;
        this.Number = Number;
    }
}
