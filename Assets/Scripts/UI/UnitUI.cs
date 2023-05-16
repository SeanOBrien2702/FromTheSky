#region Using Statements
using AeLa.EasyFeedback.APIs;
using FTS.Characters;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#endregion

namespace FTS.UI
{
    public class UnitUI : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] Color healthColour;
        [SerializeField] Color damageColour;
        [SerializeField] Color potentialDamageColour;
        [SerializeField] Transform healthGrid;
        [SerializeField] GameObject healthSegment;
        List<Image> healthBar = new List<Image>();
        int currentHealth;

        [Header("Armour")]
        [SerializeField] GameObject sheild;
        [SerializeField] Text lblArmour;
        Camera cam;

        [Header("TurnOrder")]
        [SerializeField] TextMeshProUGUI turnOrderText;
        [SerializeField] StateMachine unit;
        StateController stateController;

        int maxHealth = 0;
        #region MonoBehaviour Callbacks
        void Start()
        {
            cam = Camera.main;
            
            UnitController.OnEnemyKilled += UnitController_OnEnemyKilled;         
            if(unit)
            {
                TurnOrderUI.OnHover += TurnOrderUI_OnHover;
                stateController = FindObjectOfType<StateController>().GetComponent<StateController>();
            }
        }

        public void FixedUpdate()
        {
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
        }

        private void OnDestroy()
        {
            UnitController.OnEnemyKilled -= UnitController_OnEnemyKilled;
            if (unit)
            {
                TurnOrderUI.OnHover -= TurnOrderUI_OnHover;
            }
        }
        #endregion

        #region Private Methods
        void UpdateMaxHealth()
        {

            for (int i = 0; i < maxHealth; i++)
            {
                GameObject newHealthSegment = Instantiate(healthSegment, healthGrid);
                healthBar.Add(newHealthSegment.transform.GetChild(0).GetComponent<Image>());
                if(i % 2 != 0)
                {
                    newHealthSegment.transform.localRotation = new Quaternion(180, 0, 0, 0); 
                }
            }
            Debug.Log(healthBar.Count);
        }
        #endregion

        #region Public Methods
        public void UpdateHealth(int health)
        {
            int index = 0;

            foreach (var item in healthBar)
            {
                if (index < health)
                {
                    item.color = healthColour;
                }
                else
                {
                    item.color = damageColour;
                }
                index++;
            }
            currentHealth = health;
        }

        public void UpdateHealth(int health, int newMax)
        {
            maxHealth = newMax;
            UpdateMaxHealth();
            UpdateHealth(health);
        }

        public void UpdateArmour(int armour)
        {
            lblArmour.text = armour.ToString();
            if (armour > 0)
            {
                sheild.gameObject.SetActive(true);
            }
            else
            {
                sheild.gameObject.SetActive(false);
            }

        }

        internal void ShowDamage(int health, int maxHealth, int damage)
        {
            int index = 0;
            foreach (var item in healthBar)
            {
                if (index < health - damage)
                {
                    item.color = healthColour;
                }
                else if (index < health)
                {
                    item.color = potentialDamageColour;
                }
                else
                {
                    item.color = damageColour;
                }
                index++;
            }
        }

        internal void HideDamage(int armour)
        {
            UpdateArmour(armour);
            UpdateHealth(currentHealth);
        }
        #endregion

        private void UnitController_OnEnemyKilled(Character obj)
        {
            HideDamage(0);
        }

        private void TurnOrderUI_OnHover(bool isHover)
        {
            turnOrderText.gameObject.SetActive(isHover);
            if (isHover)
            {                      
                turnOrderText.text = stateController.GetTurnOrder(unit).ToString();
            }          
        }
    }
}