using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using FTS.Characters;
using FTS.Cards;
using FTS.UI;
using System.Linq;
using FTS.Turns;
using Bayat.Json.Utilities;

namespace FTS.Core
{
    public class ObjectiveController : MonoBehaviour
    {
        public static event System.Action OnPlayerWon = delegate { };
        ObjectiveDatabase objectiveDatabase;
        ObjectiveUIController objectiveUI;
        List<Objective> objectives = new List<Objective>();
        [SerializeField] List<Objective> testingObjectives;

        void Start()
        {
            objectiveUI = FindObjectOfType<ObjectiveUIController>().GetComponent<ObjectiveUIController>();
            objectiveDatabase = FindObjectOfType<ObjectiveDatabase>().GetComponent<ObjectiveDatabase>();

            objectives = objectiveDatabase.GetObjectives();

            if(objectives.Count == 0)
            {
                objectives.AddRange(testingObjectives);
            }

            if (objectives[0] is SurviveObjective)
            {
                TurnController.OnEnemySpawn += TurnController_OnNewTurn;
            }
            UnitController.OnPlayerSpawned += UnitController_OnPlayerSpawned;
            UnitController.OnPlayerKilled += UnitController_OnPlayerKilled;
            CardController.OnCardPlayed += CardController_OnCardPlayed;
            UnitController.OnEnemyKilled += UnitController_OnEnemyKilled;
            UnitController.OnDamageTaken += UnitController_OnDamageTaken;
            foreach (var objective in objectives)
            {
                objective.IsComplete = false;
                objective.EnableObjective();
            }
            objectiveUI.CreateObjectiveText(objectives);
        }

        private void OnDestroy()
        {
            if (objectives[0] is SurviveObjective)
            {
                TurnController.OnEnemySpawn -= TurnController_OnNewTurn;
            }
            UnitController.OnPlayerSpawned -= UnitController_OnPlayerSpawned;
            UnitController.OnPlayerKilled -= UnitController_OnPlayerKilled;
            CardController.OnCardPlayed -= CardController_OnCardPlayed;
            UnitController.OnEnemyKilled -= UnitController_OnEnemyKilled;
            UnitController.OnDamageTaken -= UnitController_OnDamageTaken;
        }

        internal void UpdateObjective()
        {
            foreach (var objective in objectives)
            {
                objective.UpdateObjective();
            }
            CheckObjectives();
        }

        void CheckObjectives()
        {
            foreach (var objective in objectives)
                objectiveUI.UpdateUI(objective);

            if (objectives[0].IsComplete)
            {
                if (objectives[1].IsComplete)
                {
                    RunController.Instance.Cinder += 50;
                }
                OnPlayerWon?.Invoke();
                SceneController.Instance.LoadScene(Scenes.DraftScene, true);
            }
        }

        private void UnitController_OnEnemyKilled(Character enemy)
        {
            foreach (var objective in objectives.FindAll(item => item is KillObjective))
            {
                objective.UpdateObjective((Enemy)enemy);
                
            }
            CheckObjectives();
        }

        private void CardController_OnCardPlayed(Card playedCard, Player player)
        {
            foreach (var objective in objectives.FindAll(item => item is CardObjective))
            {
                objective.UpdateObjective(playedCard);
            }
            CheckObjectives();
        }

        private void TurnController_OnNewTurn()
        {
            foreach (var objective in objectives.FindAll(item => item is SurviveObjective))
            {
                objective.UpdateObjective();
            }
            CheckObjectives();
        }

        private void UnitController_OnPlayerKilled(Character obj)
        {
            foreach (var objective in objectives.FindAll(item => item is UnitObjective))
            {
                objective.UpdateObjective();
            }
            CheckObjectives();
        }

        private void UnitController_OnPlayerSpawned(Character obj)
        {
            foreach (var objective in objectives.FindAll(item => item is UnitObjective))
            {
                objective.UpdateObjective();
            }
            CheckObjectives();
        }

        private void UnitController_OnDamageTaken(Unit unit, int damage)
        {
            if (unit is Enemy)
            {
                return;
            }
            foreach (var objective in objectives.FindAll(item => item is DamageObjective))
            {
                objective.UpdateObjective(damage);
            }
            CheckObjectives();
        }
    }
}
