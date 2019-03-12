using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonKIT
{
    public class AIOrc : AICombat
    {
        //If player entered in trigger
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player") //if its player
            {
                MeleeAttack(collision.gameObject); //Attack
            }
        }
        //Method of attack
        public override void MeleeAttack(GameObject target) 
        {
            //Set up here

            //
            base.MeleeAttack(target);
        }
    }
}