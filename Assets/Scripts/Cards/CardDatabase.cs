using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SP.Characters;
using SP.Grid;
using System;

namespace SP.Cards
{
    public class CardDatabase : MonoBehaviour
    {
        PlayerDatabase playerDatabase;
        //List<Card> deck = new List<Card>();
        Dictionary<CharacterClass, List<Card>> lookupTable = null;
        [SerializeField] List<Card> deck = new List<Card>();
        [SerializeField] List<CardBorder> cardBorders;
        [SerializeField] CharacterClassCards[] characterClassCards;
        CharacterClass[] classList;
        int cardID = 0;

        #region MonoBehaviour Callbacks
        private void Start()
        {
            playerDatabase = FindObjectOfType<PlayerDatabase>().GetComponent<PlayerDatabase>();
            BuildLookup();
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
        //TODO: populate buckets based on archetypes
        public List<Card> PopulateBucket()
        {
            classList = playerDatabase.GetPlayerClasses();
            List<Card> bucket = new List<Card>();
            foreach (CharacterClass characterClass in classList)
            {
                bucket.Add(lookupTable[characterClass].OrderBy(o => System.Guid.NewGuid())
                                                      .FirstOrDefault());
            }
            return bucket;
        }

        internal Card GetRandomCard()
        {
            return deck.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        }

        internal List<Card> GetDeck()
        {
            foreach (var card in deck)
            {
                card.Id = cardID.ToString();
                card.Border = cardBorders.Find(item => item.characterClass == card.CharacterClass).border;
                //Debug.Log(card.Id);
                cardID++;
            }
            return deck;
        }

        internal void AddCardToDeck(Card selectedCard)
        {
            deck.Add(selectedCard);
        }
        #endregion
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
