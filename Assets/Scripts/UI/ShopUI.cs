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

        [SerializeField] GameObject shopCardPrefab;
        [SerializeField] Transform cardPanel;
        [SerializeField] int numberShopCards = 4;

        [Header("Remove card")]
        [SerializeField] GameObject removeCardPanel;
        [SerializeField] Button removeCardButton;

        [Header("Convertions")]
        [SerializeField] Button convertHealthButton;
        [SerializeField] Button convertCinderButton;

        #region MonoBehaviour Callbacks
        void Start()
        {
            cardDB = FindObjectOfType<CardDatabase>().GetComponent<CardDatabase>();
            FillShopPanel();
            removeCardPanel.SetActive(false);
            convertCinderButton.onClick.AddListener(ConvertCinder);
            convertHealthButton.onClick.AddListener(ConvertHealth);
            removeCardButton.onClick.AddListener(RemoveCard);
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
                go.GetComponentInChildren<CardUI>().FillCardUI(card);
                int cost = go.GetComponentInChildren<ShopCardUI>().CardCost;
                Button btn = go.GetComponent<Button>();
                btn.onClick.AddListener(() => {
                    BuyCard(go, card, cost);
                });
            }
        }

        public void BuyCard(GameObject go ,Card card, int cost) 
        {
            if (RunController.Instance.Cinder >= cost)
            {
                RunController.Instance.Cinder -= cost;
                cardDB.AddCardToDeck(card);
                Destroy(go);
            }
        }

        void DisableButtons()
        {
            convertHealthButton.interactable = false;
            convertCinderButton.interactable = false;
        }
        #endregion

        #region Public Methods
        public void ExitShop()
        {
            SceneController.Instance.LoadScene(Scenes.HubScene);
        }

        public void ConvertCinder()
        {
            Debug.Log("Cinder " + RunController.Instance.Cinder);
            if (RunController.Instance.Cinder > 50)
            {
                RunController.Instance.Cinder -= 50;
                RunController.Instance.Health += 5;
                DisableButtons();
            }
            Debug.Log("Cinder " + RunController.Instance.Cinder);
        }

        public void ConvertHealth()
        {
            if (RunController.Instance.Health > 10)
            {
                RunController.Instance.Health -= 10;
                RunController.Instance.Cinder += 25;
                DisableButtons();
            }
        }

        public void RemoveCard()
        {
            if (RunController.Instance.Cinder >= 100)
            {
                removeCardPanel.SetActive(true);
                RunController.Instance.Cinder -= 100;
                removeCardButton.interactable = false;
            }
        }
        #endregion
    }
}
