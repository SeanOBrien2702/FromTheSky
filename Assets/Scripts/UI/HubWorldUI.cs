#region Using Statements
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FTS.Core;
#endregion

namespace FTS.UI
{
    public class HubWorldUI : MonoBehaviour
    {
        [SerializeField] ObjectiveGenerator objective;
        [SerializeField] PlanetCamera planetCamera;

        #region Public Methods
        public void StartMisstion()
        {
            //SceneManager.LoadScene("GameScene");
        }

        public void NextMission()
        {
            //planetCamera.PanTo();
        }

        public void PreviousMission()
        {

        }
        #endregion
    }
}
