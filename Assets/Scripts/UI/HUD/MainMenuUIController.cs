#region Using Statements
using UnityEngine;
using UnityEngine.SceneManagement;
#endregion

namespace FTS.UI
{
    public class MainMenuUIController : MonoBehaviour
    {
        [SerializeField] Animator transition;
        [SerializeField] GameObject panel;

        #region Public Methods
        public void Continue()
        {
            ///StartCoroutine(LoadGame());
            SceneManager.LoadScene("GameScene");
        }

        public void NewGame()
        {
            //StartCoroutine(LoadGame());
            SceneManager.LoadScene("CharacterSelectScene");
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