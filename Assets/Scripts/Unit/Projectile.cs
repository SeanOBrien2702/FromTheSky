using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Characters;

public class Projectile : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Unit unit = collision.collider.GetComponent<Unit>();
        if (unit)
        {
            unit.CalculateDamageTaken(2);
        }
    }
}
