#region Using Statements
using FTS.Characters;
using FTS.Turns;
using FTS.Grid;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
#endregion

namespace FTS.Cards
{
    /*
    * CLASS:       CardController
    * DESCRIPTION: Stores and controls the decks being used by the characters for the current battle
    */
    public class CardController : MonoBehaviour
    {
        public static event System.Action<Card> OnCardPlayed = delegate { };
        public static event System.Action OnCardDrawn = delegate { };
        public static event System.Action OnCardCreated = delegate { };
        public static event System.Action OnEnergyChanged = delegate { };
        List<Card> deck = new List<Card>();

        [SerializeField] int maxHandSize = 10;
        [SerializeField] int cardsPerTurn = 5;



        GameUI gameUI;
        UnitController unitController;
        CardDatabase cardDatabase;
        HandController hand;
        HexGridController grid;
        HexCell target;

        Card lastCardPlayed;
        Card cardSelected;
        int energy;
        int totalEnergy = 4;
        private CharacterClass characterClass;

        bool handleDiscard = false;
        int numberToDiscard = 0;

        #region Properties
        public Card CardSelected   // property
        {
            get { return cardSelected; }   // get method
            set { cardSelected = value; }  // set method
        }

        public int Energy   // property
        {
            get { return energy; }   // get method
            set { energy = value;
                OnEnergyChanged?.Invoke();
            }  // set method
        }

        public int TotalEnergy   // property
        {
            get { return totalEnergy; }   // get method
            set { totalEnergy = value; }  // set method
        }

        public int MaxHandSize   // property
        {
            get { return maxHandSize; }   // get method
            set { maxHandSize = value; }  // set method
        }

        public int CardsPerTurn   // property
        {
            get { return cardsPerTurn; }   // get method
            set { cardsPerTurn = value; }  // set method
        }
        #endregion

        #region MonoBehaviour Callbacks

        /*
        * FUNCTION    : Awake()
        * DESCRIPTION : 
        * PARAMETERS  :
        *		VOID
        * RETURNS     :
        *		VOID
        */
        void Awake()
        {
            gameUI = FindObjectOfType<GameUI>().GetComponent<GameUI>();
            hand = FindObjectOfType<HandController>().GetComponent<HandController>();
            grid = FindObjectOfType<HexGridController>().GetComponent<HexGridController>();
            //turnController = FindObjectOfType<TurnController>().GetComponent<TurnController>();
            unitController = FindObjectOfType<UnitController>().GetComponent<UnitController>();
            cardDatabase = FindObjectOfType<CardDatabase>().GetComponent<CardDatabase>();
            TurnController.OnNewTurn += TurnController_OnNewTurn;
            TurnController.OnCombatStart += TurnController_OnCombatStart;
            TurnController.OnEnemyTurn += TurnController_OnEnemyTurn;
            UnitController.OnUnitTurn += UnitController_OnUnitTurn;
        }

        /*
        * FUNCTION    : Start()
        * DESCRIPTION : Start is called once before the first frame update.
        * PARAMETERS  :
        *		VOID
        * RETURNS     :
        *		VOID
        */
        private void Start()
        {

        }

        private void Update()
        {
            if (handleDiscard && Input.GetMouseButtonDown(0))
            {
                Card toDiscard = MouseOverCard();
                if (toDiscard != null)
                {
                    CardDiscarded(toDiscard);
                    numberToDiscard--;
                }
                if (numberToDiscard <= 0)
                {
                    handleDiscard = false;
                    gameUI.DisablePlayerInfo();
                }
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                List<Card> inDeck = deck.FindAll(item => item.Location == CardLocation.Deck);
                foreach (var item in inDeck)
                {
                    Debug.Log(item.name);
                }
            }
        }

        private void OnDestroy()
        {
            TurnController.OnNewTurn -= TurnController_OnNewTurn;
            TurnController.OnCombatStart -= TurnController_OnCombatStart;
            TurnController.OnEnemyTurn -= TurnController_OnEnemyTurn;
            UnitController.OnUnitTurn -= UnitController_OnUnitTurn;
        }
        #endregion

        #region Private Methods
        private void CardPlayed(Card playedCard)
        {
            if (hand)
            {
                hand.RemoveCard(playedCard);
            }
            Energy -= playedCard.Cost;
            if (playedCard.IsAtomize)
            {
                playedCard.Location = CardLocation.Atomized;
            }
            else
            {
                playedCard.Location = CardLocation.Discard;
            }
            gameUI.UpdateDeckList();
            OnCardPlayed?.Invoke(playedCard);
            lastCardPlayed = playedCard;
        }

        private void CardDiscarded(Card discardedCard)
        {
            foreach (var item in discardedCard.OnDiscardEffects)
            {
                item.ActivateEffect(unitController.CurrentPlayer);
            }
            discardedCard.Location = CardLocation.Discard;
            hand.RemoveCard(discardedCard);
            gameUI.UpdateDeckList();
        }

        private bool HasEnergy(int cost)
        {
            bool hasEnergy = false;
            if (cost <= energy)
            {
                hasEnergy = true;
            }
            return hasEnergy;
        }

        private bool IsInRange(int range)
        {
            bool isInRange = false;
            //check grid if position is valid
            target = grid.GetCardTarget();
            Debug.Log("range " + range);
            if (target != null && grid.GetDistance(target) <= range)
            {
                isInRange = true;
            }
            return isInRange;
        }

        private bool IsTargetValid(CardTargeting targeting)
        {
            return grid.IsTargetValid(target, targeting);
        }

        private bool canDraw()
        {
            bool canDraw = true;
            if (deck.Count(item => item.Location == CardLocation.Hand) > maxHandSize)
            {
                canDraw = false;
            }
            else if (!deck.Any(item => item.Location == CardLocation.Deck &&
                    (item.CharacterClass == CharacterClass.Common ||
                     item.CharacterClass == unitController.GetCurrentUnit().CharacterClass)))
            {
                if (deck.Any(item => item.Location == CardLocation.Discard))
                {
                    ShuffleDeck(0);
                    canDraw = true;
                }
                else
                {
                    canDraw = false;
                }
            }
            return canDraw;
        }

        private void DiscardHand()
        {
            hand.DiscardHand();
            foreach (Card card in deck)
            {
                if (card.Location == CardLocation.Hand)
                {
                    card.Location = CardLocation.Discard;
                }
            }
        }

        private void DrawNewHand()
        {
            //Debug.Log("draw hand");
            for (int i = 0; i < cardsPerTurn; i++)
            {
                DrawCard();
            }
        }

        //Fisher-Yates shuffle
        private void ShuffleDeck(int ignoreCount)
        {
            foreach (Card card in deck)
            {
                if (card.Location == CardLocation.Discard)
                {
                    card.Location = CardLocation.Deck;
                }
            }

            int n = deck.Count - ignoreCount;
            while (n > 1)
            {
                n--;

                int k = UnityEngine.Random.Range(ignoreCount, n + 1);
                Card value = deck[k];
                deck[k] = deck[n];
                deck[n] = value;
            }
        }

        private Card MouseOverCard()
        {
            Card overCard = null;
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, raycastResults);

            if (raycastResults.Count > 0 && raycastResults[0].gameObject.tag == "Card")
            {
                string cardID = raycastResults[0].gameObject.GetComponentInParent<CardUI>().CardID;
                overCard = deck.Find(item => item.Id == cardID);
            }
            return overCard;
        }
        #endregion

        #region Public Methods
        public bool CanPlay(Card card)
        {
            bool canPlay = false;
            if (HasEnergy(card.Cost) && 
                card.Effects.Count > 0 && 
                (card.CharacterClass == CharacterClass.Common 
                || card.CharacterClass == characterClass))
            {
                canPlay = true;
            }

            return canPlay;
        }


        public void PlayCard(string cardId)
        {
            Card playedCard = deck.Find(item => item.Id == cardId);
            if(HasEnergy(playedCard.Cost) && 
                playedCard.Effects.Count > 0) 
            {
                //Debug.Log(playedCard.Id);
                if (playedCard.Targeting == CardTargeting.None)
                {
                    //Debug.Log("played non-targeting card");
                    CardPlayed(playedCard);
                    playedCard.Play(unitController.GetCurrentUnit());
                    //playedCard.Play(unitController.GetCurrentPlayer());
                }
                else
                {
                    if (IsInRange(playedCard.Range) && IsTargetValid(playedCard.Targeting))
                    {
                        CardPlayed(playedCard);
                        switch (playedCard.Targeting)
                        {
                            case CardTargeting.None:
                                break;
                            case CardTargeting.Unit:
                                playedCard.Play(target.Unit);
                                break;
                            case CardTargeting.Ground:
                                playedCard.Play(target);
                                break;
                            case CardTargeting.FromPlayer:
                                playedCard.Play(unitController.CurrentPlayer, target);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        public void DiscardCard(int cardsDiscarded, bool random)
        {
            List<Card> handBuffer = deck.FindAll(item => item.Location == CardLocation.Hand);
            if (random)
            {
                for (int i = 0; i < cardsDiscarded; i++)
                {          
                    Card discardedCard = handBuffer[UnityEngine.Random.Range(0, handBuffer.Count)];
                    CardDiscarded(discardedCard);
                }
            }
            else
            {
                if(handBuffer.Count <= cardsDiscarded)
                {
                    foreach (var item in handBuffer)
                    {
                        CardDiscarded(item);
                    }
                }
                else
                {
                    Debug.Log("handle discard");
                    
                    handleDiscard = true;
                    numberToDiscard = cardsDiscarded;
                    string info;
                    if (cardsDiscarded == 1)
                    {
                        info = "Discard a card";
                    }
                    else
                    {
                        info = "Discard "+ cardsDiscarded + " cards";
                    }
                    gameUI.EnablePlayerInfo(info);
                }
            }
        }

        //public void AddBucket(List<Card> cards)
        //{
        //    foreach (Card card in cards)
        //    {
        //        decks[card.CharacterClass].Add(Object.Instantiate(card));
        //    }
        //}

        public void AddCard(Card newCard)
        {
            deck.Add(Instantiate(newCard));
            //ShuffleDeck();
        }

        internal void AddCard(Card card, bool isTemporary, CardLocation cardLocation)
        {
            Card newCard = Instantiate(card);
            deck.Add(newCard);
            newCard.Location = cardLocation;
            newCard.IsAtomize = isTemporary;
            newCard.IsTemporary = isTemporary;
            if(cardLocation == CardLocation.Hand)
                hand.AddCard(newCard);
            OnCardCreated?.Invoke();
        }

        public void RemoveCard(string cardId)
        {
            Card removedCard = deck.Find(item => item.Id == cardId);
            if (removedCard.Location == CardLocation.Hand)
            {
                hand.RemoveCard(removedCard);
            }
            deck.Remove(removedCard);
        }

        public void RemoveClass(CharacterClass characterClass)
        {
            List<Card> cardsToRemove = deck.FindAll(item => item.CharacterClass == characterClass);
            foreach (var card in cardsToRemove)
            {
                card.Location = CardLocation.Atomized;
            }
        }

        internal Card GetCard(string cardId)
        {
            return deck.Find(item => item.Id == cardId);
        }

        public void DrawCard()
        {
            //Debug.Log("draw?");
            if (canDraw())
            {
                //Debug.Log("can draw?");
                Card card = deck.FirstOrDefault(item => item.Location == CardLocation.Deck && 
                                                (item.CharacterClass == CharacterClass.Common ||
                                                item.CharacterClass == unitController.GetCurrentUnit().CharacterClass));
                card.Location = CardLocation.Hand;
                hand.AddCard(card);
                OnCardDrawn?.Invoke();
                foreach (var effect in card.OnDrawEffects)
                {
                    effect.ActivateEffect(unitController.CurrentPlayer);
                }
            }
        }

        public void DrawCard(int numCards)
        {
            for (int i = 0; i < numCards; i++)
            {
                if (deck.Count(item => item.Location == CardLocation.Hand) < maxHandSize)
                {
                    DrawCard();
                }
                else
                {
                    break;
                }
            }
        }

        internal void ReduceCost(CostTarget costTarget, int costChange)
        {
            switch (costTarget)
            {
                case CostTarget.AllCopies:
                    foreach (var item in deck.FindAll(item => item.name == lastCardPlayed.name))
                    {
                        item.Cost += costChange;
                        if(item.Cost <= 0)
                        {
                            item.Cost = 0;
                        }
                    }
                    break;
                case CostTarget.ThisCard:
                    lastCardPlayed.Cost += costChange;
                    if (lastCardPlayed.Cost <= 0)
                    {
                        lastCardPlayed.Cost = 0;
                    }
                    break;
                case CostTarget.Random:
                    List<Card> cardsInHand = deck.FindAll(item => item.Location == CardLocation.Hand && 
                                                                  item.Cost > 0);
                    if (cardsInHand != null)
                    {
                        cardsInHand[UnityEngine.Random.Range(0, cardsInHand.Count - 1)].Cost += costChange;
                    }
                    break;
                default:
                    break;
            }
        }


        internal void PlaceOnTopOfDeck(string cardID)
        {          
            deck = deck.OrderBy(item => Guid.NewGuid()).ToList();
            deck = deck.OrderByDescending(item => item.Id == cardID).ToList();
        }



        internal void CardSelecte(string cardId)
        {
            cardSelected = deck.Find(item => item.Id == cardId);
        }

        public int GetCardCountInDeck()
        {
            return deck.Where(item => item.Location == CardLocation.Deck)
                                        .Count();
        }

        public int GetCardCountInDiscard()
        {
            return deck.Where(item => item.Location == CardLocation.Discard)
                                        .Count();
        }
        //exhaust 
        public int GetCardCountAtomized()
        {
            return deck.Where(item => item.Location == CardLocation.Atomized)
                                        .Count();
        }

        public List<Card> GetDeck()
        {
            return deck;
        }
        #endregion

        #region Events

        private void UnitController_OnUnitTurn(Character character)
        {
            DiscardHand();
            if (character is Player)
            {
                energy = totalEnergy;
                DrawNewHand();
            }
        }

        private void TurnController_OnCombatStart()
        {
            foreach (var card in cardDatabase.GetDeck())
            {
                AddCard(card);
            }
            deck = deck.OrderBy(item => Guid.NewGuid()).ToList();
            deck = deck.OrderByDescending(item => item.IsInherent).ToList();
        }

        private void TurnController_OnEnemyTurn()
        {
            DiscardHand();
            handleDiscard = false;
            numberToDiscard = 0;
        }

        private void TurnController_OnNewTurn()
        {
            //Debug.Log("new turn?");
            //energy = totalEnergy;
            //DrawNewHand();
        }
        #endregion
    }
}