using FTS.Characters;
using FTS.Saving;
using FTS.Turns;
using System;
using UnityEngine;

namespace FTS.Core
{ 
    public class RunController : MonoBehaviour, ISaveable
    {
        public static event Action<int> OnHealthChanged = delegate { };
        public static event Action<int> OnCinderChanged = delegate { };
        public static event Action<int> OnDayChanged = delegate { };

        [SerializeField] int startingCinder = 50;
        [SerializeField] int startingHealth = 50;
        [SerializeField] int startingDay = 10;
        [SerializeField] int difficultyIntervals = 3;
        RunInfo runInfo = new RunInfo();
        int health;
        int day;
        int currentDay = 2;     
        int cinder;
        bool hasWon = false;
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

        public int Day
        {
            get { return day; }
            set
            {
                day = value;
                currentDay++;
                OnDayChanged.Invoke(day);
                if(day <= 0)
                {
                    hasWon = true;
                    day = startingDay;
                    currentDay = 0;
                    SceneController.Instance.LoadScene(Scenes.EndGameScene, true);
                }
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
        }

        private void OnDestroy()
        {
            UnitController.OnDamageTaken -= UnitController_OnDamageTaken;
            UnitController.OnPlayerLost -= UnitController_OnPlayerLost;
        }

        private void TakeDamage(int damage)
        {
            Health -= damage;
            Debug.Log("take damage " + damage);
            if(Health <= 0)
            {
                SceneController.Instance.LoadScene(Scenes.EndGameScene, true);
            }
        }

        public void StartValues()
        {
            Health = startingHealth;
            day = startingDay;
            Cinder = startingCinder;
        }

        public int GetDifficultyScale()
        {
            return Mathf.FloorToInt(currentDay / difficultyIntervals);
        }

        #region Saving Methods
        public object CaptureState()
        {
            runInfo.Health = Health;
            runInfo.Day = Day;
            runInfo.Cinder = Cinder;

            return runInfo;
        }

        public void RestoreState(object state)
        {
            runInfo = (RunInfo)state;
            health = runInfo.Health;
            day = runInfo.Day;
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
    }

    struct RunInfo
    {
        public int Health;
        public int Day;
        public int Cinder;
    }
}
