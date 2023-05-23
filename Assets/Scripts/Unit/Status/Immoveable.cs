using FTS.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Characters
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Status/Immoveable", fileName = "Immoveable.asset")]
    public class Immoveable : Status
    {
        Unit unit;
        UnitUI unitUI;
        public override void Initialize(Unit newUnit)
        {
            unit = newUnit;
            unitUI = unit.gameObject.GetComponentInChildren<UnitUI>();
        }
        public override void Trigger()
        {
            //unitUI.ShowAttribute(this.GetType().Name.ToUpper(), "#194D33"); //green
            Debug.Log("immoveable triggerred");
        }
    }
}
