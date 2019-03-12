using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonKIT
{
    public class DoorKey : Item
    {
        [Header("Settings")]
        public int keyID;

        public void OnPickedUp()
        {
            PlayerStats.Instance.doorKeys.Add(keyID, true);
        }

        public override void OnTriggerEnter2D(Collider2D collision)
        {
            onPickedUp += OnPickedUp;
            base.OnTriggerEnter2D(collision);
        }
    }
}