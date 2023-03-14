#region Using Statements
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FTS.Saving;
using TMPro;
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
            SceneManager.LoadScene(Scenes.GameScene.ToString());
        }

        public void NewGame()
        {
            saving.NewGame();
            SceneManager.LoadScene(Scenes.CharacterSelectScene.ToString());
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