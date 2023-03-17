using UnityEngine;
using TMPro;
using FTS.Cards;
using System;
using System.Linq;
using UnityEngine.EventSystems;

namespace FTS.UI
{ 
    public class TooltipUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] Tooltip tooltipPrefab;
        [SerializeField] Transform tooltipPosition;

        //[SerializeField] RectTransform canvasRectTransform;
        //[SerializeField] RectTransform background;

        Draggable draggable;
        bool isGameCard = false;

        Vector2 padding = new Vector2(8, 8);


        private void Start()
        {
            draggable = GetComponent<Draggable>();
            if(draggable)
            {
                isGameCard = true;
            }
        }

        private void FixedUpdate()
        {
            if(isGameCard && draggable.IsDragging)
            {
                tooltipPosition.gameObject.SetActive(false);
            }
        }
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

        internal void CreateTooltips(string rulesText)
        {
            foreach (string word in rulesText.ToUpper().Split('>', '<'))
            {
                Debug.Log(word);    
                if(Keywords.KeywordTerms.Keys.Contains(word))
                {
                    Debug.Log("found word");
                    Tooltip tooltip = Instantiate(tooltipPrefab, tooltipPosition.transform);
                    tooltip.SetToolTip(word, Keywords.KeywordTerms[word]);
                }
            }
            tooltipPosition.gameObject.SetActive(false);
            //foreach (var keyword in Keywords.KeywordTerms)
            //{
            //    if(rulesText.Contains(keyword.Key))
            //    {
            //        Tooltip tooltip = Instantiate(tooltipPrefab, tooltipPosition);
            //        tooltip.SetToolTip(keyword.Key, keyword.Value);
            //    }
            //}
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tooltipPosition.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltipPosition.gameObject.SetActive(false);
        }
    }
}
