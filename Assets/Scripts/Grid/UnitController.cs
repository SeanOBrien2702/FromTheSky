#region Using Statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Grid;
using FTS.Turns;
using FTS.UI;
using FTS.Cards;
using FTS.Core;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
#endregion

namespace FTS.Characters
{
    public class UnitController : MonoBehaviour
    {
        public static event System.Action<Character> OnEnemyKilled = delegate { };


        HexGrid grid;
        HexGridController gridController;
        PlayerDatabase playerDatabase;
        EnemyDatabase enemyDatabase;
        TurnController turnController;
        CardController cardController;
        ObjectiveController objectiveController;
        List<Character> enemyList = new List<Character>();
        List<StateMachine> stateMachines = new List<StateMachine>();
        List<Player> playerList = new List<Player>();
        Character vehicle;
        [Range(1, 10)]
        [SerializeField] int enemiesToSpawn = 5;

        [SerializeField] FTS.UI.CharacterInfo characterInfo;

        [Header("Unit prefabs")]
        [SerializeField] Character vehiclePrefab;

        bool actionDone = false;
        int numberOfEnemies;
        int numberOfPlayers;
        int numberOfUnits;
        Character currentUnit;
        Player currentPlayer;
        private int currentIndex = 0;
        float timeSinceChangingUnits = 0;
        float changeCooldown = 0.5f;

        public static event System.Action OnPlayerSelected = delegate { };

        #region Properties

        public int NumberOfPlayers   // property
        {
            get { return numberOfPlayers; }   // get method
        }

        public int NumberOfUnits   // property
        {
            get { return numberOfUnits; }   // get method
            set { numberOfUnits = value; }  // set method
        }
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

        public Character Vehicle
        {
            get { return vehicle; }
            //get { return playerList[CurrentIndex]; }
        }


        #endregion

        #region MonoBehaviour Callbacks
        void Awake()
        {
            grid = GetComponent<HexGrid>();
            gridController = GetComponent<HexGridController>();
            objectiveController = FindObjectOfType<ObjectiveController>().GetComponent<ObjectiveController>();
            playerDatabase = FindObjectOfType<PlayerDatabase>().GetComponent<PlayerDatabase>();
            enemyDatabase = FindObjectOfType<EnemyDatabase>().GetComponent<EnemyDatabase>();
            cardController = FindObjectOfType<CardController>().GetComponent<CardController>();
            turnController = FindObjectOfType<TurnController>().GetComponent<TurnController>();
            TurnController.OnNewTurn += TurnController_OnNewTurn;
        }

        private void Start()
        {
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                CreateUnitInRandomPosition(GetRandomCharacter());
            }
            //CreateUnit(vehiclePrefab, grid.VehicleStart);
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
            TurnController.OnNewTurn -= TurnController_OnNewTurn;
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
        private void UpdateTurnOrder()
        {
            enemyList.Sort(delegate (Character x, Character y)
            {
                return x.Initiative.CompareTo(y.Initiative);
            });
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

            if (character is Player)
            {
                if (character.CharacterClass == CharacterClass.Vehicle)
                {
                    vehicle = newCharacter as Player;
                }
                else
                {
                    playerList.Add(newCharacter as Player);
                    ++numberOfPlayers;
                }
            }
            else
            {
                enemyList.Add(newCharacter);
                stateMachines.Add(newCharacter.GetComponent<StateMachine>());
                ++numberOfEnemies;
                UpdateTurnOrder();
            }

        }

        public void SetCurrentUnit(Player player)
        {
            currentPlayer = player;
            if (player != null)
            {
                OnPlayerSelected?.Invoke();
                characterInfo.EnableUI(player);
            }
        }

        internal void SelectVehicle()
        {
            currentPlayer = vehicle as Player;
            OnPlayerSelected?.Invoke();
        }

        internal void PlacePlayer(HexCell cell)
        {
            //TODO: get player from list of chosen characters not database of all characters
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

        public void RemoveUnit(Character character)
        {
            Debug.Log("unit removed");
            --numberOfUnits;
            if (character is Enemy)
            {
                Debug.Log("Enemy killed");
                //gridController.RemoveIndicators(character);
                OnEnemyKilled?.Invoke(character);
                //objectiveController.UpdateObjective();
                stateMachines.Remove(character.GetComponent<StateMachine>());
                enemyList.Remove(character);
                UpdateTurnOrder();
            }
            else
            {           
                --numberOfPlayers;
                playerList.Remove(character as Player);
                cardController.RemoveClass(character.CharacterClass);
                if (playerList.Count <= 0 && gridController.UnitsPlaced)
                {
                    Debug.Log("Game over");
                    SceneManager.LoadScene("MainMenu");
                }
            }

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

        internal HexCell GetVehiclePosition()
        {
            return vehicle.GetComponent<Mover>().Location;
        }

        public void UpdateStateMachine()
        {
            //StartCoroutine(UpdateEnemyStateMachines());
        }

        internal List<StateMachine> GetStateMachines()
        {
            return stateMachines;
        }
        #endregion

        #region Coroutines
        IEnumerator UpdateEnemyStateMachines()
        {
            foreach (StateMachine stateMachine in stateMachines)
            {
                actionDone = false;
                stateMachine.UpdateState();
                yield return new WaitUntil(() => actionDone == true || Input.GetKeyDown(KeyCode.N));
            }
            turnController.UpdatePhase();
        }
        #endregion

        #region Events
        private void TurnController_OnNewTurn()
        {
            currentPlayer = playerList.First();
        }
        #endregion
    }
}
