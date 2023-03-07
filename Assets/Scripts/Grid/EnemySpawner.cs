#region Using Statements
using FTS.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Turns;
#endregion
namespace FTS.Grid
{
    public class EnemySpawner : MonoBehaviour
    {
        HexGrid hexGrid;
        UnitController unitController;
        EnemyDatabase enemyDatabase;
        List<HexCell> spawnLocations = new List<HexCell>();
        [SerializeField] int minSpawn = 1;
        [SerializeField] int maxSpawn = 3;
        [SerializeField] int spawnLimit = 8;
        int enemiesSpawned = 0;

        #region MonoBehaviour Callbacks
        void Start()
        {
            hexGrid = GetComponent<HexGrid>();
            unitController = GetComponent<UnitController>();
            enemyDatabase = FindObjectOfType<EnemyDatabase>().GetComponent<EnemyDatabase>();
            TurnController.OnPlayerTurn += TurnController_OnNewTurn;
            TurnController.OnEnemyTurn += TurnController_OnEndTurn;
        }

        private void OnDestroy()
        {
            TurnController.OnPlayerTurn -= TurnController_OnNewTurn;
            TurnController.OnEnemyTurn -= TurnController_OnEndTurn;
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
                Debug.Log("spawn cell " +cell);
                cell.SetDangerIndicator(true);
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
                    cell.SetDangerIndicator(false); ;                   
                }
            }
        }
        #endregion

        #region Events
        private void TurnController_OnNewTurn()
        {
            SetSpawnPositions();
        }

        private void TurnController_OnEndTurn(bool isTelegraph)
        {
            if(isTelegraph)
                SpawnEnemies();
        }
        #endregion
    }
}
