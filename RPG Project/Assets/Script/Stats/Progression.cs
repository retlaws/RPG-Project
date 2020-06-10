using RPG.Core;
using RPG.Stats;
using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Linq;

namespace RPG.Stats
{

    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();

            float[] levels = lookupTable[characterClass][stat];

            if(levels.Length < level)
            {
                return 0;
            }

            return levels[level - 1];
        }

        public int GetLevel(Stat stat, CharacterClass characterClass, float currentXP)
        {
            BuildLookup();

            float[] levels = lookupTable[characterClass][stat];

            for (int i = 0; i < levels.Length; i++)
            {
                if (currentXP < levels[0]) return 1;
                if (currentXP > levels[i]) continue;
                if(currentXP <= levels[i])
                {
                    return i+1;
                }
            }
            return 1;
        }

        private void BuildLookup()
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();

                foreach(ProgressionStat progressionStat in progressionClass.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }
                lookupTable[progressionClass.characterClass] = statLookupTable;
            }
        }

        [System.Serializable]
        public class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;

        }

        [System.Serializable]
        public class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }


    }
}


