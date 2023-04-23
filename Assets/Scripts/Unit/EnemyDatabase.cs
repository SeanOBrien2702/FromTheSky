#region Using Statements
using System.Collections.Generic;
using UnityEngine;
#endregion

namespace FTS.Characters
{
    public class EnemyDatabase : MonoBehaviour
    {
        List<Enemy> enemies = new List<Enemy>();
        [SerializeField] Enemy piercingPrefab;
        [SerializeField] Enemy projectilePrefab;
        [SerializeField] Enemy trajectoryPrefab;
        [SerializeField] Enemy multiProjectilePrefab;
        //[SerializeField] Enemy StunnerPrefab;

        #region MonoBehaviour Callbacks
        private void Start()
        {
            enemies.Add(piercingPrefab);
            enemies.Add(projectilePrefab);
            enemies.Add(multiProjectilePrefab);
            enemies.Add(trajectoryPrefab);
            //enemies.Add(StunnerPrefab);
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

        internal Character GetRandomEnemy()
        {
            //TODO: add terjectory enemy to possible options
            int randomNumber = UnityEngine.Random.Range(0, 105);
            Enemy enemy;
            if (randomNumber <= 65)
            {
                enemy = projectilePrefab;
            }
            else if (randomNumber > 65 && randomNumber <= 100)
            {
                enemy = piercingPrefab;
            }
            else if (randomNumber > 100 && randomNumber <= 105)
            {
                enemy = multiProjectilePrefab;
            }
            else
            {
                enemy = projectilePrefab;
            }

            return enemy;
        }
        #endregion
    }
}