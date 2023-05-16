#region Using Statements
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FTS.Saving;
using TMPro;
using FTS.Core;
using System.Collections;
using System;
#endregion

namespace FTS.UI
{
    public class MainMenuUIController : MonoBehaviour
    {
        public static event System.Action OnCharacterSelect = delegate { };

        [SerializeField] GameObject panel;
        [SerializeField] float fadeDuration;
        SavingSystem saving;
        CanvasFader canvasFader;
        bool isLerping;

        [Header("Buttons")]
        [SerializeField] Button continueButton;     
        [SerializeField] GameObject newGameButton;
        [SerializeField] TextMeshProUGUI newGameText;

        private void Start()
        {
            CharacterSelectUI.OnReturn += CharacterSelectUI_OnReturn;
            saving = FindObjectOfType<SavingSystem>().GetComponent<SavingSystem>();
            canvasFader = GetComponent<CanvasFader>();
            if(saving.HasCurrentGame == 0)
            {
                continueButton.interactable = false;
                newGameText.text = "Start New Game";
            }
            else
            {
                continueButton.interactable = true;
                newGameText.text = "Start over";
            }
            panel.SetActive(false);
        }

        private void OnDestroy()
        {
            CharacterSelectUI.OnReturn -= CharacterSelectUI_OnReturn;
        }
        #region Public Methods
        public void Continue()
        {
            saving.Continue();
            SceneController.Instance.LoadScene(Scenes.GameScene);
        }

        public void NewGame()
        {
            canvasFader.FadeCanvas(0);
            saving.NewGame();
            OnCharacterSelect?.Invoke();
            SceneController.Instance.LoadScene(Scenes.CharacterSelectScene, true);           
        }

        public void ReturnToMainMenu()
        {
            canvasFader.FadeCanvas(1);
        }

        public void Settings()
        {

        }
        public void HowToPlay()
        {
            panel.SetActive(!panel.activeSelf);
        }

        public void Exit()
        {
            Application.Quit();
        }
        #endregion

        private void CharacterSelectUI_OnReturn()
        {
            canvasFader.FadeCanvas(1);
        }
    }
}