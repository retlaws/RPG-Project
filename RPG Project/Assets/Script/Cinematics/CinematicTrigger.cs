using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool hasBeenTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            
            if (other.gameObject.tag == "Player" && hasBeenTriggered == false)
            {
                GetComponent<PlayableDirector>().Play();
                hasBeenTriggered = true;
            }

        }
    }
}


