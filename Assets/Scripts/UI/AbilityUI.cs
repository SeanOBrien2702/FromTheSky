using FTS.Characters;
using FTS.Turns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI
{
    public class AbilityUI : MonoBehaviour
    {
        [SerializeField] Image abilityImage;
        [SerializeField] Button abilityButton;
        [SerializeField] GameObject tooltip;
        [SerializeField] TextMeshProUGUI tooltipText;

        void Start()
        {
            TurnController.OnEnemyTurn += TurnController_OnEnemyTurn;
            UnitController.OnSelectPlayer += UnitController_OnPlayerSelected;
            ToggleUI(false);
        }

        private void OnDestroy()
        {
            TurnController.OnEnemyTurn -= TurnController_OnEnemyTurn;
            UnitController.OnSelectPlayer -= UnitController_OnPlayerSelected;
        }

        void ToggleUI(bool toggle)
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(toggle);
        }

        private void TurnController_OnEnemyTurn(bool obj)
        {
            ToggleUI(false);
        }

        private void UnitController_OnPlayerSelected(Player player)
        {
            if (player.Abilty != null)
            {
                ToggleUI(true);
                abilityImage.sprite = player.Abilty.AbiltyImage;               
                tooltipText.text = player.Abilty.GetDescription();
                tooltip.SetActive(true);
                Canvas.ForceUpdateCanvases();
                tooltip.SetActive(false);
                abilityButton.onClick.RemoveAllListeners();
                abilityButton.onClick.AddListener(player.Abilty.ActivateEffect);
                abilityButton.onClick.AddListener(DiableButton);
                abilityButton.interactable = !player.Abilty.IsUsed;
            }
            else
            {
                ToggleUI(false);
            }
        }

        private void DiableButton()
        {
            abilityButton.interactable = false;
        }
    }
}
