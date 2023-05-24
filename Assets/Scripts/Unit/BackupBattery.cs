using FTS.Grid;
using FTS.Turns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FTS.Characters
{
    [RequireComponent(typeof(Building))]
    public class BackupBattery : MonoBehaviour
    {
        [SerializeField] int energyGained = 1;
        HexCell position;
        HexGridController grid;
        // Start is called before the first frame update
        void Start()
        {
            position = GetComponent<Building>().Location;
            grid = FindObjectOfType<HexGridController>().GetComponent<HexGridController>();
            TurnController.OnPlayerTurn += TurnController_OnPlayerTurn;
        }

        private void OnDestroy()
        {
            TurnController.OnPlayerTurn -= TurnController_OnPlayerTurn;
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void TurnController_OnPlayerTurn()
        {
            grid.GetClosestPlayer(position).Energy += 1;
        }
    }
}
