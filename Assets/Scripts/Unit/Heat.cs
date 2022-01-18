using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FTS.Grid;
using FTS.Turns;
using TMPro;
using System;

namespace FTS.Characters
{
    public class Heat : MonoBehaviour
    {
        [SerializeField] GameObject heatUI;
        [SerializeField] Text heatText;
        [SerializeField] bool friendlyFire = false;
        [SerializeField] int range = 1;
        int heatLevel = 5;

        Mover mover;
        HexGrid hexGrid;

        #region Properties
        public int HeatLevel
        {
            get { return heatLevel; }
            set { heatLevel = value;
                UpdateHeatText(heatLevel);
            }
        }
        #endregion

        #region MonoBehaviour Callbacks
        void Start()
        {
            mover = GetComponent<Mover>();
            hexGrid = FindObjectOfType<HexGrid>().GetComponent<HexGrid>();
            TurnController.OnEndTurn += TurnController_OnEndTurn;
            //UpdateHeat();
        }

        private void OnDestroy()
        {
            TurnController.OnEndTurn -= TurnController_OnEndTurn;
        }


        // Update is called once per frame
        void Update()
        {

        }

        #endregion
        #region Private Methods
        private void UpdateHeat()
        {
            List<HexCell> area = hexGrid.GetArea(mover.Location, range);
            Debug.Log("area size " + area.Count);
            foreach (var cell in area)
            {
                if(cell.Unit && cell != mover.Location)
                {
                    cell.Unit.CalculateDamageTaken(heatLevel);
                }
            }
            heatLevel--;
            if (heatLevel < 0)
            {
                heatLevel = 0;
            }
            UpdateHeatText(heatLevel);
        }
        #endregion
        #region Public Methods

        public void UpdateHeatText(int heat)
        {
            Debug.Log("heat level " + heat);
            heatText.text = heat.ToString();
            if (heat > 0)
            {
                heatUI.gameObject.SetActive(true);
            }
            else
            {
                heatUI.gameObject.SetActive(false);
            }

        }
        #endregion
        #region Events
        private void TurnController_OnEndTurn()
        {
            UpdateHeat();
        }


        #endregion
    }
}