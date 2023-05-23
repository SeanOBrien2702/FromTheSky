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
        Image[] images;
        #region MonoBehaviour Callbacks
        private void Start()
        {
            UnitController.OnSelectUnit += UnitController_OnSelectUnit;
            UnitController.OnSelectPlayer += UnitController_OnPlayerSelected;
            images = GetComponentsInChildren<Image>();
            Debug.Log("number of images " + images.Length + "?/??");
            foreach (var item in images)
            {
                item.enabled = false;
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
                Debug.Log(status.name + "  " + statuses.Count);
                images[index].enabled = true;
                images[index].sprite = status.StatusImage;
                index++;
            }
            Debug.Log("remaining spots  " + index);
            for (int i = index; i < images.Length; i++)
            {
                
                images[i].enabled = false;
            }
            //if (unit == null)
            //{
            //    panel.gameObject.SetActive(false);
            //    return;
            //}

            //panel.gameObject.SetActive(true);
            //nameText.text = unit.name.Split('(')[0];
            //if (unit is Character)
            //{
            //    Character character = (Character)unit;
            //    healthText.text = "Health: " + character.Health + "/" + character.MaxHealth;
            //    movementText.gameObject.SetActive(true);
            //    movementText.text = "Movement: " + character.Mover.MovementLeft + "/" + character.GetStat(Stat.Movement).ToString();
            //}
            //else if (unit is Building)
            //{
            //    Building building = (Building)unit;
            //    healthText.text = "Health: " + building.Health + "/" + building.MaxHealth;
            //    movementText.gameObject.SetActive(false);
            //}
        }

        void ClearPanel()
        {

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
