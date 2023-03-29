using FTS.Characters;
using FTS.Saving;
using FTS.Turns;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

namespace FTS.Core
{ 
    public class RunController : MonoBehaviour, ISaveable
    {
        public static event System.Action<int> OnHealthChanged = delegate { };
        public static event System.Action<int> OnCinderChanged = delegate { };
        public static event System.Action<int> OnDayChanged = delegate { };

        [SerializeField] int startingCinder = 50;
        [SerializeField] int startingHealth = 50;
        [SerializeField] int startingDay = 10;
        RunInfo runInfo = new RunInfo();
        int health;
        int day;
        int cinder;

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
                OnDayChanged.Invoke(day);
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

        void Start()
        {
            Health = startingHealth;
            day = startingDay;
            Cinder = startingCinder;
            UnitController.OnDamageTaken += UnitController_OnDamageTaken;
            UnitController.OnEnemyLost += UnitController_OnEnemyLost;
        }

        private void UnitController_OnEnemyLost()
        {
            //throw new NotImplementedException();
        }

        private void TakeDamage(int damage)
        {
            Health -= damage;
            Debug.Log("take damage " + damage);
            if(Health <= 0)
            {
                SceneController.Instance.LoadScene(Scenes.MainMenu);
            }
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
    }

    struct RunInfo
    {
        public int Health;
        public int Day;
        public int Cinder;
    }
}
