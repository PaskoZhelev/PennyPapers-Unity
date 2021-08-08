using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public BoardSpace[] allBoardSpaces;
    public BoardSpace[] initialIslandBoardSpaces;
    [HideInInspector]
    public List<BoardSpace> seaBoardSpaces;
    [HideInInspector]
    public List<BoardSpace> islandBoardSpaces;

    public bool isFirstTurn;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = new Player();
        isFirstTurn = true;
        seaBoardSpaces = identifySeaBoardSpaces();
        islandBoardSpaces = identifyIslandBoardSpaces();
        StartCoroutine(StartNewTurnWithDelay());
    }

    public void StartNewTurn()
    {
        // Game End
        if(isGameFinished())
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

        // if there are possible selection numbers 
        // then no special die was rolled
        if(DiceManager.Instance.currentPossibleSelectionNumbers.Count > 0)
        {
            UIHandler.Instance.setGeneralSelectableNumbers();
        }
    }

    // space is clicked
    public void SpaceFilled()
    {
        DisableAllSpaces();
        UIHandler.Instance.StartTimer();
    }

    public void TurnFinished()
    {
        if(isFirstTurn) { isFirstTurn = false; }
        UIHandler.Instance.resetGeneralSelectableNumbers();
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
