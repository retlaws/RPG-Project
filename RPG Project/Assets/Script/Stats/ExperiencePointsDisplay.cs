using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


namespace RPG.Stats 
{
    public class ExperiencePointsDisplay : MonoBehaviour
    {
        Experience experience;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }
       
        void Update()
        {
            GetComponent<Text>().text = experience.GetExperience().ToString();
        }
    }


}


