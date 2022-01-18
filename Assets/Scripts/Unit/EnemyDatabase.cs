#region Using Statements
using System.Collections.Generic;
using UnityEngine;
using FTS.Cards;
using System.Linq;
using System;
#endregion

namespace FTS.Characters
{
    public class EnemyDatabase : MonoBehaviour
    {
        List<Enemy> enemies = new List<Enemy>();
        [SerializeField] Enemy gruntPrefab;
        [SerializeField] Enemy cowardPrefab;
        [SerializeField] Enemy rangerPrefab;
        [SerializeField] Enemy StunnerPrefab;


        #region Properties

        #endregion

        #region MonoBehaviour Callbacks
        private void Start()
        {
            enemies.Add(gruntPrefab);
            enemies.Add(cowardPrefab);
            enemies.Add(rangerPrefab);
            enemies.Add(StunnerPrefab);
        }
        #endregion

        #region Public Methods
        internal List<Enemy> GetEnemies()
        {
            return enemies;
        }

        internal Enemy GetEnemy(int index)
        {
            return enemies[index];
        }

        //TODO: return only first enemy in list. testing purposes only
        internal Character GetRandomEnemy()
        {
            //return enemies[UnityEngine.Random.Range(0, enemies.Count)];
            return enemies[0];
        }
        #endregion
    }
}