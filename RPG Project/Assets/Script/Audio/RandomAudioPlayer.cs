using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class RandomAudioPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;
    //[SerializeField] float minPitch = -1f;
    //[SerializeField] float maxPitch = 2f;
    AudioSource audioSource;
    

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private AudioClip GetRandomAudioClip()
    {
        int clipsLength = clips.Length - 1;

        return clips[UnityEngine.Random.Range(0, clipsLength)];
    }

    public void PlayRandomAudioClip()
    {
        if(!audioSource.isPlaying)
        {
            audioSource.clip = GetRandomAudioClip();
            //SetRandomPitch();
            audioSource.Play();
        }
    }

    //private void SetRandomPitch()
    //{
    //    audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
    //}
}
