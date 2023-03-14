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
        PlayerDatabase playerDB;

        [SerializeField] Transform cardPanel;
        [SerializeField] GameObject cardPrefab;
        [SerializeField] bool isOrbital = false;

        #region MonoBehaviour Callbacks
        void Awake()
        {
            cardDB = FindObjectOfType<CardDatabase>().GetComponent<CardDatabase>();
        }

        void Start()
        {
            if (!isOrbital)
            {
                cards = cardDB.PopulateDraftPicks(numCards);
                FillPanel();
            }
        }
        #endregion

        #region Private Methods
        void FillPanel()
        {
            //playerDB = FindObjectOfType<PlayerDatabase>().GetComponent<PlayerDatabase>();
            

            foreach (Card card in cards)
            {
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
            SceneManager.LoadScene(Scenes.HubScene.ToString());
        }

        public void Skip()
        {
            SceneManager.LoadScene(Scenes.HubScene.ToString());
        }

        //Click when slecting characters
        public void OrbitFillPanel()
        {
            cards = cardDB.GetOrbitalCards();
            FillPanel();
        }
        #endregion
    }
}
