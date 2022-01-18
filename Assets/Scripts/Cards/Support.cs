using UnityEngine;
using FTS.Characters;

public enum SupportType
{
    cardDraw,
}

//namespace FTS.Cards
//{
//    [System.Serializable]
//    [CreateAssetMenu(menuName = "Card/Support", fileName = "Support.asset")]
//    public class Support : Card
//    {

//        public Support()
//        {
//            Type = CardType.support;
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
//            Debug.Log("Enhancement card played");
//        }

//        public override void Play(Character target)
//        {
//            foreach (Effect effect in Effects)
//            {
//                //Debug.Log("card draw: " + effect.cardsDrawn);
//                //Debug.Log("damage: " + effect.attack);
//                //Debug.Log("number of attacks: " + effect.numAttacks);
//                effect.ActivateEffect();
//            }
//            Debug.Log("Enhancement card played");
//        }


//        public CardType GetCardType()
//        {
//            return Type;
//        }

//    }
//}