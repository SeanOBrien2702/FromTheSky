#region Using Statements
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FTS.Characters;
using FTS.Turns;
using System;
#endregion

namespace FTS.UI
{
    public class TelegraphIntentUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI damageText;
        [SerializeField] Image damageImage;

        #region MonoBehaviour Callbacks
        void Start()
        {
            ClearIntent();
            TurnController.OnEnemySpawn += TurnController_OnEnemySpawn;
        }

        private void OnDestroy()
        {
            TurnController.OnEnemySpawn -= TurnController_OnEnemySpawn;
        }
        #endregion
        #region Private Methods

        
        #endregion

        #region Public Methods
        public void ShowAttack(int attack)
        {
            damageImage.gameObject.SetActive(true);
            damageText.text = attack.ToString();
        }

        public void ClearIntent()
        {
            damageImage.gameObject.SetActive(false);
        }
        #endregion

        #region Events
        private void TurnController_OnEnemySpawn()
        {
            ClearIntent();
        }
        #endregion
    }
}
