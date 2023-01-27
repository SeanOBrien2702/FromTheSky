#region Using Statements
using FTS.Turns;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#endregion

namespace FTS.UI
{
    public class TimeUI : MonoBehaviour
    {
        Text dateTime;
        List<Button> buttons = new List<Button>();
        [SerializeField] TurnController tc;

        #region MonoBehaviour Callbacks
        void Start()
        {
            TurnController.OnPlayerTurn += UpdateUI;
            buttons = new List<Button>(GetComponentsInChildren<Button>());
            dateTime = GetComponentInChildren<Text>();
            dateTime.text = "This is a test";
        }

        private void OnDestroy()
        {
            TurnController.OnPlayerTurn -= UpdateUI;
        }
        #endregion

        #region Events
        void UpdateUI()
        {
            //dateTime.text = tc.GetTimeOfDay().ToString();
            dateTime.text = "Day: " + tc.Turn.ToString();
        }
        #endregion
    }
}