using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text appVersion;

    public GameObject MainMenuButtonsPanel;
    public GameObject MapSelectionButtonsPanel;
    public GameObject HighScoresMapSelectionButtonsPanel;

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

    public void ContinueToMapSelection()
    {
        HideMainMenuButtonsPanel();
        ShowMapSelectionButtonsPanel();
    }

    public void ContinueToHighScoresMapSelection()
    {
        HideMainMenuButtonsPanel();
        ShowHighScoresMapSelectionButtonsPanel();
    }

    public void HideMainMenuButtonsPanel()
    {
        MainMenuButtonsPanel.SetActive(false);
    }

    public void ShowMapSelectionButtonsPanel()
    {
        MapSelectionButtonsPanel.SetActive(true);
    }

    public void TortugaMapSelected()
    {
        Constants.SELECTED_MAP = Constants.TORTUGA_MAP;
        Constants.SELECTED_MAP_MAX_ISLAND_SPACES = Constants.MAX_ISLAND_SPACES_BOARD_TORTUGA;
        ActivateGameScene();
    }

    public void MaracaoMapSelected()
    {
        Constants.SELECTED_MAP = Constants.MARACAO_MAP;
        Constants.SELECTED_MAP_MAX_ISLAND_SPACES = Constants.MAX_ISLAND_SPACES_BOARD_MARACAO;
        ActivateGameScene();
    }

    public void ShowHighScoresMapSelectionButtonsPanel()
    {
        HighScoresMapSelectionButtonsPanel.SetActive(true);
    }

    public void TortugaHighScoresSelected()
    {
        Constants.SELECTED_MAP = Constants.TORTUGA_MAP;
        ActivateHighScoresScene();
    }

    public void MaracaoHighScoresSelected()
    {
        Constants.SELECTED_MAP = Constants.MARACAO_MAP;
        ActivateHighScoresScene();
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
