using FTS.Characters;
using UnityEngine;

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Effect/Deck/Foretell", fileName = "ForetellEffect.asset")]
    public class ForetellEffect : Effect
    {
        [Range(2, 5)]
        [SerializeField] int foretellAmount;
        ForetellController foretell;

        private void Awake()
        {
            foretell = FindObjectOfType<ForetellController>().GetComponent<ForetellController>();
        }

        public override void ActivateEffect(Unit target)
        {
            foretell.Foretell(foretellAmount);
        }

        public override string GetEffectText()
        {
            return "<link=foretell><color=\"red\">Foretell</color></link> " + foretellAmount;
        }
    }
}
