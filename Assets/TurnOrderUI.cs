using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Characters;
using FTS.Turns;
using System;

public class TurnOrderUI : MonoBehaviour
{
    [SerializeField] UnitController unitController;
    [SerializeField] CharacterTurnPosition CharacterTurnPosition;
    Dictionary<Character, CharacterTurnPosition> turnPositions = new Dictionary<Character, CharacterTurnPosition>();

    internal void FillUI()
    {
        List<Character> units = unitController.GetUnits();

        foreach (var unit in units)
        {
            CharacterTurnPosition newTurnPosition = Instantiate(CharacterTurnPosition);
            newTurnPosition.transform.SetParent(transform, false);
            turnPositions.Add(unit, newTurnPosition);
            turnPositions[unit].SetPositionText(unit);

        }
        
    }
}
