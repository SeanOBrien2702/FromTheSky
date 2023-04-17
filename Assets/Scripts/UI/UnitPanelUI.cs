using FTS.Characters;
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

        #region MonoBehaviour Callbacks
        private void Start()
        {
            UnitController.OnSelectUnit += UnitController_OnSelectUnit;
            UnitController.OnSelectPlayer += UnitController_OnPlayerSelected;
            UnitController.OnMovementChanged += UnitController_OnMovementChanged;
        }

        private void OnDestroy()
        {
            UnitController.OnSelectUnit -= UnitController_OnSelectUnit;
            UnitController.OnSelectPlayer -= UnitController_OnPlayerSelected;
            UnitController.OnMovementChanged -= UnitController_OnMovementChanged;
        }
        #endregion

        #region Private Methods
        public void UpdateUI(Unit unit)
        {
            if (unit == null)
            {
                panel.gameObject.SetActive(false);
                return;
            }
           
            panel.gameObject.SetActive(true);
            nameText.text = unit.name.Split('(')[0];
            if (unit is Character)
            {
                Character character = (Character)unit;
                healthText.text = "Health: " + character.Health + "/" + character.MaxHealth;
                movementText.gameObject.SetActive(true);
                movementText.text = "Movement: " + character.Mover.MovementLeft + "/" + character.GetStat(Stat.Movement).ToString();
            }
            else if (unit is Building)
            {
                Building building = (Building)unit;
                healthText.text = "Health: " + building.Health + "/" + building.MaxHealth;
                movementText.gameObject.SetActive(false);
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
        private void UnitController_OnMovementChanged(Unit unit, int arg2)
        {
            UpdateUI(unit);
        }
        #endregion
    }
}
