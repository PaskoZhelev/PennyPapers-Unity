using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [HideInInspector]
    public int NumberToPlace;
    [HideInInspector]
    public bool isNumberToPlaceSelected;

    public SelectableNumber[] generalSelectableNumbers;
    public SelectableNumber[] purpleSelectableNumbers;

    public GameObject[] skullMarks;
    public Text[] treasureNumbers;
    public Text treasuresTotalScore;
    public Text[] skullNumbers;
    public Text skullsTotalScore;
    public Text FinalScore;

    public GameObject GameEndPanel;
    public Text gameEndFinalScoreText;
    public Text gameEndSuccessLevelText;

    [HideInInspector]
    public float timeRemaining;
    [HideInInspector]
    public bool timerIsRunning;

    public Text textTimer;

    public GameObject undoButton;

    public GameObject SpecialSelectionPanel;
    public GameObject RedSelectionPanel;
    public GameObject PurpleSelectionPanel;
    public GameObject GreenYesNoSelectionPanel;
    public GameObject GreenSelectionPanel;
    public GameObject GreenPurpleSelectionPanel;

    public GameObject LoadingPanel;

    public Sprite skullSprite;
    public Sprite shipSprite;

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

    public void setPurpleSelectableNumbers()
    {
        for (int i = 0; i < purpleSelectableNumbers.Length; i++)
        {
            purpleSelectableNumbers[i].SetNumber(i + 1);
            purpleSelectableNumbers[i].UnselectPanel();
            purpleSelectableNumbers[i].SetRelatedNumbers(purpleSelectableNumbers);
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

    public void ShowSkullMark()
    {
        for (int i = 0; i < skullMarks.Length; i++)
        {
            if(!skullMarks[i].activeInHierarchy)
            {
                skullMarks[i].SetActive(true);
                return;
            }
        }
    }

    public void ShowTreasureNumber(int num, Color32 color)
    {
        for (int i = 0; i < treasureNumbers.Length; i++)
        {
            Text currTextNum = treasureNumbers[i];
            if (!currTextNum.gameObject.activeInHierarchy)
            {
                currTextNum.gameObject.SetActive(true);
                currTextNum.text = num.ToString();
                currTextNum.color = color;
                return;
            }
        }
    }

    public void HideTreasureNumber(int num)
    {
        for (int i = 0; i < treasureNumbers.Length; i++)
        {
            Text currTextNum = treasureNumbers[i];
            if (currTextNum.text.Equals(num.ToString()))
            {
                currTextNum.gameObject.SetActive(false);
                return;
            }
        }
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

    public void ShowEndGamePanel()
    {
        Player player = GameHandler.Instance.player;
        for (int i = 0; i < player.skullPoints.Count; i++)
        {
            int currPoints = player.skullPoints[i];
            Text textField = skullNumbers[i];
            textField.gameObject.SetActive(true);
            textField.text = currPoints.ToString();
            // colors
            if (currPoints > 0)
            {
                textField.color = Constants.NUMBER_COLORS[currPoints - 1];
            } else if(currPoints < 0)
            {
                int positiveNum = currPoints * (-1);
                textField.color = Constants.NUMBER_COLORS[positiveNum - 1];
            }
        }
        treasuresTotalScore.text = player.treasureTotalPoints.ToString();
        skullsTotalScore.text = player.skullTotalPoints.ToString();
        FinalScore.text = player.FinalScore.ToString();
        treasuresTotalScore.gameObject.SetActive(true);
        skullsTotalScore.gameObject.SetActive(true);
        FinalScore.gameObject.SetActive(true);
        gameEndFinalScoreText.text = player.FinalScore.ToString();
        gameEndSuccessLevelText.text = Constants.getTextSuccessLevelScore(player.FinalScore);
        GameEndPanel.SetActive(true);
    }

    public void resetNumToPlace()
    {
        NumberToPlace = 0;
        isNumberToPlaceSelected = false;
    }

    public void ShowLoadingPanel()
    {
        LoadingPanel.SetActive(true);
    }

    public void HideLoadingPanel()
    {
        LoadingPanel.SetActive(false);
    }


    public void ShowRedSelectionPanel()
    {
        SpecialSelectionPanel.SetActive(true);
        RedSelectionPanel.SetActive(true);
    }

    public void ShowPurpleSelectionPanel()
    {
        SpecialSelectionPanel.SetActive(true);
        PurpleSelectionPanel.SetActive(true);
    }

    public void ShowGreenYesNoSelectionPanel()
    {
        SpecialSelectionPanel.SetActive(true);
        GreenYesNoSelectionPanel.SetActive(true);
    }

    public void ShowGreenPurpleSelectionPanel()
    {
        SpecialSelectionPanel.SetActive(true);
        GreenPurpleSelectionPanel.SetActive(true);
    }

    public void GreenYesClicked()
    {
        SpecialSelectionPanel.SetActive(true);
        GreenYesNoSelectionPanel.SetActive(false);
        GreenSelectionPanel.SetActive(true);
        GameHandler.Instance.EnableSeaSpaces();
    }

    public void GreenNoClicked()
    {
        DiceManager.Instance.findPossibleSelectionNumbersIgnoringSpecial();
        GameHandler.Instance.ShowNecessaryActions();
        HideAllSelectionPanels();
    }

    public void GreenClicked()
    {
        HideAllSelectionPanels();
        GameHandler.Instance.currentFillType = FillType.SHIP;
        GreenYesClicked();
    }

    public void PurpleClicked()
    {
        HideAllSelectionPanels();
        GameHandler.Instance.currentFillType = FillType.NUMBER;
        setPurpleSelectableNumbers();
        ShowPurpleSelectionPanel();
    }

    public void HideAllSelectionPanels()
    {
        SpecialSelectionPanel.SetActive(false);
        RedSelectionPanel.SetActive(false);
        PurpleSelectionPanel.SetActive(false);
        GreenSelectionPanel.SetActive(false);
        GreenYesNoSelectionPanel.SetActive(false);
        GreenPurpleSelectionPanel.SetActive(false);
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

    public void ActivateMainMenuScene()
    {
        SceneManager.LoadScene("MainMenuScene");
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
