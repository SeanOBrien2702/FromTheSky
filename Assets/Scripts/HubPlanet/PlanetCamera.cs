using System;
using FTS.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCamera : MonoBehaviour
{
    [SerializeField] float turnSpeed = 10f;
    [SerializeField] float inactiveLength = 10f;
    float inactiveTimer = 0;
    float duration = 0.5f;


    void LateUpdate()
    {

        float input = Input.GetAxis("Horizontal") * turnSpeed;
        if(input != 0)
        {
            inactiveTimer = 0;
            transform.RotateAround(transform.position, Vector3.up, turnSpeed * Time.deltaTime * -input);
        }
        else
        {
            inactiveTimer += Time.deltaTime;
        }

        if(inactiveTimer > inactiveLength)
        {
            transform.RotateAround(transform.position, Vector3.up, turnSpeed * Time.deltaTime / 1.5f);
        }
    }

    internal void PanToObjective(MapObjective nextObjective)
    {
        inactiveTimer = 0;
        StartCoroutine(LerpToObjective(nextObjective.transform.rotation));
    }


    IEnumerator LerpToObjective(Quaternion targetPosition)
    {
        float time = 0;
        Quaternion startPosition = transform.rotation;

        while (time < duration)
        {
            transform.rotation = Quaternion.Lerp(startPosition, targetPosition, time / duration);
            time += UnityEngine.Time.deltaTime;
            yield return null;
        }
    }
}
