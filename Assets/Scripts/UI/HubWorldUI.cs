#region Using Statements
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#endregion

namespace SP.UI
{
    public class HubWorldUI : MonoBehaviour
    {
        [SerializeField] Button combatButton;
        [SerializeField] Button eventButton;
        [SerializeField] Button draftButton;

        #region Public Methods
        public void Combat()
        {
            SceneManager.LoadScene("GameScene");
        }

        public void Draft()
        {
            SceneManager.LoadScene("DraftScene");
        }
        #endregion
    }
}
