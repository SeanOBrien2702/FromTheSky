using UnityEngine;

public enum RelicRarity
{
    common, uncommon, rare
}
//available
[System.Serializable]
[CreateAssetMenu(menuName = "Relic", fileName = "Relic.asset")]
public class Relic : ScriptableObject
{
    public Sprite image;
    public string name;
    public string description;
    public RelicRarity rarity;

    public string GetName()
    {
        return name;
    }
}
