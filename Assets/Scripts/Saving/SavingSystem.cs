/*
* File: SavingSystem.cs
* Project: Simulation and Game Development Solo Assignment
* Programmer: Sean O'Brien
* Description: This script saves the current state of objects to a .sav file. 
*              The SavingWrapper is used to control when the .sav file is 
*              created/updated, loaded and deleted.
*/
using Bayat.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FTS.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        string defaultFile = "save";
        int hasCurrentGame;

        Dictionary<string, object> state = new Dictionary<string, object>();

        public int HasCurrentGame { get => hasCurrentGame; set => hasCurrentGame = value; }

        private void Awake()
        {
            hasCurrentGame = PlayerPrefs.GetInt("CurrentGameState", 0);
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.L))
            //{
            //    Load(defaultFile);
            //}

            //if (Input.GetKeyDown(KeyCode.K))
            //{
            //    Save(defaultFile);
            //}
        }
        internal void Continue()
        {
            PlayerPrefs.SetInt("CurrentGameState", 0);
        }

        internal void NewGame()
        {
            PlayerPrefs.SetInt("CurrentGameState", 1);
        }


        public void Save(string saveFile)
        {
            CaptureState();
        }

        public void Load(string saveFile)
        {
            RestoreState();
        }

        public void Delete(string saveFile)
        {
            File.Delete(GetPathFromSaveFile(saveFile));
        }

        private void CaptureState()
       {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                SaveSystemAPI.SaveAsync(saveable.GetUniqueIdentifier(), saveable.CaptureState());
                Debug.Log("Data saved successfully");
            }
        }

        private async void RestoreState()
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                object loadedObject = await SaveSystemAPI.LoadAsync<object>(saveable.GetUniqueIdentifier());
                saveable.RestoreState(loadedObject);
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}