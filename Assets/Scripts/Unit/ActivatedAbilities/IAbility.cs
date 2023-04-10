using FTS.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility
{
    int Cost { get; set; }
    bool IsUsed { get; set; }
    Sprite AbiltyImage { get; set; }   

    public void ActivateEffect();

    public string GetDescription();
}