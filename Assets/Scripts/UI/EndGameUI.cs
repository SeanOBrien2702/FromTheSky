using FTS.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    [Header("Background")]
    [SerializeField] TextMeshProUGUI text;

    [Header("Background")]
    [SerializeField] Sprite winBackground;
    [SerializeField] Sprite lostBackground;
    [SerializeField] Image background;
    // Start is called before the first frame update
    void Start()
    {
        UpdateUI(RunController.Instance.HasWon);
    }

    void UpdateUI(bool hasWon)
    {
        if(hasWon)
        {
            background.sprite = winBackground;
            text.text = "You have won!";
        }
        else
        {
            background.sprite = lostBackground;
            text.text = "Game over!";
        }
    }

    public void MainMenu()
    {
        SceneController.Instance.LoadScene(Scenes.MainMenu);
    }

    public void StartOver()
    {
        SceneController.Instance.LoadScene(Scenes.CharacterSelectScene);
    }
}
