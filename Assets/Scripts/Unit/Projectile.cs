using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Characters;
using FTS.Cards;
using System;

public class Projectile : MonoBehaviour
{
    Card card;
    int damage;

    public Card Card { get => card; set => card = value; }
    public int Damage { get => damage; set => damage = value; }

    private void OnCollisionEnter(Collision collision)
    {
        Unit unit = collision.collider.GetComponent<Unit>();
        if (unit)
        {
            //TODO: also have enemies pass in effects
            if (card != null)
            {
                foreach (var effect in card.Effects)
                {
                    effect.ActivateEffect(unit);
                }
            }
            else
            {
                unit.CalculateDamageTaken(damage);
            }
        }
    }
}
