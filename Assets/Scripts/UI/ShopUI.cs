#region Using Statements
using UnityEngine;
using FTS.Cards;
using FTS.Saving;
using System.Collections.Generic;
using System;
using FTS.Turns;
using FTS.Characters;
using UnityEngine.UI;
using FTS.Core;
#endregion

namespace FTS.UI
{
    public class ShopUI : MonoBehaviour
    {
        CardDatabase cardDB;
        RunController runController;

        [SerializeField] GameObject shopCardPrefab;
        [SerializeField] Transform cardPanel;
        [SerializeField] int numberShopCards = 4;

        [Header("Convertions")]
        [SerializeField] Button convertHealth;


        [SerializeField] Button convertCinder;

        #region MonoBehaviour Callbacks
        void Start()
        {
            cardDB = FindObjectOfType<CardDatabase>().GetComponent<CardDatabase>();
            runController = FindObjectOfType<RunController>().GetComponent<RunController>();
            runController.Cinder = 100;
            FillShopPanel();
        }
        #endregion

        #region Private Methods
        public void FillShopPanel()
        {
            List<Card> cards = cardDB.GetMultipleCards(numberShopCards);

            foreach (Card card in cards)
            {
                GameObject go = (GameObject)Instantiate(shopCardPrefab);
                go.transform.SetParent(cardPanel, false);
                go.GetComponentInChildren<CardUI>().SaveCardData(card);
                int cost = go.GetComponentInChildren<ShopCardUI>().CardCost;
                Button btn = go.GetComponent<Button>();
                btn.onClick.AddListener(() => {
                    BuyCard(go, card, cost);
                });
            }
        }

        public void BuyCard(GameObject go ,Card card, int cost) 
        {
            if (runController.Cinder >= cost)
            {
                runController.Cinder -= cost;
                cardDB.AddCardToDeck(card);
                Destroy(go);
            }
        }

        void DisableButtons()
        {
            convertHealth.interactable = false;
            convertCinder.interactable = false;
        }
        #endregion

        #region Public Methods
        public void ExitShop()
        {
            SceneController.Instance.LoadScene(Scenes.HubScene);
        }

        public void ConvertCinder()
        {
            if (runController.Cinder >= 50)
            {
                runController.Cinder -= 50;
                runController.Health += 10;
                DisableButtons();
            }
        }

        public void ConvertHealth()
        {
            if (runController.Health >= 15)
            {
                runController.Health -= 15;
                runController.Cinder += 25;
                DisableButtons();
            }
        }
        #endregion
    }
}
