//using UnityEngine;
//using FTS.Characters;

//namespace FTS.Cards
//{
    
//    public class Attack : Card//, ITargetable
//    {
//        [Header("Attack specific")]
//        [SerializeField] int attackDamage;
//        [SerializeField] int numOfAttacks;

//        public Attack()
//        {
//            Type = CardType.attack;
//            IsTargeting = true;
//        }
//        public override void Play()
//        {
//            foreach (Effect effect in Effects)
//            {
//                //Debug.Log("card draw: " + effect.cardsDrawn);
//                //Debug.Log("damage: " + effect.attack);
//                //Debug.Log("number of attacks: " + effect.numAttacks);
//                effect.ActivateEffect();
//            }
//            Debug.Log("Attack card played");
//        }

//        public override void Play(Character target)
//        {

//            Debug.Log("Attack card played");
//        }

//        public CardType GetCardType()
//        {
//            return Type;
//        }

//    }
//}