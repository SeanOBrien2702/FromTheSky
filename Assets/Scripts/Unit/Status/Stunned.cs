using FTS.Turns;
using FTS.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Characters
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Status/Stunned", fileName = "Stunned.asset")]
    public class Stunned : Status
    {
        Unit unit;
        UnitUI unitUI;
        public override void Initialize(Unit newUnit)
        {
            unit = newUnit;
            unit.Stun();
            unitUI = unit.gameObject.GetComponentInChildren<UnitUI>();
            TurnController.OnEnemySpawn += TurnController_OnEnemySpawn;
        }

        private void OnDestroy()
        {
            TurnController.OnEnemySpawn -= TurnController_OnEnemySpawn;
        }


        // Update is called once per frame
        void Update()
        {

        }

        private void TurnController_OnEnemySpawn()
        {
            unit.StatusController.RemoveStatus(this);
        }

        public override void Trigger()
        {
            //unitUI.ShowAttribute(this.GetType().Name.ToUpper(), "#201dd8");
        }
    }
}
