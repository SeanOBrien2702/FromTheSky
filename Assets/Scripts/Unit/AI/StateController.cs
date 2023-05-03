#region Using Statements
using FTS.Core;
using FTS.Turns;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#endregion

namespace FTS.Characters
{
    public class StateController : MonoBehaviour
    {
        UnitController unitController;
        TurnController turnController;
        List<StateMachine> stateMachines = new List<StateMachine>();
        bool areActionsConplete = false;
        #region Properties
        public bool ActionDone  // property
        {
            get { return areActionsConplete; }   // get method
            set { areActionsConplete = value; }  // set method
        }
        #endregion

        #region MonoBehaviour Callbacks
        void Start()
        {
            unitController = GetComponent<UnitController>();
            turnController = FindObjectOfType<TurnController>().GetComponent<TurnController>();
            TurnController.OnEnemyTurn += TurnController_OnEnemyTurn;
            ObjectiveController.OnPlayerWon += ObjectiveController_OnPlayerWon;
            UnitController.OnPlayerLost += UnitController_OnPlayerLost;
            RunController.OnPlayerLost += RunController_OnPlayerLost;
        }

        private void OnDestroy()
        {
            TurnController.OnEnemyTurn -= TurnController_OnEnemyTurn;
            ObjectiveController.OnPlayerWon -= ObjectiveController_OnPlayerWon;
            UnitController.OnPlayerLost -= UnitController_OnPlayerLost;
            RunController.OnPlayerLost -= RunController_OnPlayerLost;
        }
        #endregion

        #region Public Methods

        public void UpdateStateMachine()
        {
            //StartCoroutine(UpdateEnemyStateMachines());
        }

        #endregion

        #region Coroutines
        IEnumerator UpdateEnemyStateMachines()
        {     
            stateMachines = unitController.GetStateMachines();
            foreach (StateMachine machine in stateMachines.ToList())
            {
                yield return StartCoroutine(machine.UpdateState());
            }

            turnController.UpdatePhase();
        }
        #endregion

        #region Events
        private void TurnController_OnEnemyTurn(bool isTelegraph)
        {
            StartCoroutine(UpdateEnemyStateMachines());
        }

        internal int GetTurnOrder(StateMachine machine)
        {
            return stateMachines.FindIndex(item => item == machine) + 1;
        }

        private void ObjectiveController_OnPlayerWon()
        {
            StopAllCoroutines();
        }

        private void RunController_OnPlayerLost()
        {
            StopAllCoroutines();
        }

        private void UnitController_OnPlayerLost()
        {
            StopAllCoroutines();
        }
        #endregion
    }
}