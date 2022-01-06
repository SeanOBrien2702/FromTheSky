#region Using Statements
using SP.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SP.Turns;
#endregion
namespace SP.Grid
{
    public class EnemySpawner : MonoBehaviour
    {
        HexGrid hexGrid;
        UnitController unitController;
        EnemyDatabase enemyDatabase;
        List<HexCell> spawnLocations = new List<HexCell>();
        [SerializeField] int numEnemiesToSpawm = 3;
        
        #region Properties

        #endregion

        #region MonoBehaviour Callbacks
        // Start is called before the first frame update
        void Start()
        {
            hexGrid = GetComponent<HexGrid>();
            unitController = GetComponent<UnitController>();
            enemyDatabase = FindObjectOfType<EnemyDatabase>().GetComponent<EnemyDatabase>();
            TurnController.OnNewTurn += TurnController_OnNewTurn;
            TurnController.OnEndTurn += TurnController_OnEndTurn;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDestroy()
        {
            TurnController.OnNewTurn -= TurnController_OnNewTurn;
            TurnController.OnEndTurn -= TurnController_OnEndTurn;
        }
        #endregion

        #region Private Methods
        private void SetSpawnPositions()
        {
            spawnLocations.Clear();
            //numEnemiesToSpawm = 1;//Random.Range(2, 5);
            for (int i = 0; i < numEnemiesToSpawm; i++)
            {
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

        #region Public Methods

        #endregion

        #region Coroutines

        #endregion

        #region Events
        private void TurnController_OnNewTurn()
        {
            SetSpawnPositions();
        }

        private void TurnController_OnEndTurn()
        {
            SpawnEnemies();
        }
        #endregion
    }
}
