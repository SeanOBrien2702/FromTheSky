#region Using Statements
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FTS.Characters;
using FTS.Saving;
using FTS.Turns;
using FTS.UI;
using FTS.Core;

#endregion

namespace FTS.Cards
{
    public class CardDatabase : MonoBehaviour, ISaveable
    {
        //PlayerDatabase playerDatabase;
        //UnitController unitController;
        Dictionary<CharacterClass, List<Card>> lookupTable = null;
        [SerializeField] List<Card> deck = new List<Card>();
        [SerializeField] List<CardBorder> cardBorders;
        [SerializeField] List<CardRaritySettings> cardRaritySettings;     
        [SerializeField] CharacterClassCards[] characterClassCards;
        [SerializeField] Card testCard;
        CharacterClass[] classList;
        int cardID = 0;

        #region MonoBehaviour Callbacks
        private void Start()
        {
            //playerDatabase = FindObjectOfType<PlayerDatabase>().GetComponent<PlayerDatabase>();
            BuildLookup();
            TurnController.OnCombatStart += TurnController_OnCombatStart;
        }

        private void OnDestroy()
        {
            TurnController.OnCombatStart -= TurnController_OnCombatStart;
        }
        #endregion

        #region Private Methods
        private void BuildLookup()
        {
            if (lookupTable == null)
            {
                lookupTable = new Dictionary<CharacterClass, List<Card>>();

                foreach (CharacterClassCards characterClass in characterClassCards)
                {
                    var statLookupTable = new List<Card>();
                    foreach (Card card in characterClass.cards)
                    {
                        card.Border = cardBorders.Find(item => item.characterClass == characterClass.characterClass).border;
                        card.DraftBorder = cardBorders.Find(item => item.characterClass == characterClass.characterClass).draftColor;
                        statLookupTable.Add(card);                   
                    }
                    lookupTable[characterClass.characterClass] = statLookupTable;
                }
            }
        }
        #endregion

        #region Public Methods
        public List<Card> GetMultipleCards(int numPicks)
        {
            CardRaritySettings settings = cardRaritySettings[RunController.Instance.GetDifficultyScale()];

            List<Card> cards = new List<Card>();
            while(cards.Count < numPicks)
            {

                int randomNumber = Random.Range(0, settings.GetChanceTotal());
                Card card;
                if (randomNumber <= settings.GetCommonChance())
                {
                    card = lookupTable[CharacterClass.Scout].Where(item => item.Rarity == CardRarity.Common).OrderBy(a => System.Guid.NewGuid()).FirstOrDefault();
                }
                else if (randomNumber > settings.GetCommonChance() && randomNumber <= settings.GetUncommonChance())
                {
                    card = lookupTable[CharacterClass.Scout].Where(item => item.Rarity == CardRarity.Uncommon).OrderBy(a => System.Guid.NewGuid()).FirstOrDefault();
                }
                else if (randomNumber > settings.GetUncommonChance() && randomNumber <= settings.GetRareChance())
                {
                    card = lookupTable[CharacterClass.Scout].Where(item => item.Rarity == CardRarity.Rare).OrderBy(a => System.Guid.NewGuid()).FirstOrDefault();
                }
                else
                {
                    card = lookupTable[CharacterClass.Scout].Where(item => item.Rarity == CardRarity.Ledendary).OrderBy(a => System.Guid.NewGuid()).FirstOrDefault();
                }

                if(!cards.Contains(card))
                {
                    cards.Add(card);
                }
            }

            return cards; // .OrderBy(item => random.Next()).Take(numPicks).ToList();
        }

        internal List<Card> GetOrbitalCards()
        {
            //classList = playerDatabase.GetPlayerClasses();
            List<Card> bucket = new List<Card>();

            for (int i = 0; i < 2; i++)
            {
                bucket.Add(lookupTable[classList[Random.Range(0, classList.Length)]].OrderBy(o => System.Guid.NewGuid())
                                                     .FirstOrDefault());
            }
            return bucket;
        }

        internal Card GetRandomCard()
        {
            return deck.OrderBy(x => System.Guid.NewGuid()).FirstOrDefault();
        }

        internal List<Card> GetDeck()
        {
            foreach (var card in deck)
            {        
                card.Id = cardID.ToString();
                card.Border = cardBorders.Find(item => item.characterClass == card.CharacterClass).border;
                cardID++;
            }
            return deck;
        }

        internal void AddCardToDeck(Card selectedCard)
        {
            deck.Add(selectedCard);
        }

        internal void RemoveCardFromDeck(Card selectedCard)
        {
            deck.Remove(selectedCard);
        }

        public object CaptureState()
        {
            return deck;
        }

        public void RestoreState(object state)
        { 
            deck.Clear();
            deck.AddRange((List<Card>)state);
        }
        #endregion

        private void TurnController_OnCombatStart()
        {
            foreach(CharacterClassCards characterClass in characterClassCards)
            {
                foreach (Card card in characterClass.cards)
                {
                    foreach (Effect effect in card.OnDrawEffects)
                    {
                        effect.Initialize();
                    }
                    foreach (Effect effect in card.Effects)
                    {
                        effect.Initialize();
                    }
                }
            }  
        }   
    }

    [System.Serializable]
    class CharacterClassCards
    {
        public CharacterClass characterClass;
        public List<Card> cards;
    }

    [System.Serializable]
    class CardBorder
    {
        public CharacterClass characterClass;
        public Sprite border;
        public Color draftColor;
    }
}
