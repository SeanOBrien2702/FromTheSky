#region Using Statements
using FTS.Grid;
using FTS.Turns;
using UnityEngine;
#endregion

namespace FTS.Characters
{
    public class Building : Unit
    {
        [SerializeField] int startingHealth = 5;

        #region Properties
        public override HexCell Location
        {
            get { return location; }
            set
            {
                if (location)
                {
                    location.Unit = null;
                }
                location = value;
                value.Unit = this;
                transform.localPosition = value.transform.localPosition;
            }
        }
        #endregion

        #region MonoBehaviour Callbacks
        protected override void Awake()
        {
            base.Awake();
            Health = maxHealth = startingHealth;
            TurnController.OnPlayerTurn += TurnController_OnNewTurn;
        }

        private void Start()
        {
            unitUI.UpdateHealth(Health, maxHealth);
        }

        private void OnDestroy()
        {
            TurnController.OnPlayerTurn -= TurnController_OnNewTurn;
        }
        #endregion
    }
}
