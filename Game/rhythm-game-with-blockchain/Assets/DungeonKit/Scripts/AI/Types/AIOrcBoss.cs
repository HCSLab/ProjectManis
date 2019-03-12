using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonKIT
{
    public class AIOrcBoss : AICombat
    {
        //Cached components
        AIController aiController;
        Animator animator;

        [Header("Parametrs")]
        float timeBtwShots; //time between shots
        public float startTimeBtnShots; // Start time between shots
        public float jumpSpeed; // AI Jump Speed
        public float jumpPower; // Range and strength of the jump

        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
            aiController = GetComponent<AIController>();
            aiStats = GetComponent<AIStats>();

            aiStats.onDeath += Death; //Adds to the event variable in the parent class
        }


        private void OnCollisionEnter2D(Collision2D collision) //If the player entered the trigger
        {
            if (collision.gameObject.tag == "Player") //If its is player
            {
                MeleeAttack(collision.gameObject); //Melee attack
            }
        }

        public override void MeleeAttack(GameObject target) //set up attack
        {
            //Set up here

            //
            base.MeleeAttack(target); //Parent method starts
        }

        private void Update()
        {
            //If a player enters raduisAttack
            if (Vector2.Distance(aiController.playerPos.position, transform.position) < aiController.radiusAttack)
            {
                AttackByRate(); //Attack by rate
            }
        }

        //Attack method
        void Attack()
        {
            animator.Play("OrcBoss_attack"); //Play animation
            StartCoroutine(IAttack()); //Start IEnumerator for smooth jump
        }

        //AttackByRate method
        void AttackByRate()
        {
            if (timeBtwShots <= 0)
            {
                Attack(); //Jump attack
                timeBtwShots = startTimeBtnShots; //Set time to start again
            }
            else
            {
                timeBtwShots -= Time.deltaTime; //Time minus 1 sec
            }

        }

        //if boss die level complete
        void Death()
        {
            GameManager.Instance.LevelComplete();
        }

        //IEnumerator for smooth jump
        IEnumerator IAttack()
        {
            float time = 0; //Timer time

            while (time < jumpSpeed) 
            {
                transform.position = Vector2.MoveTowards(transform.position, aiController.playerPos.position, jumpPower * Time.deltaTime); //Make jump
                time++;
                yield return null;
            }

        }
    }
}
