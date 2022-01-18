using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TooltipUI : MonoBehaviour
{
    [SerializeField] RectTransform canvasRectTransform;
    [SerializeField] RectTransform background;
    [SerializeField] TextMeshProUGUI keywordNameText;
    [SerializeField] TextMeshProUGUI keywordText;
    static readonly Dictionary<string, string> keywordTerms = new Dictionary<string, string>
    {
        {"atomize", "Can only be used once per battle"},
        {"on draw", "This effect happens when the card is drawn"},
        {"on discard", "This effect happens when the card is discarded"},
        {"energy", "Resource used to play cards"},
        {"armour", "Reduce damage taken for the turn"},
        {"barrier", "Absorb all damage from the next attack"},
        {"temporary", "Atomized when played or at end of turn"},
        {"heat", "Deal damage to all units around equal to heat than reduce damage by one"},
    };
    // Start is called before the first frame update
    Vector2 padding = new Vector2(8, 8);

    private void FixedUpdate()
    { 
        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;
        anchoredPosition += padding;
        if (anchoredPosition.x + background.rect.width > canvasRectTransform.rect.width - padding.x)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - background.rect.width - padding.x;
        }
        if(anchoredPosition.y + background.rect.height > canvasRectTransform.rect.height - padding.y)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - background.rect.height - padding.y;
        }

        background.anchoredPosition = anchoredPosition;
    }

    public void HideTooltip()
    {
        background.gameObject.SetActive(false);
    }

    internal void ShowToolTip(string tooltip)
    {
        background.gameObject.SetActive(true);
        keywordNameText.text = tooltip.ToUpper();
        keywordText.text = keywordTerms[tooltip.ToLower()];
        keywordText.ForceMeshUpdate();
        keywordNameText.ForceMeshUpdate(); 

        Vector2 textSize = keywordText.GetRenderedValues(false);
        Vector2 padding = new Vector2(30, 50);

        background.sizeDelta = textSize + padding;
        keywordText.ForceMeshUpdate();
        keywordNameText.ForceMeshUpdate();
    }
}
