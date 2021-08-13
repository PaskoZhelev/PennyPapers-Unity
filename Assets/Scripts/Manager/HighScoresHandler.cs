using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using BayatGames.SaveGameFree;
using System.Collections.Generic;
using System.Linq;
using System;

public class HighScoresHandler : MonoBehaviour
{
    public Text[] scorePanelLines;

    public Text gamesPlayed;
    public Text averageScore;

    public GameObject ScoresPanel;
    public GameObject NoScoresPanel;

    private int MAX_DISPLAY_ELEMENTS = 5;
    // Use this for initialization
    void Start()
    {
        DisplayScores();
    }

    private void DisplayScores()
    {
        List<int> gameScores = SaveGame.Load<List<int>>(Constants.TORTUGA_MAP, true, SaveGame.EncodePassword);

        if (null == gameScores || gameScores.Count == 0)
        {
            NoScoresPanel.SetActive(true);
            return;
        }

        int limitLenght = MAX_DISPLAY_ELEMENTS < gameScores.Count ? MAX_DISPLAY_ELEMENTS : gameScores.Count;
        List<int> sortedScores = gameScores.OrderByDescending(o => o).ToList();

        CalculateAverageScore(sortedScores);

        for (int i = 0; i < limitLenght; i++)
        {
            scorePanelLines[i].text = (i + 1) + ". " + sortedScores[i].ToString();
        }

        ScoresPanel.SetActive(true);
    }

    private void CalculateAverageScore(List<int> sortedScores)
    {
        gamesPlayed.text = sortedScores.Count.ToString();
        double average = Math.Round(sortedScores.Average(gm => gm), 2);
        averageScore.text = average.ToString();
    }

    public void ClickBackToMainMenuBtn()
    {
        GameManager.Instance.ActivateMainMenuScene();
    }
}
