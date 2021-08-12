using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureHelper
{
    // find treasures with the just filled boardspace
    public static FoundTreasure FindTreasuresForNumberedSpace(BoardSpace boardSpace)
    {
        BoardSpace[,] BoardGrid = GameHandler.Instance.BoardGrid;
        int row = boardSpace.row;
        int col = boardSpace.column;
        int spaceNum = boardSpace.Number;

        // don't search if treasure was already claimed
        if(TreasureFoundAlready(spaceNum))
        {
            return null;
        }

        // UP
        if (row - 2 >= 0 && col > 0 && col < Constants.GRID_COL - 1)
        {
            //Debug.Log("Going UP");
            for (int i = row - 2; i >= 0; i--)
            {
                BoardSpace currSpace = BoardGrid[i, col];
                // if matching number/ship found
                if (spaceNum == currSpace.Number || currSpace.hasShip)
                {
                    //Debug.Log("Matching Num/Ship found up");
                    // start searching for row with same nums
                    for (int j = row - 1; j > currSpace.row; j--)
                    {
                        FoundTreasure currTreasure = searchForRowWithSameNum(col, spaceNum, j, BoardGrid);
                        if(null != currTreasure)
                        {
                            return currTreasure;
                        }
                    }
                }
            }
        }

        // DOWN
        if (row + 2 <= Constants.GRID_ROW - 1 && col > 0 && col < Constants.GRID_COL - 1)
        {
            //Debug.Log("Going DOWN");
            for (int i = row + 2; i < Constants.GRID_ROW; i++)
            {
                BoardSpace currSpace = BoardGrid[i, col];
                // if matching number/ship found
                if (spaceNum == currSpace.Number || currSpace.hasShip)
                {
                    //Debug.Log("Matching Num/Ship found up");
                    // start searching for row with same nums
                    for (int j = row + 1; j < currSpace.row; j++)
                    {
                        FoundTreasure currTreasure = searchForRowWithSameNum(col, spaceNum, j, BoardGrid);
                        if (null != currTreasure)
                        {
                            return currTreasure;
                        }
                    }
                }
            }
        }

        // LEFT
        if (col - 2 >= 0 && row > 0 && row < Constants.GRID_ROW - 1)
        {
            //Debug.Log("Going LEFT");
            for (int i = col - 2; i >= 0; i--)
            {
                BoardSpace currSpace = BoardGrid[row, i];
                // if matching number/ship found
                if (spaceNum == currSpace.Number || currSpace.hasShip)
                {
                    //Debug.Log("Matching Num/Ship found left");
                    // start searching for col with same nums
                    for (int j = col - 1; j > currSpace.column; j--)
                    {
                        FoundTreasure currTreasure = searchForColWithSameNum(row, spaceNum, j, BoardGrid);
                        if (null != currTreasure)
                        {
                            return currTreasure;
                        }
                    }
                }
            }
        }

        // RIGHT
        if (col + 2 <= Constants.GRID_COL - 1 && row > 0 && row < Constants.GRID_ROW - 1)
        {
            //Debug.Log("Going DOWN");
            for (int i = col + 2; i < Constants.GRID_COL; i++)
            {
                BoardSpace currSpace = BoardGrid[row, i];
                // if matching number/ship found
                if (spaceNum == currSpace.Number || currSpace.hasShip)
                {
                    //Debug.Log("Matching Num/Ship found up");
                    // start searching for row with same nums
                    for (int j = col + 1; j < currSpace.column; j++)
                    {
                        FoundTreasure currTreasure = searchForColWithSameNum(row, spaceNum, j, BoardGrid);
                        if (null != currTreasure)
                        {
                            return currTreasure;
                        }
                    }
                }
            }
        }

        return null;

    }

    private static FoundTreasure searchForRowWithSameNum(int col, int spaceNum, int currRow, BoardSpace[,] BoardGrid)
    {
        //Debug.Log("Start searching for row with same nums");
        bool shipUsed = false;
        bool leftSideMatchFound = false;
        // left side
        for (int k = 0; k < col; k++)
        {
            //Debug.Log("Left Side, row: " + currRow + ", col: " + k);
            BoardSpace currInnerSpace = BoardGrid[currRow, k];
            if (spaceNum == currInnerSpace.Number || currInnerSpace.hasShip)
            {
                //Debug.Log("Matching Num/Ship found on the left on row: " + currRow + ", col: " + k);
                if (currInnerSpace.hasShip) { shipUsed = true; }
                leftSideMatchFound = true;
            }
        }

        if (leftSideMatchFound)
        {
            // right side
            for (int k = col + 1; k < Constants.GRID_COL; k++)
            {
                //Debug.Log("Right Side, row: " + currRow + ", col: " + k);
                BoardSpace currInnerSpace = BoardGrid[currRow, k];
                if (spaceNum == currInnerSpace.Number || currInnerSpace.hasShip)
                {
                    //Debug.Log("Matching Num/Ship found on the right on row: " + currRow + ", col: " + k);
                    if (currInnerSpace.hasShip)
                    {
                        if (shipUsed)
                        {
                            continue;
                        }
                    }

                    return new FoundTreasure(currRow, col, spaceNum);
                }
            }
        }

        return null;
    }

    private static FoundTreasure searchForColWithSameNum(int row, int spaceNum, int currCol, BoardSpace[,] BoardGrid)
    {
        //Debug.Log("Start searching for row with same nums");
        bool shipUsed = false;
        bool upSideMatchFound = false;
        // up side
        for (int k = 0; k < row; k++)
        {
            //Debug.Log("Up Side, row: " + k + ", col: " + currCol);
            BoardSpace currInnerSpace = BoardGrid[k, currCol];
            if (spaceNum == currInnerSpace.Number || currInnerSpace.hasShip)
            {
                //Debug.Log("Matching Num/Ship found on the up on row: " + k + ", col: " + currCol);
                if (currInnerSpace.hasShip) { shipUsed = true; }
                upSideMatchFound = true;
            }
        }

        if (upSideMatchFound)
        {
            // down side
            for (int k = row + 1; k < Constants.GRID_ROW; k++)
            {
                //Debug.Log("Down Side, row: " + k + ", col: " + currCol);
                BoardSpace currInnerSpace = BoardGrid[k, currCol];
                if (spaceNum == currInnerSpace.Number || currInnerSpace.hasShip)
                {
                    //Debug.Log("Matching Num/Ship found on the down on row: " + k + ", col: " + currCol);
                    if (currInnerSpace.hasShip)
                    {
                        if (shipUsed)
                        {
                            continue;
                        }
                    }

                    return new FoundTreasure(row, currCol, spaceNum);
                }
            }
        }

        return null;
    }

    public static FoundTreasure FindTreasuresForSpaceWithShip(BoardSpace boardSpaceWithShip)
    {
        BoardSpace[,] BoardGrid = GameHandler.Instance.BoardGrid;
        int row = boardSpaceWithShip.row;
        int col = boardSpaceWithShip.column;

        // UP
        if (row - 2 >= 0 && col > 0 && col < Constants.GRID_COL - 1)
        {
            //Debug.Log("Going UP");
            for (int i = row - 2; i >= 0; i--)
            {
                BoardSpace currSpace = BoardGrid[i, col];

                FoundTreasure currTreasure = searchForSpaceToMatchWithShip(currSpace);
                if (null != currTreasure)
                {
                    return currTreasure;
                }
            }
        }

        // DOWN
        if (row + 2 <= Constants.GRID_ROW - 1 && col > 0 && col < Constants.GRID_COL - 1)
        {
            //Debug.Log("Going DOWN");
            for (int i = row + 2; i < Constants.GRID_ROW; i++)
            {
                BoardSpace currSpace = BoardGrid[i, col];

                FoundTreasure currTreasure = searchForSpaceToMatchWithShip(currSpace);
                if (null != currTreasure)
                {
                    return currTreasure;
                }
            }
        }

        // LEFT
        if (col - 2 >= 0 && row > 0 && row < Constants.GRID_ROW - 1)
        {
            //Debug.Log("Going LEFT");
            for (int i = col - 2; i >= 0; i--)
            {
                BoardSpace currSpace = BoardGrid[row, i];

                FoundTreasure currTreasure = searchForSpaceToMatchWithShip(currSpace);
                if (null != currTreasure)
                {
                    return currTreasure;
                }
            }
        }

        // RIGHT
        if (col + 2 <= Constants.GRID_COL - 1 && row > 0 && row < Constants.GRID_ROW - 1)
        {
            //Debug.Log("Going DOWN");
            for (int i = col + 2; i < Constants.GRID_COL; i++)
            {
                BoardSpace currSpace = BoardGrid[row, i];

                FoundTreasure currTreasure = searchForSpaceToMatchWithShip(currSpace);
                if (null != currTreasure)
                {
                    return currTreasure;
                }
            }
        }

        return null;
    }

    private static FoundTreasure searchForSpaceToMatchWithShip(BoardSpace currSpace)
    {
        if (currSpace.isOccupied && !currSpace.hasShip && !currSpace.hasSkull && !TreasureFoundAlready(currSpace.Number))
        {
            return FindTreasuresForNumberedSpace(currSpace);
        }

        return null;
    }

    private static bool TreasureFoundAlready(int num)
    {
        return GameHandler.Instance.player.treasures.Contains(num);
    }
}
