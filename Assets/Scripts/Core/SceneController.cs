using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FTS.Core
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] CanvasFader fader;
        public static event Action OnAdditiveSceneLoaded = delegate { };
        string addativeScene = null;
        string baseScene = null;
        public static SceneController Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        private void LoadNextScene(string sceneName)
        {
            if (sceneName == baseScene && SceneManager.sceneCount > 1)
            {
                SceneManager.UnloadSceneAsync(addativeScene);
                addativeScene = null;
            }
            else
            {             
                baseScene = sceneName;
                addativeScene = null;
                StartCoroutine(StartLoad(sceneName));
            }
        }

        IEnumerator StartLoad(string sceneName)
        {
            yield return StartCoroutine(fader.LerpCanvas(1));
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            while (!operation.isDone)
            {
                yield return null;
            }
            yield return StartCoroutine(fader.LerpCanvas(0));
        }

        public void LoadScene(Scenes scene, bool isAddative = false)
        {
            if (isAddative)
            {
                if(addativeScene == scene.ToString())
                {
                    return;
                }
                baseScene = 
                addativeScene = scene.ToString();
                OnAdditiveSceneLoaded?.Invoke();
                SceneManager.LoadScene(addativeScene, LoadSceneMode.Additive);               
            }
            else
            {
                LoadNextScene(scene.ToString());
            }
        }

        public void LoadScene(Encounter encounter)
        {
            if (encounter.IsSceneAddative)
            {
                OnAdditiveSceneLoaded?.Invoke();
                addativeScene = encounter.NextScene.ToString();
                SceneManager.LoadScene(addativeScene, LoadSceneMode.Additive);
            }
            else
            {
                LoadNextScene(encounter.NextScene.ToString());
            }
        }
    }
}
