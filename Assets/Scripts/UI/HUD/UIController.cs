#region Using Statements
using FTS.Characters;
using FTS.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#endregion

public class UIController : MonoBehaviour
{
    [SerializeField] Canvas pauseHUD;
    Dictionary<string, Canvas> screens = new Dictionary<string, Canvas>();

    #region MonoBehaviour Callbacks
    void Awake()
    {
        foreach (Canvas child in GetComponentsInChildren<Canvas>())
        {
            screens.Add(child.gameObject.name, child);
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
        foreach (var screen in screens)
        {
            screen.Value.enabled = false;

        }
        screens[HUDName].enabled = true;
    }
    #endregion

    #region Public Methods
    public void Pause()
    {
        if (!pauseHUD.enabled)
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
        EnableHUD("GameHUD");
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
