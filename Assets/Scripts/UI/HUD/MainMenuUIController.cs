#region Using Statements
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FTS.Saving;
using TMPro;
using FTS.Core;
#endregion

namespace FTS.UI
{
    public class MainMenuUIController : MonoBehaviour
    {
        [SerializeField] Animator transition;
        [SerializeField] GameObject panel;
        SavingSystem saving;

        [Header("Buttons")]
        [SerializeField] Button continueButton;     
        [SerializeField] GameObject newGameButton;
        [SerializeField] TextMeshProUGUI newGameText;

        private void Start()
        {
            saving = FindObjectOfType<SavingSystem>().GetComponent<SavingSystem>();
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

        #region Public Methods
        public void Continue()
        {
            saving.Continue();
            SceneController.Instance.LoadScene(Scenes.GameScene);
        }

        public void NewGame()
        {
            saving.NewGame();
            SceneController.Instance.LoadScene(Scenes.CharacterSelectScene);
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
    }
}