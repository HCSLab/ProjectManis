using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonKIT
{
    public class NextLevelDoor : MonoBehaviour
    {
        CircleCollider2D collider2D;

        [Header("Components")]
        SpriteRenderer spriteRenderer;
        public Sprite lockedSprite, openedSprite;
        public bool lockedDoor; //Door status

        bool inTrigger;
        InteractionTrigger interactionTrigger;

        private void Start()
        {
            interactionTrigger = GetComponent<InteractionTrigger>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            collider2D = GetComponent<CircleCollider2D>();

            CheckLockStatus(); //Check door status
        }

        private void Update()
        {
            //if player in trigger
            if (interactionTrigger.inTrigger)
            {
                //if player press Interaction button
                if (InputManager.Interaction)
                {
                    InputManager.Interaction = false; //unpress button
                    if (!lockedDoor) //if door unlocked
                    {
                        GoToNextLevel(); //go to next level
                    }
                }
            }
        }

        //Check door status method
        public void CheckLockStatus()
        {
            if (lockedDoor) //if door locked
            {
                spriteRenderer.sprite = lockedSprite; //sprite locked door
                collider2D.enabled = false; //trigger disabled
            }
            else
            {
                spriteRenderer.sprite = openedSprite; //sprite unloced door
                collider2D.enabled = true; //trigger enabled
            }
        }
        //Next level method
        void GoToNextLevel()
        {
            int lvlID = ScenesManager.Instance.levelID + 1; //Level id + 1
            ScenesManager.Instance.levelID++;
            ScenesManager.Instance.LoadLoadingScene("Lvl_" + lvlID); //Load next level
        }


    }
}
