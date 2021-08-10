using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{

    public Die[] Dice;
    public Sprite[] NumberSprites;
    public Sprite GreenSideDie;
    public Sprite PurpleSideDie;
    public Sprite RedSideDie;

    [HideInInspector]
    public SortedSet<int> currentPossibleSelectionNumbers;

    private int[] rolledNumbers;

    // Start is called before the first frame update
    void Start()
    {
        currentPossibleSelectionNumbers = new SortedSet<int>();
        rolledNumbers = new int[Constants.NUM_DICE];
    }

    public void RollDice()
    {
        rolledNumbers = generateRandomDiceValues();
        for (int i = 0; i < Constants.NUM_DICE; i++)
        {
            Dice[i].RollDieAndSetValue(rolledNumbers[i]);
        }

        findPossibleSelectionNumbers();
    }

    private int[] generateRandomDiceValues()
    {
        int[] numbers = new int[Constants.NUM_DICE];
        for (int i = 0; i < Constants.NUM_DICE; i++)
        {
            // random number from 0 to 5 (inclusive)
            numbers[i] = Random.Range(0, Constants.DICE_SIDES);
        }
        return numbers;
    }

    public void findPossibleSelectionNumbers()
    {
        currentPossibleSelectionNumbers.Clear();

        // add all dice first
        for (int i = 0; i < Constants.NUM_DICE; i++)
        {
            // don't need to find possible numbers
            if(rolledNumbers[i] == 5)
            {
                currentPossibleSelectionNumbers.Clear();
                return;
            }

            currentPossibleSelectionNumbers.Add(rolledNumbers[i] + 1);
        }

        // add all sum combinations
        currentPossibleSelectionNumbers.Add((rolledNumbers[0] + 1) + (rolledNumbers[1] + 1));
        currentPossibleSelectionNumbers.Add((rolledNumbers[0] + 1) + (rolledNumbers[2] + 1));
        currentPossibleSelectionNumbers.Add((rolledNumbers[0] + 1) + (rolledNumbers[1] + 1) + (rolledNumbers[2] + 1));
    }

    public void findPossibleSelectionNumbersIgnoringSpecial()
    {
        currentPossibleSelectionNumbers.Clear();

        List<int> filteredList = new List<int>();
        for(int i = 0; i < rolledNumbers.Length; i++)
        {
            if(rolledNumbers[i] != 5)
            {
                filteredList.Add(rolledNumbers[i]);
            }
        }

        // add all dice first
        for (int i = 0; i < filteredList.Count; i++)
        {
            currentPossibleSelectionNumbers.Add(filteredList[i] + 1);
        }

        if(filteredList.Count == 1)
        {
            return;
        } else
        {
            currentPossibleSelectionNumbers.Add((filteredList[0] + 1) + (filteredList[1] + 1));
        }
    }

    public bool isRedSpecialDieRolled()
    {
        return Dice[Constants.RED_DIE_INDEX].Number == 6 && GameHandler.Instance.player.numSkullsFilled < Constants.MAX_SKULLS;
    }

    public bool isPurpleSpecialDieRolled()
    {
        return Dice[Constants.PURPLE_DIE_INDEX].Number == 6;
    }

    public bool isGreenSpecialDieRolled()
    {
        return Dice[Constants.GREEN_DIE_INDEX].Number == 6;
    }

    /* SINGLETON */
    private static DiceManager instance;
    public static DiceManager Instance { get => instance; set => instance = value; }

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
