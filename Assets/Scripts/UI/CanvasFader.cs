using FTS.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasFader : MonoBehaviour
{
    CanvasGroup canvasGroup;
    [SerializeField] float fadeDuration = 0.25f;
    bool isLerping = false;

    #region MonoBehaviour Callbacks
    private void Awake()
    { 
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            FadeCanvas(1);
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            FadeCanvas(0);
        }
    }
    #endregion

    public void FadeCanvas(int value)
    {
        if (!isLerping)
        {
            StartCoroutine(LerpCanvas(value));
            canvasGroup.blocksRaycasts = value != 0;
        }
    }

    #region Coroutines
    public IEnumerator LerpCanvas(int canvasAlpha)
    {
        isLerping = true;
        float time = 0;
        float startAplha = canvasGroup.alpha;

        while (time < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAplha, canvasAlpha, time / fadeDuration);

            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = canvasAlpha;
        isLerping = false;
    }
    #endregion
}
