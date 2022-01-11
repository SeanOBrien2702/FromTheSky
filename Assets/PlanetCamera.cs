using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCamera : MonoBehaviour
{
    [SerializeField] float turnSpeed = 10f;
    [SerializeField] float inactiveLength = 10f;
    float inactiveTimer = 0;


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
            //if mouse not over planet
            //if()
            inactiveTimer += Time.deltaTime;
        }

        if(inactiveTimer > inactiveLength)
        {
            transform.RotateAround(transform.position, Vector3.up, turnSpeed * Time.deltaTime / 1.5f);
        }
    }
}
