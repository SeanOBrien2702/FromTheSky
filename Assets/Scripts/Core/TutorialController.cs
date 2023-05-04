using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Core
{
    public class TutorialController : MonoBehaviour
    {
        bool isTutorialComplete = false;

        public static TutorialController Instance { get; private set; }
        public bool IsTutorialComplete { get => isTutorialComplete;}

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            if (!PlayerPrefs.HasKey("IsTutorialComplete"))
            {
                Debug.Log("tutorial key not found ");
                PlayerPrefs.SetInt("IsTutorialComplete", 0);
            }
            else
            {
                Debug.Log("tutorial key found ");
            }
            isTutorialComplete = (PlayerPrefs.GetInt("IsTutorialComplete") != 0);
            Debug.Log("tutorial complete " + isTutorialComplete);
        }


        public void ToggleTutorial()
        {
            isTutorialComplete = !isTutorialComplete;
            PlayerPrefs.SetInt("IsTutorialComplete", (isTutorialComplete ? 1 : 0));
        }

        internal void SetTutorial(bool setTutorial)
        {
            isTutorialComplete = setTutorial;
            PlayerPrefs.SetInt("IsTutorialComplete", (setTutorial ? 1 : 0));
        }

        //public bool GetTutorial()
        //{
        //    return isTutorialComplete;
        //}
    }
}
