using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Characters;

public class Projectile : MonoBehaviour
{
    [SerializeField] int speed = 2;

    void Update()
    {
        // Move the object forward along its z axis 1 unit/second.
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

}
