using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonKIT
{
    public class AIRangeWeapon : RangeWeapon
    {

        public override void OnTriggerEnter2D(Collider2D collider)
        {
            base.OnTriggerEnter2D(collider);

            if (collider.gameObject.tag == "Player") //if contact with player
            {
                Damage(PlayerStats.Instance); //Player damaged
            }
        }

        //Damage method
        void Damage(PlayerStats player)
        {
            player.TakingDamage(); //Player hp - 1 
            Destroying(); //Destroyng gameobject
        }

    }
}