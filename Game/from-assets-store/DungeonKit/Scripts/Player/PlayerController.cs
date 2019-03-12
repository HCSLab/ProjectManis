using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonKIT
{
    public class PlayerController : MonoBehaviour
    {
        //Components
        PlayerStats playerStats;
        Animator playerAnimator;
        Rigidbody2D rigidbody2d;

        [Header("Components")]
        public GameObject playerSprite;
        public SpriteRenderer playerSpriteRenderer;

        [Header("Parameters")]
        public float moveSpeed;


        private void Start()
        {
            playerStats = PlayerStats.Instance;
            rigidbody2d = GetComponent<Rigidbody2D>();
            playerAnimator = GetComponentInChildren<Animator>();

            PlayerStats.Instance.audioSource = GetComponent<AudioSource>();
            PlayerStats.Instance.playerSprite = playerSpriteRenderer; //Set in static PlayerStats this game object player sprite for damage effect

            SaveManager.Save(); //Save level state
        }

        private void Update()
        {
            Inputs(); //Check inputs
        }

        void FixedUpdate()
        {
            if (!UIManager.Instance.isPause && GameManager.Instance.isGame) //If pause disable, and is game
            {
                CheckActions();
            }
        }
        //Actions method
        void CheckActions()
        {
            Rotation();  //Rotation of player 
            Move(); //Player move
            Animation(); //Player animation
        }

        //Move method
        void Move()
        {
            //Movement of the character depending on the values InputManager.Horizontal, InputManager.Vertical
            transform.Translate(new Vector3(InputManager.Horizontal, InputManager.Vertical, 0) * moveSpeed * Time.deltaTime);
        }

        //Rotation method
        void Rotation()
        {
            //Check active platform
#if UNITY_STANDALONE // PC,WIN,MAC

            //If mouse in right side of screen
            if (InputManager.MouseXPositon > 0.5f)
            {
                playerSprite.transform.localScale = new Vector3(1, 1, 1); // character look right
            }
            //If mouse in left side of screen
            else if (InputManager.MouseXPositon < 0.5f)
            {
                playerSprite.transform.localScale = new Vector3(-1, 1, 1); // character look left
            }

#elif UNITY_ANDROID || UNITY_IOS //mobile
            //If stick in right side
            if (InputManager.Horizontal > 0)
            {
                playerSprite.transform.localScale = new Vector3(1, 1, 1); // character look right
            }
            //If stick in left side
            else if (InputManager.Horizontal < 0)
            {
                playerSprite.transform.localScale = new Vector3(-1, 1, 1); // character look left
            }
#endif
        }

        //Animation method
        void Animation()
        {
            if (InputManager.Horizontal != 0 || InputManager.Vertical != 0) //if character is move
            {
                playerAnimator.SetBool("Move", true); //Animator set move 
            }
            else
            {
                playerAnimator.SetBool("Move", false); //Animator reset move
            }
        }
        //Inputs method
        void Inputs()
        {
            if (InputManager.Pause) //If player press pause button
            {
                InputManager.Pause = false; //Unpress
                UIManager.Instance.Pause(); //Show UI pause screen
            }

            //if pause disable and game
            if (!UIManager.Instance.isPause && GameManager.Instance.isGame)
            {
                if (InputManager.Health) //If player press helath button
                {
                    InputManager.Health = false; //Unpress
                    playerStats.Health(); //Player health hp
                }
            }
        }
    }
}
