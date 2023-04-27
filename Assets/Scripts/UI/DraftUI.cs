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
using FTS.Core;
using TMPro;
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

        [SerializeField] int skipCinder = 10;
        [SerializeField] TextMeshProUGUI skipButtonText;

        [Header("draft Options")]
        [SerializeField] Transform cardPanel;
        [SerializeField] GameObject cardPrefab;
        [SerializeField] bool isOrbital = false;


        #region MonoBehaviour Callbacks
        private void Awake()
        {
            
        }

        void Start()
        {
            cardDB = FindObjectOfType<CardDatabase>().GetComponent<CardDatabase>();
            skipButtonText.text = "Skip card\nAdd " + skipCinder + " Cinder";
            if (!isOrbital)
            {
                cards = cardDB.GetMultipleCards(numCards);
                FillPanel();
            }
        }
        #endregion

        #region Private Methods
        void FillPanel()
        {
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
            cardDB.AddCardToDeck(card);
            SceneController.Instance.LoadScene(Scenes.HubScene);
        }

        public void Skip()
        {
            RunController.Instance.Cinder += skipCinder;
            SceneController.Instance.LoadScene(Scenes.HubScene);
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
