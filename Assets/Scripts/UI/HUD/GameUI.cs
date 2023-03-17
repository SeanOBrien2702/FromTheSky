#region Using Statements
using FTS.Cards;
using FTS.Characters;
using FTS.Turns;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
#endregion

namespace FTS.UI
{
    public class GameUI : MonoBehaviour
    {
        [Header("Player Info")]
        [SerializeField] GameObject turnCounter;
        [SerializeField] TextMeshProUGUI turnText;
        [SerializeField] GameObject playerMessagePanel;
        [SerializeField] TextMeshProUGUI playerMessageText;

        [Header("Buttons")]
        [SerializeField] GameObject startCombatButton;
        [SerializeField] GameObject endTurnButton;

        [Header("Energy")]
        [SerializeField] GameObject energy;
        [SerializeField] TextMeshProUGUI energyText;

        [Header("Deck stacks")]
        [SerializeField] GameObject deck;
        [SerializeField] TextMeshProUGUI decksText;
        [SerializeField] GameObject discard;
        [SerializeField] TextMeshProUGUI discardText;
        [SerializeField] GameObject atomized;
        [SerializeField] TextMeshProUGUI atomizedText;

        [Header("Foretell/Create")]
        [SerializeField] GameObject foretellPanel;
        [SerializeField] GameObject cardPrefab;

        UnitController unitController;
        TurnController turnController;
        CardController cardController;

        #region MonoBehaviour Callbacks
        void Awake()
        {
            CardController.OnCardPlayed += CardController_OnCardPlayed;
            CardController.OnCardDrawn += CardController_OnCardDrawn;
            CardController.OnCardCreated += CardController_OnCardCreated;
            //CardController.OnEnergyChanged += CardController_OnEnergyChanged;
            TurnController.OnEnemyTurn += TurnController_OnEnemyTurn;
            TurnController.OnPlayerTurn += TurnController_OnNewTurn;
            UnitController.OnPlayerSelected += UnitController_OnPlayerSelected;
            turnController = FindObjectOfType<TurnController>().GetComponent<TurnController>();
            unitController = FindObjectOfType<UnitController>().GetComponent<UnitController>();
        }

        private void Start()
        {
            cardController = FindObjectOfType<CardController>().GetComponent<CardController>();
        }

        private void OnDestroy()
        {
            CardController.OnCardPlayed -= CardController_OnCardPlayed;
            CardController.OnCardDrawn -= CardController_OnCardDrawn;
            CardController.OnCardCreated -= CardController_OnCardCreated;
            //CardController.OnEnergyChanged -= CardController_OnEnergyChanged;
            TurnController.OnEnemyTurn -= TurnController_OnEnemyTurn;
            TurnController.OnPlayerTurn -= TurnController_OnNewTurn;
            UnitController.OnPlayerSelected -= UnitController_OnPlayerSelected;
        }
        #endregion

        #region Private Methods
        public void UpdateDeckList()
        {
            decksText.text = cardController.GetCardCountInDeck().ToString();
            discardText.text = cardController.GetCardCountInDiscard().ToString();
            atomizedText.text = cardController.GetCardCountAtomized().ToString();
        }

        void ToggleUI(bool enable)
        {
            energyText.gameObject.SetActive(enable);
            deck.SetActive(enable);
            discard.SetActive(enable);
            atomized.SetActive(enable);
            energy.SetActive(enable);
            endTurnButton.SetActive(enable);
            turnCounter.SetActive(enable);
            playerMessageText.text = "";
        }

        private void UpdateEnergy()
        {
            energyText.text = unitController.CurrentPlayer.Energy + "/" + unitController.CurrentPlayer.MaxEnergy;
        }
        #endregion

        #region Public Methods
        //Button
        public void StartCombat()
        {
            turnController.StartCombat();
            //turnCounter.SetActive(true);
            DisablePlayerInfo();
            Destroy(startCombatButton);
        }

        public void EnablePlayerInfo(string info)
        {
            playerMessagePanel.SetActive(true);
            playerMessageText.text = info;
        }
        internal void DisablePlayerInfo()
        {
            playerMessagePanel.SetActive(false);
            playerMessageText.text = "";
        }

        public void ToggleForetell(List<Card> cards)
        {
            if (cards != null && cards.Count > 0)
            {
                foretellPanel.SetActive(true);
                foreach (var card in cards)
                {
                    GameObject drawnCard = Instantiate<GameObject>(cardPrefab);
                    drawnCard.transform.SetParent(foretellPanel.transform, false);
                    drawnCard.transform.SetAsFirstSibling();
                    drawnCard.transform.localScale = new Vector3(0.5f, 0.5f, 1);

                    CardUI newCardUI = drawnCard.GetComponent<CardUI>();
                    newCardUI.SaveCardData(card);
                    newCardUI.FillCardUI(card);
                }
                EnablePlayerInfo("Place on top ");
            }
            else
            {
                foretellPanel.SetActive(false);
                foreach (Transform child in foretellPanel.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
                DisablePlayerInfo();
            }
        }
        #endregion

        #region Events
        private void CardController_OnCardDrawn()
        {
            UpdateDeckList();
        }

        private void UnitController_OnPlayerSelected()
        {
            UpdateEnergy();
        }

        private void CardController_OnCardPlayed(Card card)
        {
            Debug.Log("Card played?");
            UpdateDeckList();
            UpdateEnergy();
        }

        private void CardController_OnCardCreated()
        {
            UpdateDeckList();
        }

        private void CardController_OnEnergyChanged()
        {
            UpdateEnergy();
        }

        private void TurnController_OnNewTurn()
        {
            UpdateEnergy();
            UpdateDeckList();
            ToggleUI(true);
        }

        private void TurnController_OnEnemyTurn(bool isTelegraph)
        {
            endTurnButton.SetActive(false);
        }
        #endregion
    }
}

