#region Using Statements
using FTS.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Turns;
using FTS.Core;
#endregion
namespace FTS.Grid
{
    public class EnemySpawner : MonoBehaviour
    {
        HexGrid hexGrid;
        UnitController unitController;
        EnemyDatabase enemyDatabase;
        TurnController turnController;
        List<HexCell> spawnLocations = new List<HexCell>();
        [SerializeField] int minSpawn = 1;
        [SerializeField] int maxSpawn = 3;
        [SerializeField] int spawnLimit = 8;
        [SerializeField] int maxEnemiesAtOnce = 4;
        int enemiesSpawned = 0;

        #region MonoBehaviour Callbacks
        void Start()
        {
            hexGrid = GetComponent<HexGrid>();
            unitController = GetComponent<UnitController>();
            enemyDatabase = FindObjectOfType<EnemyDatabase>().GetComponent<EnemyDatabase>();
            turnController = FindObjectOfType<TurnController>().GetComponent<TurnController>();
            TurnController.OnPlayerTurn += TurnController_OnNewTurn;
            TurnController.OnEnemySpawn += TurnController_OnEnemySpawn;
        }

        private void OnDestroy()
        {
            TurnController.OnPlayerTurn -= TurnController_OnNewTurn;
            TurnController.OnEnemySpawn -= TurnController_OnEnemySpawn;
        }
        #endregion

        #region Private Methods
        private void SetSpawnPositions()
        {
            spawnLocations.Clear();
            int numEnemiesToSpawm = Random.Range(minSpawn, maxSpawn);
            for (int i = 0; i < numEnemiesToSpawm; i++)
            {
                enemiesSpawned++;
                if(enemiesSpawned > spawnLimit)
                {
                    break;
                }
                HexCell cell = hexGrid.FindGridEdge();
                cell.IsSpawn = true;
                cell.SetSpawningHighlight(true);
                spawnLocations.Add(cell);
            }
        }

        private void SpawnEnemies()
        {
            foreach (var cell in spawnLocations)
            {               
                if(!cell.Unit)
                {
                    unitController.CreateUnit(enemyDatabase.GetRandomEnemy(), cell);
                    cell.SetSpawningHighlight(false); ;                   
                }
            }
            turnController.UpdatePhase();
        }
        #endregion

        #region Events
        private void TurnController_OnNewTurn()
        {
            if(unitController.GetEnemyUnits().Count <= maxEnemiesAtOnce)
                SetSpawnPositions();
        }

        private void TurnController_OnEnemySpawn()
        {         
            SpawnEnemies();
        }
        #endregion
    }
}
