#region Using Statements
using FTS.Core;
using FTS.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;
#endregion

namespace FTS.UI
{
    public class PauseUI : MonoBehaviour
    {
        GameObject player;
        [SerializeField] GameObject howToPlay;

        #region MonoBehaviour Callbacks
        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion

        #region Public Methods
        public void SaveGame()
        {
            //Save("Save");
        }

        public void LoadGame()
        {
            //Load("Save");
        }

        public void MainMenu()
        {
            SceneController.Instance.LoadScene(Scenes.MainMenu);
        }

        public void HowToPlay()
        {
            howToPlay.SetActive(!howToPlay.activeSelf);
        }

        public void Quit()
        {
            Application.Quit();
        }
        #endregion
    }
}