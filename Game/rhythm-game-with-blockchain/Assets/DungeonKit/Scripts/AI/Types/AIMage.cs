using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonKIT
{
    public class AIMage : AICombat
    {
        AIController aiController;

        GameObject player;
        [Header("Prefabs")]
        public GameObject rangeWeapon; //Prefab Throwing Weapons

        [Header("Parametrs")]
        float timeBtwShots; //time between shots
        public float startTimeBtnShots; // Start time between shots

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            aiController = GetComponent<AIController>();
        }

        private void Update()
        {
            CheckAttackRadius();
        }

        void CheckAttackRadius()
        {
            if (Vector2.Distance(transform.position, player.transform.position) < aiController.radiusAttack) //If a player is in radiusAttack
            {
                AttackByRate(); //Attack
            }
        }
        //Method of attack
        public override void RangeAttack(GameObject rangeWeapon, Transform target)
        {
            //Set up here

            //
            base.RangeAttack(rangeWeapon, target);
        }

        //AttackByRate method
        void AttackByRate()
        {
            if (timeBtwShots <= 0)
            {
                RangeAttack(rangeWeapon, player.transform); //Spawn weapon
                timeBtwShots = startTimeBtnShots;//Set time to start again
            }
            else
            {
                timeBtwShots -= Time.deltaTime;//Time minus 1 sec
            }

        }
    }

}
