using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Characters;
using FTS.Cards;
using System;

public class Projectile : MonoBehaviour
{
    Card card;

    public Card Card { get => card; set => card = value; }

    private void OnCollisionEnter(Collision collision)
    {
        Unit unit = collision.collider.GetComponent<Unit>();
        if (unit)
        {
            foreach (var effect in card.Effects)
            {
                effect.ActivateEffect(unit);
            }
        }
    }
}
