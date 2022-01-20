#region Using Statements
using FTS.Characters;
using FTS.Grid;
using System.Collections.Generic;
using UnityEngine;
#endregion

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Card", fileName = "Card.asset")]
    public class Card : ScriptableObject
    {
        string id;
        string rulesText;

        [Header("Flavour")]
        [SerializeField] string cardName;
        [SerializeField] string flavourText;
        [SerializeField] Sprite image;
        [SerializeField] Sprite border;
        Color draftBorder;

        [Header("Mechanics")]
        [SerializeField] int cost = 1;
        [SerializeField] int range = 2;      
        [SerializeField] CardRarity cardRarity;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] CardType type;
        List<DraftArchetypes> draftArchetypes;
        [SerializeField] CardTargeting targeting;
        [SerializeField] bool isAtomize;
        [SerializeField] bool isTemporary;

        CardLocation location = CardLocation.Deck;
        [SerializeField] List<Effect> onDrawEffects;
        [SerializeField] List<Effect> effects;
        [SerializeField] List<Effect> onDiscardEffects;

        #region Properties
        public CardTargeting Targeting  // property
        {
            get { return targeting; }   // get method
            set { targeting = value; }  // set method
        }

        public bool IsAtomize  // property
        {
            get { return isAtomize; }   // get method
            set { isAtomize = value; }  // set method
        }

        public bool IsTemporary  // property
        {
            get { return isTemporary; }   // get method
            set { isTemporary = value; }  // set method
        }

        public CardLocation Location  // property
        {
            get { return location; }   // get method
            set { location = value; }  // set method
        }

        public string Id  // property
        {
            get { return id; }   // get method
            set { id = value; }  // set method
        }
        public Sprite Border  // property
        {
            get { return border; }   // get method
            set { border = value; }  // set method
        }

        public Color DraftBorder  // property
        {
            get { return draftBorder; }   // get method
            set { draftBorder = value; }  // set method
        }

        public int Cost  // property
        {
            get { return cost; }   // get method
            set { cost = value; }  // set method
        }

        public int Range  // property
        {
            get { return range; }   // get method
            set { range = value; }  // set method
        }

        public Sprite Image   // property
        {
            get { return image; }   // get method
            set { image = value; }  // set method
        }

        public string CardName   // property
        {
            get { return cardName; }   // get method
            set { cardName = value; }  // set method
        }

        public string RulesText  // property
        {
            get { return rulesText; }   // get method
            set { rulesText = value; }  // set method
        }
        public CardRarity Rarity   // property
        {
            get { return cardRarity; }   // get method
            set { cardRarity = value; }  // set method
        }
        public CardType Type   // property
        {
            get { return type; }   // get method
            set { type = value; }  // set method
        }

        public CharacterClass CharacterClass   // property
        {
            get { return characterClass; }   // get method
            set { characterClass = value; }  // set method
        }

        public List<DraftArchetypes> DraftArchetypes   // property
        {
            get { return draftArchetypes; }   // get method
            set { draftArchetypes = value; }  // set method
        }

        public List<Effect> Effects   // property
        {
            get { return effects; }   // get method
            set { effects = value; }  // set method
        }

        public List<Effect> OnDrawEffects   // property
        {
            get { return onDrawEffects; }   // get method
            set { onDrawEffects = value; }  // set method
        }

        public List<Effect> OnDiscardEffects   // property
        {
            get { return onDiscardEffects; }   // get method
            set { onDiscardEffects = value; }  // set method
        }
        #endregion

        #region MonoBehaviour Callbacks
        public Card()
        {
            id = System.Guid.NewGuid().ToString();
            //Debug.Log("new card id:  " + id);
            location = CardLocation.Deck;
        }
        #endregion

        #region Public Methods
        public void Play()
        {
            foreach (Effect effect in Effects)
            {
                effect.ActivateEffect();
            }
        }

        public void Play(Character target)
        {
            foreach (Effect effect in Effects)
            {
                effect.ActivateEffect(target);
            }
        }

        public void Play(HexCell target)
        {
            foreach (Effect effect in Effects)
            {
                effect.ActivateEffect(target);
            }
        }

        public void Play(Character player, HexCell target)
        {
            foreach (Effect effect in Effects)
            {
                effect.ActivateEffect(player, target);
            }
        }
        #endregion
    }
}
