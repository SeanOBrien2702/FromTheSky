using FTS.Characters;
using FTS.Saving;
using FTS.Turns;
using System;
using UnityEngine;

namespace FTS.Core
{ 
    public class RunController : MonoBehaviour, ISaveable
    {
        public static event System.Action OnPlayerLost = delegate { };
        public static event Action<int> OnHealthChanged = delegate { };
        public static event Action<int> OnCinderChanged = delegate { };

        [SerializeField] int startingCinder = 50;
        [SerializeField] int startingHealth = 50;
        [SerializeField] int startingDay = 10;
        [SerializeField] int difficultyIntervals = 2;
        RunInfo runInfo = new RunInfo();
        int health;
        int encounterCount = 0;   
        int cinder;
        bool hasWon = true;
        CombatType combatType;

        public static RunController Instance { get; private set; }

        public int Health
        {
            get { return health; }
            set
            {
                health = value;
                OnHealthChanged.Invoke(health);
            }
        }

        public int Cinder
        {
            get { return cinder; }
            set
            {
                cinder = value;
                OnCinderChanged.Invoke(health);
            }
        }

        public bool HasWon { get => hasWon; set => hasWon = value; }
        public CombatType CombatType { get => combatType; set => combatType = value; }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            StartValues();
            UnitController.OnDamageTaken += UnitController_OnDamageTaken;
            UnitController.OnPlayerLost += UnitController_OnPlayerLost;
            ObjectiveController.OnPlayerWon += ObjectiveController_OnPlayerWon;
        }

        private void OnDestroy()
        {
            UnitController.OnDamageTaken -= UnitController_OnDamageTaken;
            UnitController.OnPlayerLost -= UnitController_OnPlayerLost;
            ObjectiveController.OnPlayerWon -= ObjectiveController_OnPlayerWon;
        }

        private void TakeDamage(int damage)
        {
            Health -= damage;
            Debug.Log("take damage " + damage);
            if(Health <= 0)
            {
                OnPlayerLost?.Invoke();
                SceneController.Instance.LoadScene(Scenes.EndGameScene, true);
            }
        }

        public void StartValues()
        {
            Health = startingHealth;
            encounterCount = 0;
            Cinder = startingCinder;
        }

        public int GetDifficultyScale()
        {
            int difficulty = Mathf.FloorToInt(encounterCount / difficultyIntervals);
            if (difficulty > 3)
            {
                difficulty = 3;
            }
            return difficulty;
        }

        internal void SetCombatType(CombatType type)
        {
            combatType = type;
            encounterCount++;
        }

        #region Saving Methods
        public object CaptureState()
        {
            runInfo.Health = Health;
            runInfo.EncounterCount = encounterCount;
            runInfo.Cinder = Cinder;

            return runInfo;
        }

        public void RestoreState(object state)
        {
            runInfo = (RunInfo)state;
            health = runInfo.Health;
            encounterCount = runInfo.EncounterCount;
            cinder = runInfo.Cinder;
        }
        #endregion

        private void UnitController_OnDamageTaken(Unit unit, int damage)
        {
            if(unit is Building)
            {
                TakeDamage(damage);
            }
        }
        private void UnitController_OnPlayerLost()
        {
            hasWon = false;
        }

        private void ObjectiveController_OnPlayerWon()
        {
            if (combatType == CombatType.Boss)
            {
                SceneController.Instance.LoadScene(Scenes.EndGameScene, true);
            }
            else
            {
                SceneController.Instance.LoadScene(Scenes.DraftScene, true);
            }
        }
    }

    struct RunInfo
    {
        public int Health;
        public int EncounterCount;
        public int Cinder;
    }
}
