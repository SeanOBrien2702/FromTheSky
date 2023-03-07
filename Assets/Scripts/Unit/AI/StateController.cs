#region Using Statements
using FTS.Turns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

namespace FTS.Characters
{
    public class StateController : MonoBehaviour
    {
        UnitController unitController;
        TurnController turnController;

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
        }

        private void OnDestroy()
        {
            TurnController.OnEnemyTurn -= TurnController_OnEnemyTurn;
        }
        #endregion

        #region Public Methods

        public void UpdateStateMachine()
        {
            StartCoroutine(UpdateEnemyStateMachines());
        }

        #endregion

        #region Coroutines
        IEnumerator UpdateEnemyStateMachines()
        {     
            List<StateMachine> stateMachines = unitController.GetStateMachines();
            //Debug.Log("Start state machine " + stateMachines.Count);
            foreach (StateMachine machine in stateMachines)
            {
                yield return StartCoroutine(machine.UpdateState());
            }

            turnController.UpdatePhase();
        }
        #endregion

        private void TurnController_OnEnemyTurn(bool isTelegraph)
        {
            StartCoroutine(UpdateEnemyStateMachines());
        }
    }
}