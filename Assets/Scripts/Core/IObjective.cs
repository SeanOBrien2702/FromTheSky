using SP.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjective 
{
    string ObjectiveDescription { get; set; }
    int GoldReward { get; set; }
    Card CardReward { get; set; }
    bool IsComplete { get; set; }
    bool IsOptional { get; set; }

    public void UpdateObjective();
}
