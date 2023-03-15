#region Using Statements
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#endregion

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject pauseHUD;
    float previousTimeScale = 0;
    List<GameObject> screens = new List<GameObject>();

    #region MonoBehaviour Callbacks
    void Awake()
    {
        Transform allChildren = GetComponentInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            screens.Add(child.gameObject);
        }
        Game();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }
    #endregion

    #region Private Methods
    private void EnableHUD(string HUDName)
    {
        foreach (GameObject screen in screens)
        {
            if (screen.name == HUDName)
            {
                screen.SetActive(true);
            }
            else
            {
                screen.SetActive(false);
            }
        }
    }
    #endregion

    #region Public Methods
    public void Pause()
    {
        if (!pauseHUD.activeSelf)
        {
            EnableHUD("PauseHUD");
        }
        else
        {
            EnableHUD("GameHUD");
        }
    }

    public void Game()
    {
        EnableHUD("GameHUD");
    }

    public void GameOver()
    {
        //EnableHUD("GameOverHUD");
        SceneManager.LoadScene(Scenes.MainMenu.ToString());
    }

    public void Win()
    {
        EnableHUD("WinHUD");
    }

    public void Event()
    {
        EnableHUD("EventHUD");
    }

    public void Draft()
    {
        EnableHUD("DraftHUD");
    }

    public void Settings()
    {
        EnableHUD("SettingsHUD");
    }

    public void Exit()
    {
        Application.Quit();
    }
    #endregion
}
