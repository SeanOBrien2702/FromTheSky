using UnityEngine;

namespace FTS.Cards
{
    public class EnhancementEffect : Effect
    {
        [Header("Enhancement")]
        [SerializeField] int armour = 5;
        public override void ActivateEffect()
        {
            Debug.Log("Enhancement effect");
        }
    }
}
