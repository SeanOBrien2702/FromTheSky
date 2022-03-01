using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeedUI : MonoBehaviour
{
    [SerializeField] SeedController seed;
    [SerializeField] GameObject tmpInput;


    Vector3 positionToMoveTo;
    Vector3 startPosition;
    //bool isRandom;
    TMP_InputField text;


    void Start()
    {
        startPosition = transform.position;
        positionToMoveTo = transform.position + new Vector3(-200, 0, 0);
        text = tmpInput.GetComponent<TMP_InputField>();
        //StartCoroutine(LerpPosition(positionToMoveTo, 1f));
    }
    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }

    public void ToggleSeed()
    {
        seed.IsRandom = !seed.IsRandom;
        if(!seed.IsRandom)
        {
            StartCoroutine(LerpPosition(positionToMoveTo, 0.5f));
        }
        else
        {
            StartCoroutine(LerpPosition(startPosition, 0.5f));
        }
    }

    public void OnEditEnd()
    {
        Debug.Log("done editing "+ text.text);
        seed.Seed = text.text.GetHashCode();
    }
}

