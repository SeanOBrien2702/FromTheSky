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

    private void Start()
    {
        TurnController.OnCombatStart += TurnController_OnCombatStart;
        UnitController.OnUnitTurn += UnitController_OnUnitTurn;
        
    }

    private void TurnController_OnCombatStart()
    {
        //
    }

    private void UnitController_OnUnitTurn(Character obj)
    {
        Debug.Log("turn order ui " + transform.GetChild(0).name);
        transform.GetChild(0).transform.SetAsLastSibling();
        int index = 1;
        foreach (Transform item in transform)
        {
            item.GetComponent<CharacterTurnPosition>().SetPositionText(index);
            index++;
        }
    }

    private void OnDestroy()
    {
        TurnController.OnCombatStart -= TurnController_OnCombatStart;
        UnitController.OnUnitTurn -= UnitController_OnUnitTurn;
    }

    //private void TurnController_OnUnitTurn(Character obj)
    //{
        
    //    transform.GetChild(0).transform.SetAsLastSibling();
    //}

    internal void FillUI()
    {
        List<Character> units = unitController.GetUnits();

        foreach (var unit in units)
        {
            CharacterTurnPosition newTurnPosition = Instantiate(CharacterTurnPosition);
            newTurnPosition.transform.SetParent(transform, false);
            turnPositions.Add(unit, newTurnPosition);
            turnPositions[unit].SetPortrait(unit.Portrait);

        }
        Debug.Log(transform.childCount);
        transform.GetChild(transform.childCount - 1).SetAsFirstSibling();
    }
}
