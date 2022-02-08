using FTS.Cards;
using FTS.Characters;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class CardUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject highlight;
    [SerializeField] TextMeshProUGUI cardName;
    [SerializeField] TextMeshProUGUI rulesText;
    //[SerializeField] Text rulesText;
    [SerializeField] Image border;
    [SerializeField] Image cardArt;

    [Header("Range")]
    [SerializeField] GameObject rangeSymbol;
    [SerializeField] Text range;

    [Header("Energy")]
    [SerializeField] GameObject energySymbol;
    [SerializeField] TextMeshProUGUI cost;
    private CardTargeting targeting;
    private CardType type;
    private string cardId;
    Card cardInfo;
    bool overTooltip = false;
    string currentTooltip = null;
    TooltipUI tooltipUI;

    public Card CardInfos // property
    {
        get { return cardInfo; }   // get method
        set { cardInfo = value; }  // set method
    }

    public string Name  // property
    {
        get { return cardName.text; }   // get method
        set { cardName.text = value; }  // set method
    }

    public string Range  // property
    {
        get { return range.text; }   // get method
        set { range.text = value; }  // set method
    }

    public string CardID  // property
    {
        get { return cardId; }   // get method
        set { cardId = value; }  // set method
    }

    public CardTargeting Targeting  // property
    {
        get { return targeting; }   // get method
    }

    public CardType Type  // property
    {
        get { return type; }   // get method
    }

    private void Start()
    {
        tooltipUI = FindObjectOfType<TooltipUI>().GetComponent<TooltipUI>();
    }

    private void Update()
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(rulesText, Input.mousePosition, null);
        if (linkIndex > -1)
        {
            TMP_LinkInfo linkInfo = rulesText.textInfo.linkInfo[linkIndex];
            string linkID = linkInfo.GetLinkText();
            if(linkID != currentTooltip)
            {
                //Debug.Log("show tool tip");
                currentTooltip = linkID;
                tooltipUI.ShowToolTip(currentTooltip);
            }
        }
        else if(currentTooltip != null)
        {
            currentTooltip = null;
            tooltipUI.HideTooltip();
        }
    }


    private void ConfigureCardType(Card card)
    {
        if(card.Targeting == CardTargeting.None)
        {
            rangeSymbol.SetActive(false);
            range.gameObject.SetActive(false);
        }
        else
        {
            range.text = card.Range.ToString();
        }

        if(card.Effects.Count <= 0)
        {
            energySymbol.SetActive(false);
        }
    }

    private void FillCardText(Card card)
    {
        rulesText.text = "";
        //configure rules text
        if (card.IsInherent)
        {
            rulesText.text += "<link=inherent><color=\"red\">Inherent</color></link>\n";
        }
        if (card.OnDrawEffects.Count > 0)
        {
            foreach (var item in card.OnDrawEffects)
            {
                rulesText.text += "On draw: ";
                rulesText.text += item.GetEffectText() + ". ";
            }
            rulesText.text += "\n";
        }
        foreach (var item in card.Effects)
        {
            rulesText.text += item.GetEffectText() + ". ";
        }
        if (card.OnDiscardEffects.Count > 0)
        {
            rulesText.text += "\n";
            foreach (var item in card.OnDiscardEffects)
            {
                rulesText.text += "On discard: ";
                rulesText.text += item.GetEffectText() + ". ";
            }
        }
        if (card.IsAtomize)
        {
            if(card.IsTemporary)
            {
                rulesText.text += "\n<link=temporary><color=\"red\">Temporary</color></link>";
            }
            else
            {
                rulesText.text += "\n<link=atomize><color=\"red\">Atomize</color></link>";
            }     
        }
    }

    public void SaveCardData(Card card)
    {
        cardName.text = card.CardName;
        cost.text = card.Cost.ToString();
        //Debug.Log(card.Border);
        border.sprite = card.Border;
        if (card.Image == null)
        {
            //cardArt.gameObject.SetActive(false);
        }
        else
        {
            cardArt.sprite = card.Image;
        }
        cardId = card.Id;
        targeting = card.Targeting;
        type = card.Type;

        foreach (var item in card.Effects)
        {
            rulesText.text += item.GetEffectText();
        }
    }

    public void FillCardUI(Card card)
    {
        cost.text = card.Cost.ToString();
        //Debug.Log(card.Type);
        ConfigureCardType(card);
        FillCardText(card);
        //Keywords(card.RulesText);
    }

    public void HighlightCard(bool enable)
    {
        highlight.SetActive(enable);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(rulesText, Input.mousePosition, null);
            if(linkIndex > -1)
            {
                TMP_LinkInfo linkInfo = rulesText.textInfo.linkInfo[linkIndex];
                string linkID = linkInfo.GetLinkText();
                Debug.Log(linkID);
                tooltipUI.ShowToolTip(linkID);
            }

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(rulesText, Input.mousePosition, null);
        if (linkIndex > -1)
        {
            TMP_LinkInfo linkInfo = rulesText.textInfo.linkInfo[linkIndex];
            string linkID = linkInfo.GetLinkText();
            overTooltip = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        overTooltip = false;
    }
}
