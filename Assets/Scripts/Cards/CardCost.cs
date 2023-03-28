#region Using Statements
using FTS.Characters;
using FTS.Grid;
using System.Collections.Generic;
using UnityEngine;
#endregion

namespace FTS.Cards
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "CardCost", fileName = "CardCost.asset")]
    public class CardCost : ScriptableObject
    {
        [SerializeField] int[] costs;
        [SerializeField] int variation;
        [SerializeField] int scaler = 10;

        #region Public Methods
        public int GetCost(CardRarity rarity)
        {
            int cost = costs[(int)rarity];
            cost += Random.Range(-variation, variation) * scaler;
            return cost;
        }
        #endregion
    }
}
