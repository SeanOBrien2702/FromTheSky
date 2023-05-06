using AeLa.EasyFeedback.APIs;
using FTS.Cards;
using FTS.Characters;
using FTS.Core;
using FTS.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI
{
    public class RemoveCardUI : MonoBehaviour
    {
        List<Card> cards = new List<Card>();
        CardDatabase cardDB;
        [SerializeField] Transform cardPanel;
        [SerializeField] Transform cardContentPanel;
        [SerializeField] GameObject cardPrefab;

        #region MonoBehaviour Callbacks
        void Start()
        {
            cardDB = FindObjectOfType<CardDatabase>().GetComponent<CardDatabase>();
            cards = cardDB.GetDeck();
            cardPanel.gameObject.SetActive(false);
            FillPanel();
        }
        #endregion

        #region Private Methods
        void FillPanel()
        {
            foreach (Card card in cards)
            {
                GameObject go = (GameObject)Instantiate(cardPrefab);
                go.gameObject.transform.localScale = Vector3.one;
                go.transform.SetParent(cardContentPanel, false);
                go.GetComponentInChildren<CardUI>().FillCardUI(card);
                Button btn = go.GetComponent<Button>();
                btn.onClick.AddListener(() =>
                {
                    RemoveCard(card);
                });
            }
        }

        public void RemoveCard(Card card)
        {
            cardDB.RemoveCardFromDeck(card);
            cardPanel.gameObject.SetActive(false);
        }
        #endregion

        #region Public Methods
        public void Skip()
        {
            cardPanel.gameObject.SetActive(false);
        }
        #endregion
    }
}