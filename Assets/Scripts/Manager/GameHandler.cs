using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public BoardSpace[] allBoardSpaces;
    public BoardSpace[] initialIslandBoardSpaces;

    public BoardSpace[,] BoardGrid = new BoardSpace[Constants.GRID_ROW, Constants.GRID_COL];

    [HideInInspector]
    public List<BoardSpace> seaBoardSpaces;
    [HideInInspector]
    public List<BoardSpace> islandBoardSpaces;

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
        player = new Player();
        isFirstTurn = true;
        seaBoardSpaces = identifySeaBoardSpaces();
        islandBoardSpaces = identifyIslandBoardSpaces();
        GenerateBoardGrid();

        StartCoroutine(StartNewTurnWithDelay());
    }

    public void StartNewTurn()
    {
        // Game End
        if (isGameFinished())
        {

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
        Debug.Log("Showing necessary Actions");
        // if there are possible selection numbers 
        // then no special die was rolled
        if (DiceManager.Instance.currentPossibleSelectionNumbers.Count > 0)
        {
            Debug.Log("No special dice rolled");
            currentFillType = FillType.NUMBER;
            UIHandler.Instance.setGeneralSelectableNumbers();
            return;
        }

        // RED
        if (DiceManager.Instance.isRedSpecialDieRolled())
        {
            Debug.Log("RED rolled");
            currentFillType = FillType.SKULL;
            UIHandler.Instance.ShowRedSelectionPanel();
            EnablePossibleAdjacentToLastFilled();
            return;
        }

        // PURPLE
        if (DiceManager.Instance.isPurpleSpecialDieRolled() && !DiceManager.Instance.isGreenSpecialDieRolled())
        {
            Debug.Log("PURPLE rolled");
            currentFillType = FillType.NUMBER;
            UIHandler.Instance.setPurpleSelectableNumbers();
            UIHandler.Instance.ShowPurpleSelectionPanel();
            return;
        }

        // GREEN
        if (DiceManager.Instance.isGreenSpecialDieRolled() && !DiceManager.Instance.isPurpleSpecialDieRolled())
        {
            Debug.Log("GREEN rolled");
            currentFillType = FillType.SHIP;
            UIHandler.Instance.ShowGreenYesNoSelectionPanel();
            return;
        }

        // GREEN and PURPLE
        if (DiceManager.Instance.isGreenSpecialDieRolled() && DiceManager.Instance.isPurpleSpecialDieRolled())
        {
            Debug.Log("GREEN and PURPLE rolled");
            UIHandler.Instance.ShowGreenPurpleSelectionPanel();
            return;
        }

        //Max skulls already filled so Red die is ignored
        DiceManager.Instance.findPossibleSelectionNumbersIgnoringSpecial();
        ShowNecessaryActions();
    }

    // space is clicked
    public void SpaceClicked()
    {
        switch (currentFillType)
        {
            case FillType.NUMBER:
                lastFilledSpace.SetNumber(UIHandler.Instance.NumberToPlace);
                break;
            case FillType.SKULL:
                lastFilledSpace.PutSkull();
                break;
            // Ship
            default:
                lastFilledSpace.PutShip();
                break;
        }

        DisableAllSpaces();

        // TODO Undo functionality disabled for now
        // Turn finished is called immediately
        //UIHandler.Instance.StartTimer();
        GameHandler.Instance.TurnFinished();
    }

    public void TurnFinished()
    {
        if (isFirstTurn) { isFirstTurn = false; }
        UIHandler.Instance.resetGeneralSelectableNumbers();
        UIHandler.Instance.HideAllSelectionPanels();
        StartNewTurn();
    }

    public void EnablePossibleIslandSpaces()
    {
        if(isFirstTurn)
        {
            EnableInitialIslandSpaces();
            return;
        }
        
        foreach(BoardSpace space in islandBoardSpaces)
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

        HashSet<BoardSpace> spaces = new HashSet<BoardSpace>();
        for (int dx = (x > 0 ? -1 : 0); dx <= (x < Constants.GRID_ROW ? 1 : 0); ++dx)
        {
            for (int dy = (y > 0 ? -1 : 0); dy <= (y < Constants.GRID_COL ? 1 : 0); ++dy)
            {
                if (dx != 0 || dy != 0)
                {
                    Debug.Log("x:" + x + ",y:" + y + ",dx:" + dx + ",dy:" + dy);
                    BoardSpace currSpace = BoardGrid[x + dx, y + dy];
                    if(!currSpace.isOccupied && !currSpace.isSea && !currSpace.isMountain)
                    {
                        spaces.Add(currSpace);
                    }
                }
            }
        }

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
        if (player.treasures.Count == Constants.MAX_TREASURES || player.numIslandSpacesFilled == Constants.MAX_ISLAND_SPACES_BOARD_TORTUGA)
        {
            return true;
        }

        return false;
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
