using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonKIT
{
    public class AIStats : MonoBehaviour
    {
        //Cached components
        AIController aiController;
        AICanvas aICanvas;
        SpriteRenderer aiSprite;
        AudioSource audioSource;

        //Event
        public delegate void DeathAction(); // AI Death Event
        public event DeathAction onDeath;

        [HideInInspector] public DamageEffect damageEffect; //Visual damage effect

        [Header("Settings")]
        public DoubleFloat HP = new DoubleFloat(100, 100); //DoubleFloat(currentHP,maxHP)

        private void Start()
        {
            aiSprite = GetComponentInChildren<SpriteRenderer>();
            aICanvas = GetComponentInChildren<AICanvas>();
            aiController = GetComponent<AIController>();
            audioSource = GetComponent<AudioSource>();
        }

        //Сaused by taking damage
        public void TakingDamage(float damage)
        {
            aiController.isAttacked = true; //sends AI that he was attacked

            HP.current -= damage; //damage
            aICanvas.UpdateUI(); //Update AI ui (hp bar)

            AudioManager.Instance.Play(audioSource, AudioManager.Instance.aiDamage, false); //play damage sound

            StartCoroutine(damageEffect.Damage(aiSprite)); //Start damage effect

            if (HP.current <= 0) //if HP < 0 Death
            {
                Death();
            }
        }

        void Death()
        {
            if (onDeath != null)
                onDeath(); // Death event

            Destroy(gameObject);
        }


    }
}
