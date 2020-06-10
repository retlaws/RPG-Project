using RPG.Resources;
using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int level = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpEffect = null;
        [SerializeField] bool shouldUseModifiers = false; 
        Experience experience;

        public event Action onLevelUp;

        int currentLevel = 1;
        GameObject player;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            experience = GetComponent<Experience>();
        }

        private void OnEnable()
        {
            if(experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }    
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void Start()
        {
            currentLevel = CalculateLevel();
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel(); 
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                Instantiate(levelUpEffect, transform);
                onLevelUp();
            }
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat)/100);
        }

        public int GetLevel()
        {
            return currentLevel;
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if(!shouldUseModifiers) { return 0; }

            IModifierProvider[] listOfProviders = GetComponents<IModifierProvider>();

            float totalModification = 0f;

            foreach (IModifierProvider provider in listOfProviders)
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    totalModification += modifier;
                }
            }
            return totalModification;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) { return 0; }

            IModifierProvider[] listOfProviders = GetComponents<IModifierProvider>();

            float totalPercentageModification = 0f;

            foreach (IModifierProvider provider in listOfProviders)
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    totalPercentageModification += modifier;
                }
            }
            return totalPercentageModification;
        }

        private int CalculateLevel()
        {
            if(gameObject == player)
            {
                float currentXP = GetComponent<Experience>().GetExperience();
                level = progression.GetLevel(Stat.ExperienceToLevelUp, characterClass, currentXP);
                return level;
            }
            else
            {
                return level;
            }
        }
        
    }
}


