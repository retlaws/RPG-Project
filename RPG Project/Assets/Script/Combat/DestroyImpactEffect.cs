using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Combat
{
    public class DestroyImpactEffect : MonoBehaviour
    {
        [SerializeField] GameObject targetToDestroy = null;
        ParticleSystem particle;

        void Awake()
        {
            particle = GetComponent<ParticleSystem>();
        }

        // Update is called once per frame
        void Update()
        {
            if (particle.isStopped)
            {
                if(targetToDestroy != null)
                {
                    Destroy(targetToDestroy);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}

