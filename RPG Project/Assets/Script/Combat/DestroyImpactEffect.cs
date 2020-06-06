using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Combat
{
    public class DestroyImpactEffect : MonoBehaviour
    {
        ParticleSystem particle;
        void Start()
        {
            particle = GetComponent<ParticleSystem>();
        }

        // Update is called once per frame
        void Update()
        {
            if (particle.isStopped)
            {
                Destroy(gameObject);
            }
        }
    }
}

