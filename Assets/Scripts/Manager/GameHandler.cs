using BayatGames.SaveGameFree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public BoardSpace[] allBoardSpaces;
    public BoardSpace[] initialIslandBoardSpaces;

    public BoardSpace[] tortugaAllBoardSpaces;
    public BoardSpace[] tortugaInitialIslandBoardSpaces;
    public BoardSpace[] maracaoAllBoardSpaces;
    public BoardSpace[] maracaoInitialIslandBoardSpaces;

    public BoardSpace[,] BoardGrid = new BoardSpace[Constants.GRID_ROW, Constants.GRID_COL];

    [HideInInspector]
    public List<BoardSpace> seaBoardSpaces;
    [HideInInspector]
    public List<BoardSpace> islandBoardSpaces;
    [HideInInspector]
    public HashSet<BoardSpace> possibleAdjacentBoardSpaces;
    [HideInInspector]
    public List<BoardSpace> spacesWithSkull;

    [HideInInspector]
    public BoardSpace lastFilledSpace;
    [HideInInspector]
    public FillType currentFillType;

    [HideInInspector]
    public bool isFirstTurn;
    [HideInInspector]
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        UIHandler.Instance.ShowLoadingPanel();
        if (Constants.SELECTED_MAP == Constants.TORTUGA_MAP)
        {
            allBoardSpaces = tortugaAllBoardSpaces;
            initialIslandBoardSpaces = tortugaInitialIslandBoardSpaces;
            UIHandler.Instance.SetupTortugaBoard();
        } else
        {
            allBoardSpaces = maracaoAllBoardSpaces;
            initialIslandBoardSpaces = maracaoInitialIslandBoardSpaces;
            UIHandler.Instance.SetupMaracaoBoard();
        }
        
        player = new Player(Constants.SELECTED_MAP);
        isFirstTurn = true;
        seaBoardSpaces = identifySeaBoardSpaces();
        islandBoardSpaces = identifyIslandBoardSpaces();
        possibleAdjacentBoardSpaces = new HashSet<BoardSpace>();
        spacesWithSkull = new List<BoardSpace>();
        GenerateBoardGrid();

        StartCoroutine(StartGame());
    }

    public IEnumerator StartGame()
    {
        yield return new WaitForSeconds(Constants.STARTING_GAME_DELAY);
        UIHandler.Instance.HideLoadingPanel();
        yield return new WaitForSeconds(Constants.NEW_TURN_DELAY);
        StartNewTurn();
    }

    public void StartNewTurn()
    {
        // Game End
        if (isGameFinished())
        {
            Debug.Log("Game Finished");
            TriggerEndGame();
            return;
        }

        DiceManager.Instance.RollDice();

        StartCoroutine(ShowPossiblePlayActions());
    }

    public IEnumerator StartNewTurnWithDelay()
    {
        yield return new WaitForSeconds(Constants.NEW_TURN_DELAY);
        StartNewTurn();
    }

    private IEnumerator ShowPossiblePlayActions()
    {
        yield return new WaitForSeconds(Constants.DICE_ROLLING_COMPENSATION_TIME);
        ShowNecessaryActions();
    }

    public void ShowNecessaryActions()
    {
        // if there are possible selection numbers 
        // then no special die was rolled
        if (DiceManager.Instance.currentPossibleSelectionNumbers.Count > 0)
        {
            currentFillType = FillType.NUMBER;
            UIHandler.Instance.setGeneralSelectableNumbers();
            return;
        }

        // RED
        if (DiceManager.Instance.isRedSpecialDieRolled())
        {
            currentFillType = FillType.SKULL;
            UIHandler.Instance.ShowRedSelectionPanel();
            EnablePossibleAdjacentToLastFilled();
            return;
        }

        // PURPLE
        if (DiceManager.Instance.isPurpleSpecialDieRolled() && !DiceManager.Instance.isGreenSpecialDieRolled())
        {
            currentFillType = FillType.NUMBER;
            UIHandler.Instance.setPurpleSelectableNumbers();
            UIHandler.Instance.ShowPurpleSelectionPanel();
            return;
        }

        // GREEN
        if (DiceManager.Instance.isGreenSpecialDieRolled() && !DiceManager.Instance.isPurpleSpecialDieRolled())
        {
            currentFillType = FillType.SHIP;
            UIHandler.Instance.ShowGreenYesNoSelectionPanel();
            return;
        }

        // GREEN and PURPLE
        if (DiceManager.Instance.isGreenSpecialDieRolled() && DiceManager.Instance.isPurpleSpecialDieRolled())
        {
            UIHandler.Instance.ShowGreenPurpleSelectionPanel();
            return;
        }

        //Max skulls already filled so Red die is ignored
        DiceManager.Instance.findPossibleSelectionNumbersIgnoringSpecial();
        ShowNecessaryActions();
    }

    // Check for treasure with the just filled space
    public void CheckForTreasure(BoardSpace filledSpace)
    {
        FoundTreasure foundTreasure = filledSpace.hasShip ? TreasureHelper.FindTreasuresForSpaceWithShip(filledSpace) : TreasureHelper.FindTreasuresForNumberedSpace(filledSpace);
        if (null != foundTreasure)
        {
            BoardSpace spaceWithTreasure = BoardGrid[foundTreasure.row, foundTreasure.column];
            // treasure can't be in the sea
            if(spaceWithTreasure.isSea || spaceWithTreasure.hasTreasure)
            {
                return;
            }

            Debug.Log("Treasure Found in row: " + foundTreasure.row + ", col: " + foundTreasure.column + ", Num: " + foundTreasure.Number);
            spaceWithTreasure.ActivateTreasureCircle(foundTreasure.Number);
            player.AddTreasure(foundTreasure.Number);
            UIHandler.Instance.ShowTreasureNumber(foundTreasure.Number, Constants.NUMBER_COLORS[foundTreasure.Number - 1]);
            return;
        }
    }

    // space is clicked
    public void SpaceClicked()
    {
        switch (currentFillType)
        {
            case FillType.NUMBER:
                lastFilledSpace.PutNumber(UIHandler.Instance.NumberToPlace);
                break;
            case FillType.SKULL:
                lastFilledSpace.PutSkull();
                UIHandler.Instance.ShowSkullMark();
                break;
            // Ship
            default:
                lastFilledSpace.PutShip();
                break;
        }

        DisableAllSpaces();

        // TODO Undo functionality disabled for now
        // Turn finished is called immediately
        if (Constants.UNDO_FUNCTIONALITY_ENABLED)
        {
            UIHandler.Instance.StartTimer();
        } else
        {
            GameHandler.Instance.TurnFinished();
        }
    }

    public void TurnFinished()
    {
        if (isFirstTurn) { isFirstTurn = false; }
        possibleAdjacentBoardSpaces.Remove(lastFilledSpace);
        UIHandler.Instance.resetGeneralSelectableNumbers();
        UIHandler.Instance.HideAllSelectionPanels();
        StartCoroutine(StartNewTurnWithDelay());
        //StartNewTurn();
    }

    public void EnablePossibleIslandSpaces()
    {
        if(isFirstTurn)
        {
            EnableInitialIslandSpaces();
            return;
        }

        // enable only adjacent to number or ship spaces
        if(Constants.ADJACENCY_RULE_ENABLED && possibleAdjacentBoardSpaces.Count > 0)
        {
            foreach (BoardSpace space in possibleAdjacentBoardSpaces)
            {
                if (!space.isOccupied)
                {
                    space.EnableSpace();
                }
            }

            return;
        } 

        // only if rule is not enabled
        // or there are no possible spaces
        EnableAllUnoccuppiedIslandSpaces();
    }

    public void EnableAllUnoccuppiedIslandSpaces()
    {
        foreach (BoardSpace space in islandBoardSpaces)
        {
            if (!space.isOccupied)
            {
                space.EnableSpace();
            }
        }
    }

    public void EnableInitialIslandSpaces()
    {
        foreach (BoardSpace space in initialIslandBoardSpaces)
        {
            space.EnableSpace();
        }
    }

    public void EnablePossibleAdjacentToLastFilled()
    {
        if (null == lastFilledSpace)
        {
            EnablePossibleIslandSpaces();
            return;
        }

        int x = lastFilledSpace.row;
        int y = lastFilledSpace.column;

        HashSet<BoardSpace> spaces = findAvailableAdjacentSpaces(x, y);

        if(spaces.Count == 0)
        {
            EnablePossibleIslandSpaces();
            return;
        }

        foreach(BoardSpace space in spaces)
        {
            if (!space.isOccupied)
            {
                space.EnableSpace();
            }
        }
    }

    public void AddPossibleAdjacentIslandSpaces(int x, int y)
    {
        HashSet<BoardSpace> adjacentSpaces = findAvailableAdjacentSpaces(x, y);
        possibleAdjacentBoardSpaces.UnionWith(adjacentSpaces);
    }

    public void EnableSeaSpaces()
    {
        foreach (BoardSpace space in seaBoardSpaces)
        {
            if (!space.isOccupied)
            {
                space.EnableSpace();
            }
        }
    }

    public void DisableAllSpaces()
    {
        foreach(BoardSpace space in islandBoardSpaces)
        {
            space.DisableSpace();
        }

        foreach(BoardSpace space in seaBoardSpaces)
        {
            space.DisableSpace();
        }
    }

    private bool isGameFinished()
    {
        if (player.treasures.Count == Constants.MAX_TREASURES || player.numIslandSpacesFilled == Constants.SELECTED_MAP_MAX_ISLAND_SPACES)
        {
            return true;
        }

        return false;
    }

    public void TriggerEndGame()
    {
        player.skullPoints = FindSkullPoints();
        player.treasureTotalPoints = new List<int>(player.treasures).Sum();
        player.skullTotalPoints = player.skullPoints.Sum();
        player.FinalScore = player.treasureTotalPoints + player.skullTotalPoints;
        SaveScoreData();
        UIHandler.Instance.ShowEndGamePanel();
    }

    private void SaveScoreData()
    {
        Debug.Log("Saving Data");
        List<int> existingScores = SaveGame.Load<List<int>>(player.map, true, SaveGame.EncodePassword);
        if (null != existingScores)
        {
            existingScores.Add(player.FinalScore);
        }
        else
        {
            existingScores = new List<int>();
            existingScores.Add(player.FinalScore);
        }

        SaveGame.Save<List<int>>(player.map, existingScores, true);
    }

    public List<int> FindSkullPoints()
    {
        List<int> skullPoints = new List<int>();
        foreach (BoardSpace space in spacesWithSkull)
        {
            HashSet<BoardSpace> adjacentSpaces = findAllAdjacentSpaces(space.row, space.column);
            int smallestNum = findSmallestNumberFromSpaces(adjacentSpaces);
            // skull is overcome
            if (space.cross.activeInHierarchy)
            {
                skullPoints.Add(smallestNum);
            }
            else
            {
                smallestNum = smallestNum == 0 ? 0 : (-1) * smallestNum;
                skullPoints.Add(smallestNum);
                
                if(space.hasTreasure)
                {
                    player.treasures.Remove(space.treasureValue);
                    UIHandler.Instance.HideTreasureNumber(space.treasureValue);
                }
            }
        }

        return skullPoints;
    }

    private int findSmallestNumberFromSpaces(HashSet<BoardSpace> spaces)
    {
        List<int> filteredList = spaces.Select(sp => sp.Number).Where(x => x > 0).ToList();
        if(filteredList.Count > 0)
        {
            return filteredList.Min();
        }
        return 0;
    }

    public void OvercomeSkullWhenPossible(BoardSpace filledSpace)
    {
        if (filledSpace.hasSkull)
        {
            HashSet<BoardSpace> adjacentSpaces = findAllAdjacentSpaces(filledSpace.row, filledSpace.column);

            foreach(BoardSpace space in adjacentSpaces)
            {
                if (space.Number == Constants.OVERCOME_SKULL_NUMBER)
                {
                    filledSpace.ActivateCross();
                    return;
                }
            }
        } else if(filledSpace.Number == Constants.OVERCOME_SKULL_NUMBER)
        {
            HashSet<BoardSpace> adjacentSpaces = findAllAdjacentSpaces(filledSpace.row, filledSpace.column);

            foreach (BoardSpace space in adjacentSpaces)
            {
                if (space.hasSkull)
                {
                    space.ActivateCross();
                }
            }
        }
    }

    public HashSet<BoardSpace> findAllAdjacentSpaces(int x, int y)
    {
        HashSet<BoardSpace> spaces = new HashSet<BoardSpace>();
        for (int dx = (x > 0 ? -1 : 0); dx <= (x < Constants.GRID_ROW ? 1 : 0); ++dx)
        {
            for (int dy = (y > 0 ? -1 : 0); dy <= (y < Constants.GRID_COL ? 1 : 0); ++dy)
            {
                if (dx != 0 || dy != 0)
                {
                    //Debug.Log("x:" + x + ",y:" + y + ",dx:" + dx + ",dy:" + dy);
                    int currRow = (x + dx) >= Constants.GRID_ROW ? Constants.GRID_ROW - 1 : x + dx;
                    int currCol = (y + dy) >= Constants.GRID_COL ? Constants.GRID_COL - 1 : y + dy;
                    BoardSpace currSpace = BoardGrid[currRow, currCol];
                    if (!currSpace.isSea)
                    {
                        spaces.Add(currSpace);
                    }
                }
            }
        }

        return spaces;
    }

    public HashSet<BoardSpace> findAvailableAdjacentSpaces(int x, int y)
    {
        HashSet<BoardSpace> spaces = new HashSet<BoardSpace>();
        for (int dx = (x > 0 ? -1 : 0); dx <= (x < Constants.GRID_ROW ? 1 : 0); ++dx)
        {
            for (int dy = (y > 0 ? -1 : 0); dy <= (y < Constants.GRID_COL ? 1 : 0); ++dy)
            {
                if (dx != 0 || dy != 0)
                {
                    //Debug.Log("x:" + x + ",y:" + y + ",dx:" + dx + ",dy:" + dy);
                    int currRow = (x + dx) >= Constants.GRID_ROW ? Constants.GRID_ROW - 1 : x + dx;
                    int currCol = (y + dy) >= Constants.GRID_COL ? Constants.GRID_COL - 1 : y + dy;
                    BoardSpace currSpace = BoardGrid[currRow, currCol];
                    if (!currSpace.isOccupied && !currSpace.isSea && !currSpace.isMountain)
                    {
                        spaces.Add(currSpace);
                    }
                }
            }
        }

        return spaces;
    }

    private List<BoardSpace> identifySeaBoardSpaces()
    {
        List<BoardSpace> spaces = new List<BoardSpace>();
        foreach(BoardSpace space in allBoardSpaces)
        {
            if(space.isSea)
            {
                spaces.Add(space);
            }
        }

        return spaces;
    }

    private List<BoardSpace> identifyIslandBoardSpaces()
    {
        List<BoardSpace> spaces = new List<BoardSpace>();
        foreach (BoardSpace space in allBoardSpaces)
        {
            if (!space.isSea && !space.isMountain)
            {
                spaces.Add(space);
            }
        }

        return spaces;
    }

    private void GenerateBoardGrid()
    {
        int currIndex = 0;
        for(int i = 0; i < Constants.GRID_ROW; i++)
        {
            for (int j = 0; j < Constants.GRID_ROW; j++)
            {
                BoardGrid[i, j] = allBoardSpaces[currIndex];
                allBoardSpaces[currIndex].row = i;
                allBoardSpaces[currIndex].column = j;
                currIndex++;
            }
        }
    }

    /* SINGLETON */
    private static GameHandler instance;
    public static GameHandler Instance { get => instance; set => instance = value; }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

    }

}
