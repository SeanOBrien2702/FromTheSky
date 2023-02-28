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
        public static event System.Action<int> OnValueChanged = delegate { };
        [SerializeField] int startingHealth = 50;
        RunInfo runInfo = new RunInfo();
        int health;
        int day;
        int money;

        public int Health
        {
            get { return health; }
            set
            {
                health = value;
                OnValueChanged.Invoke(health);
            }
        }

        public int Day
        {
            get { return day; }
            set
            {
                day = value;
            }
        }

        public int Money
        {
            get { return money; }
            set
            {
                money = value;
            }
        }

        void Start()
        {
            Health = startingHealth;
            UnitController.OnDamageTaken += UnitController_OnDamageTaken;
            UnitController.OnEnemyLost += UnitController_OnEnemyLost;
        }

        private void UnitController_OnEnemyLost()
        {
            throw new NotImplementedException();
        }

        private void TakeDamage(int damage)
        {
            Health -= damage;
            Debug.Log("take damage " + damage);
            if(Health <= 0)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }

        #region Saving Methods
        public object CaptureState()
        {
            runInfo.Health = Health;
            runInfo.Day = Day;
            runInfo.Money = Money;

            return runInfo;
        }

        public void RestoreState(object state)
        {
            runInfo = (RunInfo)state;
            health = runInfo.Health;
            day = runInfo.Day;
            money = runInfo.Money;
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
        public int Money;
    }
}
