#region Using Statements
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
#endregion

public class Fader : MonoBehaviour
{
    [SerializeField] Animator transition;

    float transitionTime = 1f;

    #region MonoBehaviour Callbacks
    //private void Update()
    //{
    //    if(Input.GetMouseButtonDown(0))
    //    {
    //        Debug.Log("mouse down");
    //        StartCoroutine(LoadGame());
    //    }
    //}
    #endregion

    #region Coroutines
    IEnumerator LoadGame()
    {
        transition.enabled = true;
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("GameScene");
    }
    #endregion
}
