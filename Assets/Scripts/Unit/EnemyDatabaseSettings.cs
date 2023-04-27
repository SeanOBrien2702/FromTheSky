using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Difficulty/EnemyDataBaseSettings", fileName = "EnemyDataBaseSettings.asset")]
public class EnemyDatabaseSettings : ScriptableObject
{
    [SerializeField] int commonChance = 90;
    [SerializeField] int rareChance = 10;
    //[SerializeField] int supportChance = 0;
    [SerializeField] int bossChance = 0;

    public int GetCommonChance()
    {
        return commonChance;
    }

    public int GetRareChance()
    {
        return commonChance + rareChance;
    }

    //public int GetSupportChance()
    //{
    //    return commonChance + rareChance + supportChance;
    //}

    public int GetChanceTotal()
    {
        return commonChance + rareChance + bossChance;
    }
}
