using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text appVersion;

    // Start is called before the first frame update
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        // some scenes don't have the appVersion text
        if(null != appVersion)
        {
            appVersion.text = Application.version;
        }
    }

    public void ActivateMainMenuScene()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void ActivateGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ActivateHighScoresScene()
    {
        SceneManager.LoadScene("HighScoresScene");
    }

    public void ActivateRulesScene()
    {
        SceneManager.LoadScene("RulesScene");
    }

    /* SINGLETON */
    private static GameManager instance;
    public static GameManager Instance { get => instance; set => instance = value; }

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
