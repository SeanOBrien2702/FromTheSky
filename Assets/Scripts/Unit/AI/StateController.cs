#region Using Statements
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
        }

        private void OnDestroy()
        {
            TurnController.OnEnemyTurn -= TurnController_OnEnemyTurn;
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

        private void TurnController_OnEnemyTurn(bool isTelegraph)
        {
            StartCoroutine(UpdateEnemyStateMachines());
        }

        internal int GetTurnOrder(StateMachine machine)
        {
            return stateMachines.FindIndex(item => item == machine) + 1;
        }
    }
}