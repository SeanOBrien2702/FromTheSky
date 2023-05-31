#region Using Statements
using FTS.Core;
using FTS.Grid;
using FTS.Turns;
using HighlightPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#endregion

namespace FTS.Characters
{
    public class UnitController : MonoBehaviour
    {
        public static event Action<Character> OnEnemyKilled = delegate { };
        public static event Action<Enemy> OnEnemyStunned = delegate { };
        public static event Action<Character> OnPlayerKilled = delegate { };
        public static event Action<Character> OnPlayerSpawned = delegate { };

        public static event Action<Unit, int> OnDamageTaken = delegate { };
        public static event Action<Player, int> OnEnergyChanged = delegate { };
        public static event Action<Unit, int> OnMovementChanged = delegate { };

        public static event Action<Unit> OnSelectUnit = delegate { };
        public static event Action<Player> OnSelectPlayer = delegate { };

        public static event System.Action OnPlayerLost = delegate { };
        public static event System.Action OnEnemyLost = delegate { };

        HexGrid grid;
        HexGridController gridController;
        PlayerDatabase playerDatabase;
        EnemyDatabase enemyDatabase;
        TurnController turnController;
        List<Character> enemyList = new List<Character>();
        List <StateMachine> stateMachines = new List<StateMachine>();
        List<Unit> units = new List<Unit>();
        List<Unit> targetableUnits = new List<Unit>();
        
        List<Player> playerList = new List<Player>();
        [Range(1, 10)]

        [Header("Buildings")]
        [SerializeField] int minSpawn = 2;
        [SerializeField] int maxSpawn = 4;
        [SerializeField] Building building;

        int numberOfPlayers;
        int numberOfUnits;
        Unit currentUnit;
        private int currentIndex = 0;
        private int currentUnitIndex = 0;
        float timeSinceChangingUnits = 0;
        float changeCooldown = 0.5f;

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
            get {
                if (currentUnit is Player)
                {
                    return (Player)currentUnit;
                }
                return null;
            }
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
        public Unit NextUnit
        {
            get { currentUnitIndex++; return units[CurrentUnitIndex]; }
        }

        public Unit PreviousUnit
        {
            get { currentUnitIndex--; return units[CurrentUnitIndex]; }
        }

        public Unit CurrentUnit
        {
            get { return currentUnit; }
        }
        #endregion

        #region MonoBehaviour Callbacks
        void Awake()
        {
            grid = GetComponent<HexGrid>();
            gridController = GetComponent<HexGridController>();
            playerDatabase = FindObjectOfType<PlayerDatabase>().GetComponent<PlayerDatabase>();
            enemyDatabase = FindObjectOfType<EnemyDatabase>().GetComponent<EnemyDatabase>();
            turnController = FindObjectOfType<TurnController>().GetComponent<TurnController>();
            TurnController.OnPlayerTurn += TurnController_OnNewTurn;
            TurnController.OnCombatStart += TurnController_OnCombatStart;
        }

        private void Start()
        {
            SpawnBuildings();       
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
            if (Application.isEditor)
            {
                if (Input.GetKeyDown(KeyCode.O))
                {
                    OnPlayerLost?.Invoke();
                }
            }
        }

        private void OnDestroy()
        {
            TurnController.OnPlayerTurn -= TurnController_OnNewTurn;
            TurnController.OnCombatStart -= TurnController_OnCombatStart;
        }
        #endregion

        #region Private Methods
        public Character GetRandomCharacter()
        {
            return enemyDatabase.GetRandomEnemy();
        }

        /*
        * FUNCTION    : SpawnBuildings
        * DESCRIPTION : Set position of buildings
        * PARAMETERS  : void
        * RETURNS     : void
        */
        private void SpawnBuildings()
        {
            List<HexCell> spawnLocations = grid.GetRandomPosition(UnityEngine.Random.Range(minSpawn, maxSpawn));

            foreach (HexCell cell in spawnLocations)
            {
                if (!cell.Unit)
                {
                    CreateUnit(building, cell);
                }
            }
        }
        internal bool IsPlayer()
        {
            bool isPlayer = currentUnit is Player ? true : false;
            return isPlayer;
        }

        private void UpdateTurnOrder()
        {
            //units = units.OrderByDescending(x => x.Initiative).ToList();
        }
        #endregion

        #region Public Methods
        public void CreateUnit(Unit character, HexCell cell)
        {
            Unit newCharacter = Instantiate(character);
            newCharacter.transform.SetParent(transform, false);
            if(newCharacter is Building)
            {
                newCharacter.Location = cell;
            }
            else
            {
                newCharacter.GetComponent<Mover>().Location = cell;
            }

            
            cell.Unit = newCharacter;
            //newCharacter.RollInitive();
            //newCharacter.CreateHeathBar();

            ++numberOfUnits;

            units.Add(newCharacter);

            if (newCharacter is Enemy)
            {
                newCharacter.IsFriendly = false;
                enemyList.Add((Character)newCharacter);
                stateMachines.Add(newCharacter.GetComponent<StateMachine>());
            }
            else if(newCharacter is Player)
            {
                ++numberOfPlayers;
                targetableUnits.Add(newCharacter);
                playerList.Add(newCharacter as Player);
                OnPlayerSpawned?.Invoke(newCharacter as Player);          
            }
            else
            {
                targetableUnits.Add(newCharacter);
            }
            UpdateTurnOrder();
        }

        public void StartTurn()
        {
            currentUnit = NextUnit;
        }

        public void SetCurrentUnit(Unit unit)
        {
            currentUnit = unit;
            if(unit is Player)
            {
                OnSelectPlayer?.Invoke(unit as Player);
            }
            else
            {
                OnSelectUnit?.Invoke(currentUnit);
            }
        }

        internal Unit PlacePlayer(HexCell cell)
        {
            currentUnit = playerDatabase.GetUnplacedCharacter(playerList);
            if (currentUnit != null)
            {
                CreateUnit(currentUnit, cell);
                currentUnit = NextPlayer;
            }
            return currentUnit;
        }

        internal Unit GetCurrentUnit()
        {
            return currentUnit;
        }

        public void RemoveUnit(Unit unit)
        {
            --numberOfUnits;
            if (unit is Enemy)
            {              
                OnEnemyKilled?.Invoke((Enemy)unit);
                stateMachines.Remove(unit.GetComponent<StateMachine>());
                enemyList.Remove((Enemy)unit);
            }
            else
            {
                if (unit is Player)
                {
                    --numberOfPlayers;

                    Player player = (Player)unit;
                    playerList.Remove(player);
                    //cardController.RemoveClass(player.CharacterClass);
                    OnPlayerKilled?.Invoke((Player)unit);
                    if (playerList.Count <= 0 && gridController.UnitsPlaced)
                    {
                        OnPlayerLost?.Invoke();
                        SceneController.Instance.LoadScene(Scenes.EndGameScene, true);                      
                    }
                }
                targetableUnits.Remove(unit);
            }
            units.Remove(unit);
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

        public List<Unit> GetUnits()
        {
            return units;
        }

        public List<Unit> GetTargetableUnits()
        {
            return targetableUnits;
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

        internal void DamageTaken(Unit unit, int damage)
        {
            OnDamageTaken?.Invoke(unit, damage);
        }

        internal void EnemyStunned(Enemy unit)
        {
            OnEnemyStunned?.Invoke(unit);
        }

        internal void EnergyChanged(Player player, int energy)
        {
            OnEnergyChanged?.Invoke(player, energy);
        }

        internal void MovementChanged(int movement)
        {
            OnMovementChanged?.Invoke(currentUnit, movement);
        }
        #endregion

        #region Events
        private void TurnController_OnNewTurn()
        {
            currentUnit = playerList.First();
            //currentUnit = currentPlayer;
            SetCurrentUnit(currentUnit);
        }

        private void TurnController_OnCombatStart()
        {
            //currentUnit = enemyList.First();
        }
        #endregion
    }
}
