using FTS.Cards;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "CardRaritySettings", fileName = "CardRaritySettings.asset")]
public class CardRaritySettings : ScriptableObject
{
    [SerializeField] int commonChance = 90;
    [SerializeField] int uncommonChance = 10;
    [SerializeField] int rareChance = 10;
    [SerializeField] int legendaryChance = 0;

    public int GetCommonChance()
    {
        return commonChance;
    }

    public int GetUncommonChance()
    {
        return commonChance + uncommonChance;
    }

    public int GetRareChance()
    {
        return commonChance + uncommonChance + rareChance;
    }

    public int GetChanceTotal()
    {
        return commonChance + uncommonChance + rareChance + legendaryChance;
    }
}
