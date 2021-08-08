using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [HideInInspector]
    public int NumberToPlace;
    [HideInInspector]
    public bool isNumberToPlaceSelected;

    public SelectableNumber[] generalSelectableNumbers;

    [HideInInspector]
    public float timeRemaining;
    [HideInInspector]
    public bool timerIsRunning;

    public Text textTimer;

    public GameObject undoButton;

    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = Constants.TIMER;
        timerIsRunning = false;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                HideUndoButton();
                ResetTimer();
                GameHandler.Instance.TurnFinished();
            }
        }
    }

    public void setGeneralSelectableNumbers()
    {
        resetGeneralSelectableNumbers();
        List<int> listNumbers = DiceManager.Instance.currentPossibleSelectionNumbers.ToList();

        for (int i = 0; i < listNumbers.Count; i++)
        {
            generalSelectableNumbers[i].SetNumber(listNumbers[i]);
            generalSelectableNumbers[i].SetRelatedNumbers(generalSelectableNumbers);
        }
    }

    public void resetGeneralSelectableNumbers()
    {
        for (int i = 0; i < generalSelectableNumbers.Length; i++)
        {
            generalSelectableNumbers[i].SetNumber(0);
            generalSelectableNumbers[i].UnselectPanel();
        }

        isNumberToPlaceSelected = false;
    }

    public void SetNumToPlace(int num)
    {
        if(!isNumberToPlaceSelected)
        {
            GameHandler.Instance.EnablePossibleIslandSpaces();
        }
        
        NumberToPlace = num;
        isNumberToPlaceSelected = true;
    }

    public void resetNumToPlace()
    {
        NumberToPlace = 0;
        isNumberToPlaceSelected = false;
    }

    public void ShowUndoButton()
    {
        undoButton.gameObject.SetActive(true);
    }

    public void HideUndoButton()
    {
        undoButton.gameObject.SetActive(false);
    }

    public void StartTimer()
    {
        timerIsRunning = true;
        ShowUndoButton();
    }

    public void ResetTimer()
    {
        timeRemaining = Constants.TIMER;
    }

    void DisplayTime(float timeToDisplay)
    {
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        if(seconds < 0)
        {
            textTimer.text = "0";
        } else
        {
            textTimer.text = seconds.ToString();
        }
    }


    /* SINGLETON */
    private static UIHandler instance;
    public static UIHandler Instance { get => instance; set => instance = value; }

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
