using FTS.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Cards
{
    public class DecreaseCardsDrawn : MonoBehaviour
    {
        CardController cardController;
        [SerializeField] int cardReduction = -1;

        void Start()
        {
            cardController = FindObjectOfType<CardController>().GetComponent<CardController>();
        }

        private void OnDestroy()
        {
            cardController.CardDrawPerTurn(cardReduction);
        }
    }
}
