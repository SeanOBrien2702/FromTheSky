#region Using Statements
using FTS.Characters;
using FTS.Turns;
using FTS.Grid;
using FTS.UI;
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
        public static event System.Action<Card, Player> OnCardPlayed = delegate { };
        public static event System.Action OnCardDrawn = delegate { };
        public static event System.Action OnCardCreated = delegate { };
        List<Card> deck = new List<Card>();

        [SerializeField] int maxHandSize = 10;
        [SerializeField] int cardsPerTurn = 5;

        [Header("CardSounds")]
        [SerializeField] SFXObject attackSFX;
        [SerializeField] SFXObject supportSFX;
        [SerializeField] SFXObject summonSFX;


        GameUI gameUI;
        Player player;
        CardDatabase cardDatabase;
        HandController hand;
        HexGridController grid;
        HexCell target;

        Card lastCardPlayed;
        Card cardSelected;
        private CharacterClass characterClass;

        bool handleDiscard = false;
        int numberToDiscard = 0;

        #region Properties
        public Card CardSelected   // property
        {
            get { return cardSelected; }   // get method
            set { cardSelected = value; }  // set method
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
            cardDatabase = FindObjectOfType<CardDatabase>().GetComponent<CardDatabase>();
            TurnController.OnPlayerTurn += TurnController_OnNewTurn;
            TurnController.OnEnemyTurn += TurnController_OnEnemyTurn;
            UnitController.OnSelectPlayer += UnitController_OnSelectPlayer;
            UnitController.OnSelectUnit += UnitController_OnSelectUnit;
            FillDeck();
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
        }

        private void OnDestroy()
        {
            TurnController.OnPlayerTurn -= TurnController_OnNewTurn;
            TurnController.OnEnemyTurn -= TurnController_OnEnemyTurn;
            UnitController.OnSelectPlayer += UnitController_OnSelectPlayer;
            UnitController.OnSelectUnit += UnitController_OnSelectUnit;
        }
        #endregion

        #region Private Methods
        private void CardPlayed(Card playedCard)
        {
            PlayCardSound(playedCard.Type);
            if (hand)
            {
                hand.RemoveCard(playedCard);
            }
            player.Energy -= playedCard.Cost;
            if (playedCard.IsAtomize)
            {
                playedCard.Location = CardLocation.Atomized;
            }
            else
            {
                playedCard.Location = CardLocation.Discard;
            }
            gameUI.UpdateDeckList();
            OnCardPlayed?.Invoke(playedCard, player);
            lastCardPlayed = playedCard;
        }

        private void CardDiscarded(Card discardedCard)
        {
            foreach (var item in discardedCard.OnDiscardEffects)
            {
                item.ActivateEffect(player);
            }
            discardedCard.Location = CardLocation.Discard;
            hand.RemoveCard(discardedCard);
            gameUI.UpdateDeckList();
        }

        private bool HasEnergy(int cost)
        {
            bool hasEnergy = false;
            if (cost <= player.Energy)
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
            else if (!deck.Any(item => item.Location == CardLocation.Deck))
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
            for (int i = 0; i <= cardsPerTurn; i++)
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

        void PlayCardSound(CardType type)
        {
            switch (type)
            {
                case CardType.Attack:
                    SFXManager.Main.Play(attackSFX);
                    break;
                case CardType.Support:
                    SFXManager.Main.Play(attackSFX);
                    break;
                case CardType.Summon:
                    SFXManager.Main.Play(attackSFX);
                    break;
                default:
                    break;
            }
        }
        
        private void FillDeck()
        {
            foreach (var card in cardDatabase.GetDeck())
            {
                AddCard(card);
            }
            deck = deck.OrderBy(item => Guid.NewGuid()).ToList();
            deck = deck.OrderByDescending(item => item.IsInherent).ToList();
        }
        #endregion

        #region Public Methods
        public bool CanPlay(Card card)
        {
            bool canPlay = false;
            if (HasEnergy(card.Cost) && 
                card.Effects.Count > 0)
            {
                canPlay = true;
            }

            return canPlay;
        }


        public void PlayCard(string cardId)
        {
            Card playedCard = deck.Find(item => item.Id == cardId);
            if(player &&
                HasEnergy(playedCard.Cost) && 
                playedCard.Effects.Count > 0) 
            {
                if (playedCard.Targeting == CardTargeting.None)
                {
                    CardPlayed(playedCard);
                    playedCard.Play();
                }
                else
                {
                    if (IsInRange(playedCard.Range) && IsTargetValid(playedCard.Targeting))
                    {
                        CardPlayed(playedCard);

                        if(playedCard.Targeting == CardTargeting.Unit)
                        {
                            playedCard.Play(target.Unit);
                        }
                        else if(playedCard.Targeting != CardTargeting.Projectile)
                        {
                            playedCard.Play(target);
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

        internal void AddCard(Card card, bool isTemporary, CardLocation cardLocation, bool isFree = false)
        {
            Card newCard = Instantiate(card);        
            newCard.Location = cardLocation;
            newCard.IsAtomize = isTemporary;
            newCard.IsTemporary = isTemporary;
            if(isFree)
            {
                newCard.Cost = 0;
            }
            if(cardLocation == CardLocation.Hand)
                hand.AddCard(newCard);
            OnCardCreated?.Invoke();
            deck.Add(newCard);
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
            if (canDraw())
            {
                Card card = deck.FirstOrDefault(item => item.Location == CardLocation.Deck);
                card.Location = CardLocation.Hand;
                hand.AddCard(card);
                OnCardDrawn?.Invoke();
                foreach (var effect in card.OnDrawEffects)
                {
                    effect.ActivateEffect(player);
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

        internal void CardDrawPerTurn(int cardsDrawn)
        {
            cardsPerTurn += cardsDrawn;
        }

        internal int GetDamage()
        {
            int damage = 0;
            foreach (IDamageEffect item in CardSelected.Effects.OfType<IDamageEffect>())
            {
                damage += item.GetTotalDamage();
            }
            return damage;
        }
        #endregion

        #region Events
        private void UnitController_OnSelectPlayer(Player newPlayer)
        {
            player = newPlayer;
        }

        private void UnitController_OnSelectUnit(Unit unit)
        {
            player = null;
        }

        private void TurnController_OnEnemyTurn(bool isTelegraph)
        {
            if (!isTelegraph)
            {
                DiscardHand();
                handleDiscard = false;
                numberToDiscard = 0;
            }
        }

        private void TurnController_OnNewTurn()
        {
            //Debug.Log("draw new hard?");
            //energy = totalEnergy;
            DrawNewHand();
        }

        internal bool IsFreeAim()
        {
            if((int)cardSelected.Targeting < (int)CardTargeting.Projectile)
            {
                return true;
            }
            else
            {
                return false;
            }            
        }
        #endregion
    }
}