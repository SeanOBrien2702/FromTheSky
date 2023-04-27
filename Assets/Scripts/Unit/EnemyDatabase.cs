#region Using Statements
using FTS.Core;
using FTS.Turns;
using System.Collections.Generic;
using UnityEngine;
#endregion

namespace FTS.Characters
{
    public class EnemyDatabase : MonoBehaviour
    {
        [SerializeField] EnemyDatabaseSettings[] databaseSettings;
        EnemyDatabaseSettings settings;

        [Header("Common enemies")]        
        [SerializeField] Enemy piercingPrefab;
        [SerializeField] Enemy projectilePrefab;
        // [SerializeField] Enemy trajectoryPrefab;
        List<Enemy> commonEnemies = new List<Enemy>();

        [Header("Rare enemies")]
        [SerializeField] Enemy elitePiercingPrefab;
        [SerializeField] Enemy eliteProjectilePrefab;
        //[SerializeField] Enemy StunnerPrefab;
        List<Enemy> rareEnemies = new List<Enemy>();

        //[Header("Support enemies")]
        //[SerializeField] Enemy medicPrefab;
        //List<Enemy> supportEnemies = new List<Enemy>();

        [Header("Boss enemies")]        
        [SerializeField] Enemy bossPrefab;
        [SerializeField] Enemy zealotPrefab;
        List<Enemy> bossEnemies = new List<Enemy>();

        #region MonoBehaviour Callbacks
        private void Start()
        {
            commonEnemies.Add(piercingPrefab);
            commonEnemies.Add(projectilePrefab);
            //commonEnemies.Add(trajectoryPrefab);

            rareEnemies.Add(elitePiercingPrefab);
            rareEnemies.Add(eliteProjectilePrefab);

            bossEnemies.Add(bossPrefab);
            bossEnemies.Add(zealotPrefab);
        }
        #endregion

        #region Public Methods
        //internal List<Enemy> GetEnemies()
        //{
        //    return enemies;
        //}

        internal Enemy GetEnemy(int index)
        {
            return commonEnemies[index];
        }

        internal Character GetRandomEnemy()
        {
            settings = databaseSettings[RunController.Instance.GetDifficultyScale()];

            int randomNumber = UnityEngine.Random.Range(0, settings.GetChanceTotal());
            Enemy enemy;
            if (randomNumber <= settings.GetCommonChance())
            {
                enemy = commonEnemies[Random.Range(0, commonEnemies.Count)];
            }
            else if (randomNumber > settings.GetCommonChance() && randomNumber <= settings.GetRareChance())
            {
                enemy = rareEnemies[Random.Range(0, rareEnemies.Count)];
            }       
            else if (randomNumber > settings.GetRareChance())
            {
                enemy = bossEnemies[Random.Range(0, bossEnemies.Count)];
            }      
            else
            {
                enemy = commonEnemies[Random.Range(0, commonEnemies.Count)];;
            }

            return enemy;
        }

        internal Character GetCombatType()
        {
            Enemy enemy = null;
            switch (RunController.Instance.CombatType)
            {
                case CombatType.Normal:
                    enemy = commonEnemies[Random.Range(0, commonEnemies.Count)];
                    break;
                case CombatType.Elite:
                    enemy = rareEnemies[Random.Range(0, rareEnemies.Count)];
                    break;
                case CombatType.Boss:
                    enemy = bossEnemies[Random.Range(0, bossEnemies.Count)];
                    break;
                default:
                    break;
            }
            return enemy;
        }
        #endregion
    }
}