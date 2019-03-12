using System;
using UnityEngine;

namespace DungeonKIT
{
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject rangeWeaponPrefab;

        [Header("Parameters")]
        float timeBtwShots;
        public float startTimeBtnShots;

        private void Update() //Every frame
        {
            if (!UIManager.Instance.isPause && GameManager.Instance.isGame) //If pause disable, and is game
            {
                Attack();
            }
        }

        private void FixedUpdate() //Every second considering the physics
        {
            if (!UIManager.Instance.isPause) //if pause disable
            {

                //Check active platform
#if UNITY_STANDALONE // PC,WIN,MAC

                if (InputManager.Attack) //if player press attack button
                {
                    AttackByRate();
                }

#elif UNITY_ANDROID || UNITY_IOS //mobile

                if (VirtualJoystick.isAttack) //if player drag attack stick
                {
                    AttackByRate();
                }
#endif
            }
        }

        //Attack method
        void Attack()
        {
            //Check active platform
#if UNITY_STANDALONE // PC,WIN,MAC

            if (InputManager.Attack)
            {
                return;
            }
            else
            {
                timeBtwShots = 0;
            }

#elif UNITY_ANDROID || UNITY_IOS //mobile

            if (VirtualJoystick.isAttack)
            {
                return;
            }
            else
            {
                timeBtwShots = 0;
            }
#endif
        }

        //Range attack method
        void RangeAttack()
        {
            GameObject rangeWeapon = Instantiate(rangeWeaponPrefab); //spawn weapon
            rangeWeapon.transform.position = transform.position; //Set weapon postion

            //Check active platform
#if UNITY_STANDALONE // PC,WIN,MAC

            Vector3 mousePosition = Input.mousePosition; //Cache mouse position
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition); //calculate the mouse position relative to the screen and the world

            Vector2 dir = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y); //calculate angle

            rangeWeapon.transform.up = dir; //Set angle rotation

#elif UNITY_ANDROID || UNITY_IOS //mobile

            float z = 0;

            float forX = VirtualJoystick.joystickAttackDir.x;
            float forY = VirtualJoystick.joystickAttackDir.z;

            //We perform calculations to determine the angle of flight of our weapon.
            if (forY > 0)
            {
                z = (float)(Math.Acos(forX / Math.Sqrt(Math.Pow(forX, 2) + Math.Pow(forY, 2))));
                z = z * (float)(180 / Math.PI) - 90;
            }
            else
            {
                z = (float)(Math.Acos(forX / Math.Sqrt(Math.Pow(forX, 2) + Math.Pow(forY, 2))));
                z = 360 - (z * (float)(180 / Math.PI)) - 90;
            }
            //Rotating
            rangeWeapon.transform.rotation = Quaternion.Euler(0, 0, z);
#endif
        }

        //Attack by rate method
        void AttackByRate()
        {
            if (timeBtwShots <= 0)
            {
                RangeAttack(); //Attack
                timeBtwShots = startTimeBtnShots; //set timer again
            }
            else
            {
                timeBtwShots -= Time.deltaTime; //timer - 1 sec
            }

        }

    }
}
