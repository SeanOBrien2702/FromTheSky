#region Using Statements
using FTS.Characters;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#endregion

namespace FTS.Turns
{
    public class TurnController : MonoBehaviour
    {
        //UnitController unitController;
        StateController stateController;
        int turnOrderPosition = 0;
        [SerializeField] int turn = 0;
        [SerializeField] Button endTurnButton;
        [SerializeField] TextMeshProUGUI turnText;
        [SerializeField] Text turnInfoText;
        bool hasCombatStarted = false;
        TurnPhases turnPhase;

        public static event System.Action OnNewTurn = delegate { };
        public static event System.Action OnEndTurn = delegate { };
        public static event System.Action OnEnemyTurn = delegate { };
        public static event System.Action OnCombatStart = delegate { };

        #region Properties
        public TurnPhases TurnPhase   // property
        {
            get { return turnPhase; }   // get method
            set { turnPhase = value; }  // set method
        }

        public int Turn   // property
        {
            get { return turn; }   // get method
            set { turn = value; }  // set method
        }
        public int TurnOrder  // property
        {
            get { return turnOrderPosition; }   // get method
        }
        #endregion

        #region MonoBehaviour Callbacks
        void Start()
        {
            //unitController = FindObjectOfType<UnitController>().GetComponent<UnitController>();
            //stateController = FindObjectOfType<StateController>().GetComponent<StateController>();
            turnPhase = TurnPhases.Placement;
            turnInfoText.text = "Place Units";
        }

        void Update()
        {
            if (Input.GetKeyDown("space") && hasCombatStarted)
            {
                UpdatePhase();
            }
        }
        #endregion

        #region Private Methods
        private void EnemyTelegraph()
        {
            OnEnemyTurn?.Invoke();
            turnPhase = TurnPhases.EnemyTelegraph;
            //stateController.UpdateStateMachine();
            turnInfoText.text = "Enemy planning";
        }

        private void PlayerTurn()
        {
            NewTurn();
            //Debug.Log("player turn starts");
            turnPhase = TurnPhases.PlayerTurn;
            turnInfoText.text = "Player Turn";
        }

        private void Enviorment()
        {
            
            turnPhase = TurnPhases.Environment;
            turnInfoText.text = "Environment Turn";
        }

        private void EnemyAction()
        {
            Debug.Log("enemy turn starts");
            turnPhase = TurnPhases.EnemyActions;
            OnEnemyTurn?.Invoke();
            //stateController.UpdateStateMachine();
            turnInfoText.text = "Enemy actions"; 
            EndTurn();
        }

        private void VehicleAction()
        {
            EndTurn();
            turnPhase = TurnPhases.VehicleAction;
            //unitController.SelectVehicle();
            turnInfoText.text = "Vehicle turn";
        }

        private void NewTurn()
        {
            //Debug.Log("New turn");
            OnNewTurn?.Invoke();
            turn++;
            turnText.text = "Turn: " + turn.ToString();
        }

        private void EndTurn()
        {
            //Debug.Log("END TURN?");
            OnEndTurn?.Invoke();
            //turn++;
            //turnText.text = "Turn: " + turn.ToString();
        }
        #endregion

        #region Public Methods

        //end turn button
        public void EndRound()
        {
            UpdatePhase();
        }

        public void UpdatePhase()
        {
            switch (turnPhase)
            {
                case TurnPhases.Placement:
                    //EnemyTelegraph();
                    PlayerTurn();
                    break;
                //case TurnPhases.EnemyTelegraph:
                //    PlayerTurn();
                //    break;
                case TurnPhases.PlayerTurn:
                    //Enviorment();
                    EnemyAction();
                    break;
                case TurnPhases.Environment:
                    EnemyAction();
                    break;
                case TurnPhases.EnemyActions:
                    //VehicleAction();
                    PlayerTurn();
                    break;
                //case TurnPhases.VehicleAction:
                //    EnemyTelegraph();
                //    break;
                default:
                    break;
            }
        }

        public void StartPlayerTurn()
        {
            PlayerTurn();
        }

        public void StartEnemyTurn()
        {
            PlayerTurn();
        }

        //start button
        internal void StartCombat()
        {
            //EnemyTelegraph();
            OnCombatStart?.Invoke();
            PlayerTurn();
            hasCombatStarted = true;
        }
        #endregion
    }
}
