#region Using Statements
using SP.Cards;
using UnityEngine;
using UnityEngine.UI;
#endregion

public class DraftCardInfo : MonoBehaviour
{
    [Header("Draft")]
    [SerializeField] Text draftCardName;
    [SerializeField] Text draftCost;
    [SerializeField] Image draftBorder;
    [SerializeField] Image draftArt;

    [Header("Card Zoom")]
    [SerializeField] Text cardName;
    [SerializeField] Text rulesText;
    [SerializeField] Text cost;
    [SerializeField] Image border;
    [SerializeField] Image image;

    private CardTargeting targeting;
    private string cardId;
    Card cardInfo;

    #region Properties
    public Card Card  // property
    {
        get { return cardInfo; }   // get method
        set { cardInfo = value; }  // set method
    }
    public string Name  // property
    {
        get { return cardName.text; }   // get method
        set { cardName.text = value; }  // set method
    }

    public string CardID  // property
    {
        get { return cardId; }   // get method
        set { cardId = value; }  // set method
    }

    public CardTargeting Targeting  // property
    {
        get { return targeting; }   // get method
        set { targeting = value; }  // set method
    }
    #endregion

    #region Public Methods
    public void FillCard(Card card)
    {
        cardInfo = card;
        draftCardName.text = card.CardName;
        draftCost.text = card.Cost.ToString();
        draftBorder.color = card.DraftBorder;
        draftArt.sprite = card.Image;

        GetComponent<Image>().color = card.DraftBorder;
        cardName.text = card.CardName;
        rulesText.text = card.RulesText;
        cost.text = card.Cost.ToString();
        border.sprite = card.Border;
        image.sprite = card.Image;
        cardId = card.Id;
        targeting = card.Targeting;
    }
    #endregion
}
