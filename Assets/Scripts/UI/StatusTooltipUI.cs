using UnityEngine;
using TMPro;
using FTS.Cards;
using System;
using System.Linq;
using UnityEngine.EventSystems;
using FTS.Characters;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace FTS.UI
{
    public class StatusTooltipUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] StatusTooltip tooltipPrefab;
        [SerializeField] Transform tooltipPosition;
        List<StatusTooltip> tooltips = new List<StatusTooltip>();

        //[SerializeField] RectTransform canvasRectTransform;
        //[SerializeField] RectTransform background;

        Draggable draggable;
        bool isGameCard = false;

        Vector2 padding = new Vector2(8, 8);


        #region MonoBehaviour Callbacks
        private void Start()
        {
            tooltipPosition.gameObject.SetActive(false);
            UnitController.OnSelectUnit += UnitController_OnSelectUnit;
            UnitController.OnSelectPlayer += UnitController_OnPlayerSelected;;
        }

        private void OnDestroy()
        {
            UnitController.OnSelectUnit -= UnitController_OnSelectUnit;
            UnitController.OnSelectPlayer -= UnitController_OnPlayerSelected;         
        }
        #endregion
        //#region MonoBehaviour Callbacks
        //private void FixedUpdate()
        //{
        //    if (background.gameObject.activeSelf)
        //    {
        //        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;
        //        anchoredPosition += padding;
        //        if (anchoredPosition.x + background.rect.width > canvasRectTransform.rect.width - padding.x)
        //        {
        //            anchoredPosition.x = canvasRectTransform.rect.width - background.rect.width - padding.x;
        //        }
        //        if (anchoredPosition.y + background.rect.height > canvasRectTransform.rect.height - padding.y)
        //        {
        //            anchoredPosition.y = canvasRectTransform.rect.height - background.rect.height - padding.y;
        //        }

        //        background.anchoredPosition = anchoredPosition;
        //    }
        //}
        //#endregion


        public void HideTooltip()
        {
            //background.gameObject.SetActive(false);
        }

        public void ShowToolTip(string tooltip)
        {
            ////background.gameObject.SetActive(true);
            //keywordNameText.text = tooltip.ToUpper();
            //keywordText.text = Keywords.KeywordTerms[tooltip.ToLower()];

            //Vector2 textSize = keywordText.GetRenderedValues(false);
            //Vector2 padding = new Vector2(30, 50);

            ////background.sizeDelta = textSize + padding;
            //keywordText.ForceMeshUpdate();
            //keywordNameText.ForceMeshUpdate();
        }

        internal void UpdateTooltips(Unit unit)
        {
            if (unit)
            {
                foreach (Transform child in tooltipPosition)
                {
                    Destroy(child.gameObject);
                }
                foreach (var status in unit.StatusController.GetStatuses())
                {
                    StatusTooltip tooltip = Instantiate(tooltipPrefab, tooltipPosition.transform);
                    tooltip.SetToolTip(status); ;
                }
            }
        }

        #region Events
        public void OnPointerEnter(PointerEventData eventData)
        {
            tooltipPosition.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltipPosition.gameObject.SetActive(false);
        }
        
        private void UnitController_OnSelectUnit(Unit unit)
        {
            UpdateTooltips(unit);
        }

        private void UnitController_OnPlayerSelected(Player player)
        {
            UpdateTooltips(player);
        }
        #endregion
    }
}
