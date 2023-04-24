#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FTS.Grid;
using FTS.Turns;
using FTS.UI;
using WalldoffStudios.Indicators;
#endregion

namespace FTS.Characters
{
    public class StateMachine : MonoBehaviour
    {
        public State currentState;
        public State remainState;
        StateController stateController;
        [HideInInspector] public CameraController cameraController;
        IndicatorController indicator;

        [HideInInspector] public TelegraphIntentUI telegraphIntentUI;      
        [HideInInspector] public Mover mover;
        [HideInInspector] public Enemy enemy;
        [HideInInspector] public HexGridController gridController;
        [HideInInspector] public AttackIndicatorController attackIndicator;
        [HideInInspector] public UnitController unitController;
        [HideInInspector] public TurnController turnController;
        //[HideInInspector] public Unit Target;
        [HideInInspector] public HexCell newEnemyPosition;

        #region Properties
        public int AttackRange
        {          
            get { return enemy.Stats.GetStat(Stat.AttackRange, enemy.CharacterClass); }
        }

        public EnemyTargeting Targeting
        {
            get { return enemy.Targeting; }
        }
        #endregion

        #region MonoBehaviour Callbacks
        void Awake()
        {
            turnController = FindObjectOfType<TurnController>().GetComponent<TurnController>();
            gridController = FindObjectOfType<HexGridController>().GetComponent<HexGridController>();
            attackIndicator = FindObjectOfType<AttackIndicatorController>().GetComponent<AttackIndicatorController>();
            unitController = FindObjectOfType<UnitController>().GetComponent<UnitController>();
            stateController = FindObjectOfType<StateController>().GetComponent<StateController>();
            cameraController = FindObjectOfType<CameraController>().GetComponent<CameraController>();
            enemy = GetComponent<Enemy>();
            mover = GetComponent<Mover>();
            indicator = enemy.Indicator;
            telegraphIntentUI = enemy.IntentUI;
        }
        #endregion

        #region Private Methods
        private void OnExitState()
        {
            
        }
        #endregion

        #region Public Methods
        public void TransitionToState(State nextState)
        {
            if (nextState != remainState)
            {
                currentState = nextState;
                OnExitState();
            }
        }
        #endregion

        #region Coroutines
        public IEnumerator UpdateState()
        {
            //Debug.Log("old state: "+currentState);
            currentState.CheckTransitions(this);
            //Debug.Log("new state: " +currentState);
            yield return StartCoroutine(currentState.DoActions(stateController, this, cameraController));
            //Debug.Log("action complete");

            
        }
        #endregion
    }
}