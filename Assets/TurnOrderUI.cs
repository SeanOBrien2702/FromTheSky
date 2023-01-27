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
        //UnitController.OnUnitTurn += UnitController_OnUnitTurn;
        UnitController.OnEnemyKilled += UnitController_OnEnemyKilled;
        UnitController.OnPlayerKilled += UnitController_OnPlayerKilled;
    }

    private void UnitController_OnPlayerKilled(Character player)
    {
        Destroy(turnPositions[player].gameObject);
        turnPositions.Remove(player);
    }

    private void UnitController_OnEnemyKilled(Character enemy)
    {
        //Destroy(turnPositions[enemy].gameObject);
       // turnPositions.Remove(enemy);
    }

    private void OnDestroy()
    {
        TurnController.OnCombatStart -= TurnController_OnCombatStart;
        //UnitController.OnUnitTurn -= UnitController_OnUnitTurn;
        UnitController.OnEnemyKilled -= UnitController_OnEnemyKilled;
        UnitController.OnPlayerKilled -= UnitController_OnPlayerKilled;
    }
    private void TurnController_OnCombatStart()
    {
        //
    }

    private void UnitController_OnUnitTurn(Character obj)
    {
        //Debug.Log("turn order ui " + transform.GetChild(0).name);
        transform.GetChild(0).transform.SetAsLastSibling();
        int index = 1;
        foreach (Transform item in transform)
        {
            item.GetComponent<CharacterTurnPosition>().SetPositionText(index);
            index++;
        }
    }

    internal void FillUI()
    {
        List<Character> units = unitController.GetUnits();

        foreach (var unit in units)
        {
            CharacterTurnPosition newTurnPosition = Instantiate(CharacterTurnPosition);
            newTurnPosition.transform.SetParent(transform, false);
            turnPositions.Add(unit, newTurnPosition);
            turnPositions[unit].SetPortrait(unit.Portrait);
            //Debug.Log("unit: " + unit.gameObject.name);
        }
        transform.GetChild(transform.childCount - 1).SetAsFirstSibling();
    }
}
