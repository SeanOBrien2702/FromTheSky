using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Characters;
using FTS.Grid;
using System;

namespace FTS.Characters
{
    public abstract class Status : ScriptableObject
    {
        [SerializeField] StatusType statusType;
        [SerializeField] string statusName;
        [SerializeField] Sprite statusImage;
        [SerializeField] string statusDescription;

        public Sprite StatusImage { get => statusImage; }
        public string StatusName { get => statusType.ToString().ToUpper(); }
        public string StatusDescription { get => statusDescription; }
        public StatusType StatusType { get => statusType; }

        public virtual void Initialize(Unit unit)
        {
            
        }

        public virtual void Trigger()
        {
            Debug.Log("Base effect class");
        }

  
        public Status GetInfo()
        {
            return this;
        }
    }
}
