//using UnityEngine;
//using SP.Characters;

//namespace SP.Cards
//{
//    [System.Serializable]
//    [CreateAssetMenu(menuName = "Card/Enhancement", fileName = "Enhancement.asset")]
//    public class Enhancement : Card//, ITargetable
//    {
//        [Header("Enhancement specific")]
//        [SerializeField] int boostAmount;

//        public Enhancement()
//        {
//            Type = CardType.enhancement;
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