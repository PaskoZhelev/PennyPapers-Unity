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


    // Start is called before the first frame update
    void Start()
    {
        currentPossibleSelectionNumbers = new SortedSet<int>();
    }

    public void RollDice()
    {
        int[] numbers = generateRandomDiceValues();
        for (int i = 0; i < Constants.NUM_DICE; i++)
        {
            Dice[i].RollDieAndSetValue(numbers[i]);
        }

        findPossibleSelectionNumbers(numbers);
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

    public void findPossibleSelectionNumbers(int[] numbers)
    {
        currentPossibleSelectionNumbers.Clear();

        // add all dice first
        for (int i = 0; i < Constants.NUM_DICE; i++)
        {
            // don't need to find possible numbers
            if(numbers[i] == 5)
            {
                currentPossibleSelectionNumbers.Clear();
                return;
            }

            currentPossibleSelectionNumbers.Add(numbers[i] + 1);
        }

        // add all sum combinations
        currentPossibleSelectionNumbers.Add((numbers[0] + 1) + (numbers[1] + 1));
        currentPossibleSelectionNumbers.Add((numbers[0] + 1) + (numbers[2] + 1));
        currentPossibleSelectionNumbers.Add((numbers[0] + 1) + (numbers[1] + 1) + (numbers[2] + 1));
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
