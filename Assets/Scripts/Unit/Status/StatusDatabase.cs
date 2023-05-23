using FTS.Cards;
using FTS.Characters;
using FTS.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusDatabase : MonoBehaviour
{
    Dictionary<StatusType, Status> lookupTable = null;
    [SerializeField] List<Status> statuses = new List<Status>();

    public static StatusDatabase Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    internal Status GetStatus(StatusType attributeType)
    {
        return statuses.Find(item => item.StatusType == attributeType);
    }
}
