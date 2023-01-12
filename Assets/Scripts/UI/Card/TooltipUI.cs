using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    [SerializeField] RectTransform canvasRectTransform;
    [SerializeField] RectTransform background;
    [SerializeField] TextMeshProUGUI keywordNameText;
    [SerializeField] TextMeshProUGUI keywordText;
    
    Vector2 padding = new Vector2(8, 8);

    #region MonoBehaviour Callbacks
    private void FixedUpdate()
    {
        if (background.gameObject.activeSelf)
        {
            Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;
            anchoredPosition += padding;
            if (anchoredPosition.x + background.rect.width > canvasRectTransform.rect.width - padding.x)
            {
                anchoredPosition.x = canvasRectTransform.rect.width - background.rect.width - padding.x;
            }
            if (anchoredPosition.y + background.rect.height > canvasRectTransform.rect.height - padding.y)
            {
                anchoredPosition.y = canvasRectTransform.rect.height - background.rect.height - padding.y;
            }

            background.anchoredPosition = anchoredPosition;
        }
    }
    #endregion


    public void HideTooltip()
    {
        background.gameObject.SetActive(false);
    }

    public void ShowToolTip(string tooltip)
    {
        background.gameObject.SetActive(true);
        keywordNameText.text = tooltip.ToUpper();
        keywordText.text = Keywords.KeywordTerms[tooltip.ToLower()];

        Vector2 textSize = keywordText.GetRenderedValues(false);
        Vector2 padding = new Vector2(30, 50);

        background.sizeDelta = textSize + padding;
        keywordText.ForceMeshUpdate();
        keywordNameText.ForceMeshUpdate();
    }
}
