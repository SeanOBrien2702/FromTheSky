using FTS.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Characters
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Status/Precarious", fileName = "Precarious.asset")]
    public class Precarious : Status
    {
        Unit unit;
        UnitUI unitUI;
        public override void Initialize(Unit newUnit)
        {
            unit = newUnit;
            unitUI = unit.gameObject.GetComponentInChildren<UnitUI>();
            Debug.Log("Initialize " + unit + " unit?");
        }

        public override void Trigger()
        {
            unit.CalculateDamageTaken(1);
            //unitUI.ShowAttribute(this.GetType().Name.ToUpper(), "#9c27b0");
            //Debug.Log("immoveable triggerred " + unitUI + " unit ui?");
        }
    }
}
