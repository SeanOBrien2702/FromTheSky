#region Using Statements
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FTS.Cards;
using FTS.Characters;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;
#endregion

namespace FTS.UI
{
    public class DraftUI : MonoBehaviour
    {
        List<DraftArchetypes> archetypes = new List<DraftArchetypes>();
        List<Card> cards = new List<Card>();
        int numCards = 3;

        CardDatabase cardDB;

        [SerializeField] Transform cardPanel;
        [SerializeField] GameObject cardPrefab;

        #region MonoBehaviour Callbacks
        void Awake()
        {
            cardDB = FindObjectOfType<CardDatabase>().GetComponent<CardDatabase>();
        }

        void Start()
        {
            FillPanel();
        }
        #endregion

        #region Private Methods
        void FillPanel()
        {
            for (int i = 0; i < numCards; i++)
            {
                int index = i;
                Card card = cardDB.GetRandomCard();
                cards.Add(card);
                GameObject go = (GameObject)Instantiate(cardPrefab);
                go.transform.SetParent(cardPanel, false);
                go.GetComponentInChildren<CardUI>().SaveCardData(card);
                Button btn = go.GetComponent<Button>();
                btn.onClick.AddListener(() => {
                    SelectCard(card);
                });
            }
        }
        #endregion

        #region Public Methods
        //When clicking on card to select. Add card to card data base
        public void SelectCard(Card card)
        {
            Debug.Log(card.name);
            cardDB.AddCardToDeck(card);
            SceneManager.LoadScene("EncounterSelection");
        }

        public void Skip()
        {
            SceneManager.LoadScene("EncounterSelection");
        }
        #endregion
    }
}
