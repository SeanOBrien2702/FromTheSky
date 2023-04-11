using FTS.Characters;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FTS.UI
{
    public class UnitPanelUI : MonoBehaviour
    {
        [SerializeField] GameObject panel;
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] TextMeshProUGUI healthText;
        [SerializeField] TextMeshProUGUI movementText;
        [SerializeField] TextMeshProUGUI descriptionText;
        [SerializeField] TextMeshProUGUI abiltyText;

        private void Start()
        {
            UnitController.OnPlayerSelected += UnitController_OnPlayerSelected;
        }

        private void OnDestroy()
        {
            UnitController.OnPlayerSelected -= UnitController_OnPlayerSelected;
        }

        public void UpdateUI(Character unit)
        {
            if(unit == null)
            {
                panel.gameObject.SetActive(false);
                return;
            }
            panel.gameObject.SetActive(true);
            nameText.text = unit.name;
            healthText.text = "Health: " + unit.GetStat(Stat.Health).ToString();
            movementText.text = "Movement: " + unit.GetStat(Stat.Movement).ToString();
        }

        private void UnitController_OnPlayerSelected(Player player)
        {
            UpdateUI(player);
        }
    }
}
