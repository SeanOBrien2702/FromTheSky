using System.Collections.Generic;
using UnityEngine;

namespace FTS.Characters
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "CharacterStats", menuName = "Stats/characterStats")]
    public class CharacterStats : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;
        Dictionary<CharacterClass, Dictionary<Stat, int>> lookupTable = null;

        public int GetStat(Stat stat, CharacterClass characterClass)
        {
            BuildLookup();
            int statValue = lookupTable[characterClass][stat];
            return statValue;
        }

        private void BuildLookup()
        {
            if (lookupTable == null)
            {
                lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, int>>();
                foreach (ProgressionCharacterClass progressionClass in characterClasses)
                {
                    var statLookupTable = new Dictionary<Stat, int>();

                    foreach (ProgressionStat progressionStat in progressionClass.stats)
                    {
                        statLookupTable[progressionStat.stat] = progressionStat.value;
                    }
                    lookupTable[progressionClass.characterClass] = statLookupTable;
                }
            }
        }
    }
    [System.Serializable]
    class ProgressionCharacterClass
    {
        public CharacterClass characterClass;
        public ProgressionStat[] stats;
    }
    [System.Serializable]
    class ProgressionStat
    {
        public Stat stat;
        public int value;
    }
}