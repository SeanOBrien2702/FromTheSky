using System.Collections;
using System.Collections.Generic;
using TMPro;
using FTS.Core;
using UnityEngine;
using UnityEngine.UI;
using FTS.Turns;

namespace FTS.UI
{
    public class GameTutorialUI : MonoBehaviour
    {
        [SerializeField] RectTransform tutorialWindow;

        [SerializeField] GameObject tutorialPanel;
        [SerializeField] TextMeshProUGUI tutorialText;

        [SerializeField] private List<TutorialInfo> panels;

        [SerializeField] private Button buttonPrev;
        [SerializeField] private Button buttonNext;

        int index = 0;

        void Start()
        {
            buttonPrev.onClick.AddListener(Click_Prev);
            buttonNext.onClick.AddListener(Click_Next);
            buttonPrev.gameObject.SetActive(false);
            tutorialText.text = panels[0].Text;
            tutorialWindow.localPosition = panels[0].WindowPos;
            tutorialWindow.sizeDelta = panels[0].WindowSize;
            tutorialWindow.gameObject.SetActive(false);
            tutorialPanel.SetActive(false);
            TurnController.OnPlayerTurn += TurnController_OnPlayerTurn;
        }

        private void OnDestroy()
        {
            TurnController.OnPlayerTurn -= TurnController_OnPlayerTurn;
        }

        public void CancelTutorial()
        {
            tutorialWindow.gameObject.SetActive(false);
            tutorialPanel.gameObject.SetActive(false); 
            TutorialController.Instance.SetTutorial(true);
        }

        public void Click_Prev()
        {
            ChangePage(-1);
        }

        public void Click_Next()
        {
            ChangePage(1);
        }

        void ChangePage(int direction)
        {
            buttonNext.gameObject.SetActive(true);
            buttonPrev.gameObject.SetActive(true);

            index += direction;
            if (index <= 0)
            {
                index = 0;
                buttonPrev.gameObject.SetActive(false);
            }
            else if (index >= panels.Count - 1)
            {
                index = panels.Count - 1;
                buttonNext.gameObject.SetActive(false);
            }

            tutorialText.text = panels[index].Text;
            tutorialWindow.localPosition = panels[index].WindowPos;
            tutorialWindow.sizeDelta = panels[index].WindowSize;
        }

        private void TurnController_OnPlayerTurn()
        {
            if(!TutorialController.Instance.IsTutorialComplete)
            {
                tutorialWindow.gameObject.SetActive(true);
                tutorialPanel.gameObject.SetActive(true);
            }
        }
    }
}
