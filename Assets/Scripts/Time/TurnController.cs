#region Using Statements
using FTS.Characters;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#endregion

namespace FTS.Turns
{
    public class TurnController : MonoBehaviour
    {
        UnitController unitController;
        StateController stateController;
        int turnOrderPosition = 0;
        [SerializeField] int turn = 0;
        [SerializeField] Button endTurnButton;
        [SerializeField] TextMeshProUGUI turnText;
        [SerializeField] Text turnInfoText;
        [SerializeField] TurnOrderUI turnOrderUI;
        bool hasCombatStarted = false;
        TurnPhases turnPhase;

        public static event System.Action OnCombatStart = delegate { };
        public static event System.Action<bool> OnEnemyTurn = delegate { };
        public static event System.Action OnPlayerTurn = delegate { };
        //public static event System.Action OnEnemyAction = delegate { };

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
            unitController = FindObjectOfType<UnitController>().GetComponent<UnitController>();
            turnPhase = TurnPhases.Placement;
            turnInfoText.text = "Place Units";
        }
        #endregion

        #region Private Methods
        private void EnemyTelegraph()
        {
            OnEnemyTurn?.Invoke(true);
            turnInfoText.text = "Enemy planning";
        }

        private void PlayerTurn()
        {
            NewTurn();
            turnInfoText.text = "Player Turn";
        }

        private void EnemyAction()
        {
            OnEnemyTurn?.Invoke(false);
            turnInfoText.text = "Enemy actions"; 
        }

        private void NewTurn()
        {
            OnPlayerTurn?.Invoke();
            turn++;
            turnText.text = "Turn: " + turn.ToString();
        }
        #endregion

        #region Public Methods
        //end turn button
        public void EndRound()
        {
            if(turnPhase == TurnPhases.PlayerTurn)
                UpdatePhase();
        }

        public void UpdatePhase()
        {
            turnPhase++;
            if (turnPhase > TurnPhases.EnemyActions)
            {
                turnPhase = TurnPhases.EnemyTelegraph;
            }
            switch (turnPhase)
            {
                case TurnPhases.EnemyTelegraph:
                    EnemyTelegraph();
                    break;
                case TurnPhases.PlayerTurn:
                    PlayerTurn();              
                    break;
                case TurnPhases.Environment:
                    UpdatePhase();
                    break;
                case TurnPhases.EnemyActions:
                    EnemyAction();
                    break;
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
            OnCombatStart?.Invoke();
            turnOrderUI.FillUI();
            UpdatePhase();
            hasCombatStarted = true;
        }
        #endregion
    }
}
