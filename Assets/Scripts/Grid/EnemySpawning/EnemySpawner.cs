#region Using Statements
using FTS.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FTS.Turns;
using FTS.Core;
using static UnityEditor.FilePathAttribute;
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
        [SerializeField] SpawningSettings[] spawningSettings;
        SpawningSettings settings;

        [SerializeField] CameraController cameraController;
        int enemiesSpawned = 0;

        [Header("DropPod")]
        [SerializeField] Transform dropPodPosotion;
        [SerializeField] GameObject dropPod;
        ParticleSystem dropPodSmoke;
        private Vector3 dropPodStartPos;
        Vector3 landingOffset = new Vector3(0, 8, 0);
        private Vector3 offset = new Vector3(0, 100, 0);
        float duration = 1.0f;
    
        #region MonoBehaviour Callbacks
        void Start()
        {
            settings = spawningSettings[RunController.Instance.GetDifficultyScale()];
            hexGrid = GetComponent<HexGrid>();
            unitController = GetComponent<UnitController>();
            enemyDatabase = FindObjectOfType<EnemyDatabase>().GetComponent<EnemyDatabase>();
            turnController = FindObjectOfType<TurnController>().GetComponent<TurnController>();
            TurnController.OnPlayerTurn += TurnController_OnNewTurn;
            TurnController.OnEnemySpawn += TurnController_OnEnemySpawn;

            dropPodStartPos = dropPodPosotion.transform.position;
            dropPodSmoke = dropPodPosotion.GetComponentInChildren<ParticleSystem>();

            for (int i = 0; i < settings.StartingEnemies; i++)
            {
                CreateUnitInRandomPosition();
                enemiesSpawned++;
            }
        }

        private void OnDestroy()
        {
            TurnController.OnPlayerTurn -= TurnController_OnNewTurn;
            TurnController.OnEnemySpawn -= TurnController_OnEnemySpawn;
        }
        #endregion

        #region Private Methods

        void CreateUnitInRandomPosition()
        {
            HexCell cell = hexGrid.GetRandomPosition(1)[0];
            unitController.CreateUnit(enemyDatabase.GetRandomEnemy(), cell);
        }

        private void SetSpawnPositions()
        {
            spawnLocations.Clear();
            int numEnemiesToSpawm = Random.Range(settings.MinSpawn, settings.MaxSpawn);
            for (int i = 0; i < numEnemiesToSpawm; i++)
            {               
                if(enemiesSpawned >= settings.SpawnLimit)
                {
                    break;
                }
                enemiesSpawned++;
                HexCell cell = hexGrid.GetRandomPosition(1)[0];
                cell.IsSpawn = true;
                cell.SetSpawningHighlight(true);
                spawnLocations.Add(cell);
            }
        }

        private void ResetDropPod()
        {
            dropPodPosotion.transform.position = dropPodStartPos;
            dropPod.SetActive(true);
        }
        #endregion

        #region Coroutines
        private IEnumerator SpawnEnemies()
        {
            foreach (var cell in spawnLocations)
            {
                Vector3 position = cell.transform.position;
                yield return StartCoroutine(cameraController.MoveToPosition(position));
                yield return StartCoroutine(SpawnAnimation(position));
                dropPodSmoke.Play();
                if (cell.Unit)
                {
                    cell.Unit.Die();
                }

                unitController.CreateUnit(enemyDatabase.GetRandomEnemy(), cell);
                cell.SetSpawningHighlight(false);
                Invoke(nameof(ResetDropPod), 1.5f);
                yield return new WaitForSeconds(0.25f);
                dropPod.SetActive(false);
            }
            turnController.UpdatePhase();
        }

        private IEnumerator SpawnAnimation(Vector3 position)
        {          
            yield return StartCoroutine(LerpDroppod(position + offset, position + landingOffset));
        }

        IEnumerator LerpDroppod(Vector3 startPosition, Vector3 targetPosition)
        {
            float time = 0;
            dropPodPosotion.localPosition = startPosition;
            while (time < duration)
            {
                float t = time / duration;
                t = t * t;
                dropPodPosotion.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
                time += Time.deltaTime;
                yield return null;
            }
            dropPodPosotion.localPosition = targetPosition;
        }
        #endregion

        #region Events
        private void TurnController_OnNewTurn()
        {
            Debug.Log(unitController.GetEnemyUnits().Count + " "+ settings.MaxEnemiesAtOnce);
            if (unitController.GetEnemyUnits().Count < settings.MaxEnemiesAtOnce)
                SetSpawnPositions();
        }

        private void TurnController_OnEnemySpawn()
        {
            Debug.Log("spawn?");
            StartCoroutine(SpawnEnemies());
        }
        #endregion
    }
}
