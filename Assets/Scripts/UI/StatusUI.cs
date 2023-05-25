using FTS.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI
{
    public class StatusUI : MonoBehaviour
    {
        List<Status> statuses = new List<Status>();
        [SerializeField] Image[] images;
        [SerializeField] GameObject[] background;

        #region MonoBehaviour Callbacks
        private void Start()
        {
            UnitController.OnSelectUnit += UnitController_OnSelectUnit;
            UnitController.OnSelectPlayer += UnitController_OnPlayerSelected;
            foreach (var item in background)
            {
                item.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            UnitController.OnSelectUnit -= UnitController_OnSelectUnit;
            UnitController.OnSelectPlayer -= UnitController_OnPlayerSelected;
        }
        #endregion

        #region Private Methods
        public void UpdateUI(Unit unit)
        {
            statuses = unit.GetStatuses();
            
            int index = 0;
            foreach (var status in statuses)
            {
                background[index].SetActive(true);
                images[index].enabled = true;
                images[index].sprite = status.StatusImage;
                index++;
            }
            
            for (int i = index; i < background.Length; i++)
            {
                
                background[i].SetActive(false);
            }
        }
        #endregion

        #region Events
        private void UnitController_OnSelectUnit(Unit unit)
        {
            UpdateUI(unit);
        }

        private void UnitController_OnPlayerSelected(Player player)
        {
            UpdateUI(player);
        }
        #endregion
    }
}
