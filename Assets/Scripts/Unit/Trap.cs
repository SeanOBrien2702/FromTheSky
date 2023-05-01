using FTS.Cards;
using FTS.Characters;
using FTS.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] Effect[] effects;
 
    public void SetPosition(HexCell position)
    {
        transform.position = position.transform.position;
        position.Trap = this;
    }

    public Effect[] GetEffects()
    {
        return effects;
    }

    internal void ActivateTrap(Unit unit)
    {
        foreach (var effect in effects)
        {
            effect.ActivateEffect(unit);
        }
        Destroy(gameObject);
    }
}
