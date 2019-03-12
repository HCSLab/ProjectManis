using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonKIT
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Item : MonoBehaviour
    {
        public delegate void PickedUpAction(); //Delegate for Pick up item
        public event PickedUpAction onPickedUp; //Pick up event

        public virtual void OnTriggerEnter2D(Collider2D collision) //If player entered in trigger
        {
            if (collision.gameObject.tag == "Player") //if its player
            {
                onPickedUp(); //Event
                Destroy(gameObject); // Destroy this GameObject
            }
        }

    }
}