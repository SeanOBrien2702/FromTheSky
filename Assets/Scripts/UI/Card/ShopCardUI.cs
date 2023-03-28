using FTS.Cards;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FTS.UI
{
    public class ShopCardUI : MonoBehaviour
    {
        [SerializeField] CardCost cost;
        [SerializeField] TextMeshProUGUI costText;
        int cardCost;

        public int CardCost { get => cardCost; set => cardCost = value; }

        internal void SetCost(CardRarity rarity)
        {
            cardCost = cost.GetCost(rarity);
            costText.text = cardCost.ToString();
        }
    }
}