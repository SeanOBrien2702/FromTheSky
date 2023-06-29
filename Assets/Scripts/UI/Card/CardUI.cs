﻿using FTS.Cards;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using FTS.Characters;

namespace FTS.UI
{
    public class CardUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] GameObject highlight;
        [SerializeField] TextMeshProUGUI cardName;
        [SerializeField] TextMeshProUGUI rulesText;
        [SerializeField] TextMeshProUGUI typeText;
        //[SerializeField] Text rulesText;
        [SerializeField] Image border;
        [SerializeField] Image cardArt;
        [SerializeField] Image rarity;

        [Header("Range")]
        [SerializeField] GameObject rangeBorder;
        [SerializeField] Image rangeSymbol;
        [SerializeField] TextMeshProUGUI rangeText;
        [SerializeField] Sprite projectile;
        [SerializeField] Sprite piercing;

        [Header("Energy")]
        [SerializeField] GameObject energySymbol;
        [SerializeField] TextMeshProUGUI cost;

        [Header("Unit")]
        [SerializeField] GameObject healthImage;
        [SerializeField] GameObject movementImage;
        [SerializeField] TextMeshProUGUI healthText;
        [SerializeField] TextMeshProUGUI movementText;

        [Header("Colour")]
        [SerializeField] Color[] cardRarityColour;
        private CardTargeting targeting;
        private CardType type;
        private string cardId;
        Card cardInfo;
        bool overTooltip = false;
        string currentTooltip = null;
        TooltipUI tooltipUI;
        ShopCardUI shopUI;

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

        private void Awake()
        {
            tooltipUI = GetComponent<TooltipUI>();
            shopUI= GetComponent<ShopCardUI>();
            CardController.OnCardCreated += CardController_OnCardCreated;
            CardController.OnCardDrawn += CardController_OnCardDrawn;
            CardController.OnCardPlayed += CardController_OnCardPlayed;
        }

        private void OnDestroy()
        {
            CardController.OnCardCreated -= CardController_OnCardCreated;
            CardController.OnCardDrawn -= CardController_OnCardDrawn;
            CardController.OnCardPlayed -= CardController_OnCardPlayed;
        }

        private void Update()
        {
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(rulesText, Input.mousePosition, null);
            if (linkIndex > -1)
            {
                TMP_LinkInfo linkInfo = rulesText.textInfo.linkInfo[linkIndex];
                string linkID = linkInfo.GetLinkText();
                if (linkID != currentTooltip)
                {
                    //Debug.Log("show tool tip");
                    currentTooltip = linkID;
                    //tooltipUI.ShowToolTip(currentTooltip);
                }
            }
            else if (currentTooltip != null)
            {
                currentTooltip = null;
                //tooltipUI.HideTooltip();
            }
        }

        private void ConfigureCardType(Card card)
        {
            switch (card.Targeting)
            {
                case CardTargeting.None:
                    rangeBorder.SetActive(false);
                    break;
                case CardTargeting.Unit:
                    rangeSymbol.gameObject.SetActive(false);
                    rangeText.text = card.Range.ToString();
                    break;
                case CardTargeting.Ground:
                    rangeSymbol.gameObject.SetActive(false);
                    rangeText.text = card.Range.ToString();
                    break;
                case CardTargeting.Projectile:
                    tooltipUI.CreateTooltip(projectile, "Projectile", "Damage the first unit in a direction");
                    rangeSymbol.sprite = projectile;
                    rangeText.gameObject.SetActive(false);
                    break;
                case CardTargeting.Piercing:
                    tooltipUI.CreateTooltip(piercing, "Piercing", "Damage all units in a direction");
                    rangeSymbol.sprite = piercing;
                    rangeText.gameObject.SetActive(false);
                    break;
                case CardTargeting.Trajectory:
                    rangeText.gameObject.SetActive(false);
                    break;
            }

            if (card.Type == CardType.Summon)
            {
                healthImage.SetActive(true);
                movementImage.SetActive(true);
                healthText.text = card.GetUnitInfo(Stat.Health);
                movementText.text = card.GetUnitInfo(Stat.Movement);
            }

            if (card.Effects.Count <= 0)
            {
                energySymbol.SetActive(false);
            }
        }

        private string FillCardText(Card card)
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
                if (card.IsTemporary)
                {
                    rulesText.text += "\n<link=temporary><color=\"red\">Temporary</color></link>";
                }
                else
                {
                    rulesText.text += "\n<link=atomize><color=\"red\">Atomize</color></link>";
                }
            }
            return rulesText.text;
        }

        public void SaveCardData(Card card)
        {
            cardInfo = card;
            cardName.text = card.CardName;
            typeText.text = card.Type.ToString();
            cost.text = card.Cost.ToString();
            border.sprite = card.Border;
            if (card.Image)
            {
                cardArt.sprite = card.Image;
            }
            cardId = card.Id;
            targeting = card.Targeting;
            type = card.Type;
            rarity.color = cardRarityColour[(int)card.Rarity];

            if (shopUI)
            {
                shopUI.SetCost(card.Rarity);
            }
        }

        public void FillCardUI(Card card)
        {
            SaveCardData(card);
            ConfigureCardType(card);
            string cardText = FillCardText(card);
            tooltipUI.CreateTooltips(cardText);
        }

        public void HighlightCard(bool enable)
        {
            highlight.SetActive(enable);
        }

        #region Events
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                int linkIndex = TMP_TextUtilities.FindIntersectingLink(rulesText, Input.mousePosition, null);
                if (linkIndex > -1)
                {
                    TMP_LinkInfo linkInfo = rulesText.textInfo.linkInfo[linkIndex];
                    string linkID = linkInfo.GetLinkText();
                    Debug.Log(linkID);
                    //tooltipUI.ShowToolTip(linkID);
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

        private void CardController_OnCardPlayed(Card arg1, Characters.Player arg2)
        {
            FillCardText(cardInfo);
        }

        private void CardController_OnCardDrawn()
        {
            FillCardText(cardInfo);
        }

        private void CardController_OnCardCreated()
        {
            FillCardText(cardInfo);
        }
        #endregion
    }
}