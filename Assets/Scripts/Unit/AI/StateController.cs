#region Using Statements
using SP.Turns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

namespace SP.Characters
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

        //public void UpdateStateMachine()
        //{
        //    StartCoroutine(UpdateEnemyStateMachines());
        //}

        #endregion

        #region Coroutines
        IEnumerator UpdateEnemyStateMachines()
        {
            List<StateMachine> stateMachines = unitController.GetStateMachines();
            for (int i = 0; i < stateMachines.Count; i++)
            {
                yield return StartCoroutine(stateMachines[i].UpdateState());
            }
            //foreach (StateMachine stateMachine in stateMachines)
            //{
            //    Debug.Log("update enemy state machine");
                
            //}
            turnController.UpdatePhase();
        }
        #endregion

        private void TurnController_OnEnemyTurn()
        {
            Debug.Log("Start state machine");
            StartCoroutine(UpdateEnemyStateMachines());
        }
    }
}