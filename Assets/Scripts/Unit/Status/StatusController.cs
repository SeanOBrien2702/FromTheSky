using FTS.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FTS.Characters
{
    public class StatusController : MonoBehaviour
    {
        [SerializeField] List<Status> statuses;
        Unit unit;
        UnitUI unitUI;

        private void Start()
        {
            unit = GetComponent<Unit>();
            unitUI = GetComponentInChildren<UnitUI>();
            foreach (var status in statuses)
            {
                status.Initialize(unit);
            }
        }

        internal void AddStatus(StatusType statusType)
        {
            Status attribute = StatusDatabase.Instance.GetStatus(statusType);
            attribute.Initialize(unit);
            statuses.Add(attribute);
        }

        public void TriggerStatus(StatusType statusType)
        {
            if (statuses.Exists(item => item.StatusType == statusType))
            {
                unitUI.ShowAttribute(statusType.ToString().ToUpper(), "#194D33"); //green
                statuses.Find(item => item.StatusType == statusType).Trigger();
            }
        }

        public void RemoveStatus(Status status) 
        { 
            if(statuses.Contains(status))
            {
                statuses.Remove(status);
            }
        }

        internal List<Status> GetStatuses()
        {
            return statuses;
        }

        public bool DoesStatusExist(StatusType attributeType)
        {
            return statuses.Exists(item => item.StatusType == attributeType);
        }
    }
}
