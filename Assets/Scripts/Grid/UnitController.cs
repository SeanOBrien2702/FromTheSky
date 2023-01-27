#region Using Statements
using FTS.Cards;
using FTS.Grid;
using FTS.Turns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
#endregion

namespace FTS.Characters
{
    public class UnitController : MonoBehaviour
    {
        public static event Action<Character> OnEnemyKilled = delegate { };
        public static event Action<Character> OnPlayerKilled = delegate { };

        HexGrid grid;
        HexGridController gridController;
        PlayerDatabase playerDatabase;
        EnemyDatabase enemyDatabase;
        TurnController turnController;
        CardController cardController;
        StateController stateController;
        List<Character> enemyList = new List<Character>();
        List <StateMachine> stateMachines = new List<StateMachine>();
        List<Character> units = new List<Character>();
        List<Player> playerList = new List<Player>();
        [Range(1, 10)]
        [SerializeField] int enemiesToSpawn = 5;

        [SerializeField] UI.CharacterInfo characterInfo;

        [Header("Unit prefabs")]
        [SerializeField] Character vehiclePrefab;

        int numberOfPlayers;
        int numberOfUnits;
        Character currentUnit;
        Player currentPlayer;
        private int currentIndex = 0;
        private int currentUnitIndex = 0;
        float timeSinceChangingUnits = 0;
        float changeCooldown = 0.5f;

        public static event System.Action OnPlayerSelected = delegate { };
       //public static event Action<Character> OnUnitTurn = delegate { };

        #region Properties
        public int NumberOfPlayers { get => numberOfPlayers; set => numberOfPlayers = value; }

        public int NumberOfUnits { get => numberOfUnits; set => numberOfUnits = value; }

        public int CurrentIndex
        {
            get
            {
                if (currentIndex > playerList.Count - 1)
                {
                    currentIndex = 0;
                }
                if (currentIndex < 0)
                {
                    currentIndex = playerList.Count - 1;
                }
                return currentIndex;
            }
            set { currentIndex = value; }
        }
        public Player NextPlayer
        {
            get { currentIndex++; return playerList[CurrentIndex]; }
        }

        public Player PreviousPlayer
        {
            get { currentIndex--; return playerList[CurrentIndex]; }
        }

        public Player CurrentPlayer
        {
            get { return currentPlayer; }
        }

        public int CurrentUnitIndex
        {
            get
            {
                if (currentUnitIndex > units.Count - 1)
                {
                    currentUnitIndex = 0;
                }
                if (currentUnitIndex < 0)
                {
                    currentUnitIndex = units.Count - 1;
                }
                return currentUnitIndex;
            }
            set { currentUnitIndex = value; }
        }
        public Character NextUnit
        {
            get { currentUnitIndex++; return units[CurrentUnitIndex]; }
        }

        public Character PreviousUnit
        {
            get { currentUnitIndex--; return units[CurrentUnitIndex]; }
        }

        public Character CurrentUnit
        {
            get { return currentUnit; }
        }
        #endregion

        #region MonoBehaviour Callbacks
        void Awake()
        {
            grid = GetComponent<HexGrid>();
            gridController = GetComponent<HexGridController>();
            stateController = GetComponent<StateController>();
            playerDatabase = FindObjectOfType<PlayerDatabase>().GetComponent<PlayerDatabase>();
            enemyDatabase = FindObjectOfType<EnemyDatabase>().GetComponent<EnemyDatabase>();
            cardController = FindObjectOfType<CardController>().GetComponent<CardController>();
            turnController = FindObjectOfType<TurnController>().GetComponent<TurnController>();
            TurnController.OnPlayerTurn += TurnController_OnNewTurn;
            TurnController.OnCombatStart += TurnController_OnCombatStart;
        }

        private void Start()
        {
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                CreateUnitInRandomPosition(GetRandomCharacter());
            }
        }

        private void Update()
        {
            if (turnController.TurnPhase == TurnPhases.PlayerTurn)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) && timeSinceChangingUnits >= changeCooldown)
                {
                    SetCurrentUnit(PreviousPlayer);
                    timeSinceChangingUnits = 0;
                }

                if (Input.GetKeyDown(KeyCode.Tab) && timeSinceChangingUnits >= changeCooldown)
                {
                    SetCurrentUnit(NextPlayer);
                    timeSinceChangingUnits = 0;
                }
                timeSinceChangingUnits += Time.deltaTime;
            }
        }

        private void OnDestroy()
        {
            TurnController.OnPlayerTurn -= TurnController_OnNewTurn;
            TurnController.OnCombatStart -= TurnController_OnCombatStart;
        }
        #endregion

        #region Private Methods
        private Character GetRandomCharacter()
        {
            return enemyDatabase.GetRandomEnemy();
        }

        void CreateUnitInRandomPosition(Character newCharacter)
        {
            HexCell[] cells = grid.GetGrid();
            HexCell cell;
            do
            {
                cell = cells[UnityEngine.Random.Range(grid.Width * 4, cells.Length)];
            } while (cell.Unit != null || cell.IsObstacle);
            CreateUnit(newCharacter, cell);
        }

        internal bool IsPlayer()
        {
            bool isPlayer = currentUnit is Player ? true : false;
            return isPlayer;
        }

        private void UpdateTurnOrder()
        {
            units = units.OrderByDescending(x => x.Initiative).ToList();
        }
        #endregion

        #region Public Methods
        public void CreateUnit(Character character, HexCell cell)
        {
            Character newCharacter = Instantiate(character);
            newCharacter.transform.SetParent(transform, false);
            newCharacter.GetComponent<Mover>().Location = cell;
            cell.Unit = newCharacter;
            newCharacter.RollInitive();
            newCharacter.CreateHeathBar();

            ++numberOfUnits;

            units.Add(newCharacter);

            if (newCharacter is Enemy)
            {
                enemyList.Add(newCharacter);
                stateMachines.Add(newCharacter.GetComponent<StateMachine>());
            }
            else
            {
                playerList.Add(newCharacter as Player);
                ++numberOfPlayers;
            }
            UpdateTurnOrder();
        }


        public void StartTurn()
        {
            currentUnit = NextUnit;
            //OnUnitTurn?.Invoke(currentUnit);
            if (currentUnit is Player)
            {
                characterInfo.EnableUI(currentUnit);
            }
        }


        public void SetCurrentUnit(Player player)
        {
            currentPlayer = player;
            OnPlayerSelected?.Invoke();
            if (player != null)
            {
                characterInfo.EnableUI(player);
            }
        }

        internal void PlacePlayer(HexCell cell)
        {
            currentPlayer = playerDatabase.GetUnplacedCharacter(playerList);
            if (currentPlayer != null)
            {
                CreateUnit(currentPlayer, cell);
                currentPlayer = NextPlayer;
            }
        }

        internal void RemovePlayer(Player unit)
        {
            currentPlayer = unit;
        }

        internal Player GetCurrentPlayer()
        {
            return currentPlayer;
        }

        internal Character GetCurrentUnit()
        {
            return currentUnit;
        }

        public void RemoveUnit(Character character)
        {
            --numberOfUnits;
            if (character is Enemy)
            {
                OnEnemyKilled?.Invoke(character);
                stateMachines.Remove(character.GetComponent<StateMachine>());
                enemyList.Remove(character);
            }
            else
            {
                --numberOfPlayers;
                playerList.Remove(character as Player);
                cardController.RemoveClass(character.CharacterClass);
                OnPlayerKilled?.Invoke(character);
                if (playerList.Count <= 0 && gridController.UnitsPlaced)
                {
                    SceneManager.LoadScene("MainMenu");
                }
            }
            units.Remove(character);
        }

        public Player GetPlayerByClass(CharacterClass characterClass)
        {
            return playerList.Find(item => item.CharacterClass == characterClass);
        }

        public List<Player> GetPlayerUnits()
        {
            return playerList;
        }

        public List<Character> GetEnemyUnits()
        {
            return enemyList;
        }

        public List<Character> GetUnits()
        {
            return units;
        }

        public CharacterClass[] GetPlayerClasses()
        {
            CharacterClass[] classList = new CharacterClass[numberOfPlayers];
            int index = 0;
            foreach (var item in playerList)
            {
                classList[index] = item.CharacterClass;
                ++index;
            }
            return classList;
        }
        internal List<StateMachine> GetStateMachines()
        {
            return stateMachines;
        }
        #endregion

        #region Events
        private void TurnController_OnNewTurn()
        {
            currentPlayer = playerList.First();
            currentUnit = currentPlayer;
        }

        private void TurnController_OnCombatStart()
        {
            currentUnit = enemyList.First();
        }
        #endregion
    }
}
