using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonKIT
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance; //Singleton

        AudioSource backgroundMusic;
        bool isMusicPlay; //for check music status

        [Header("AudioClips")] //List of sounds
        public AudioClip aiDamage;
        public AudioClip playerDamage;

        public AudioClip music;

        //Singleton
        private void Awake()
        {
            if (AudioManager.Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            backgroundMusic = GetComponent<AudioSource>();

            if (!isMusicPlay) //if the music is not playing
            {
                isMusicPlay = true;
                Play(backgroundMusic, music, true); //Play music
            }
        }

        public void Play(AudioSource audioSource, AudioClip audioClip, bool loop)
        {
            audioSource.clip = audioClip;
            audioSource.loop = loop;
            audioSource.Play();
        }
    }
}
