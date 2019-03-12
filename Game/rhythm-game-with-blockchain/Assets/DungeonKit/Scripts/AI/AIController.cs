using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonKIT
{
    public class AIController : MonoBehaviour
    {
        [HideInInspector] public Transform playerPos; //cached player position
        Animator animator;

        [Header("Sprite AI")]
        public Transform aiSprite;

        Vector3 startPosition; //coordinate position to return when isReturnToStartPos

        [Header("Radius")]
        public float radiusFollow; //the radius at which AI begins to follow the player
        public float radiusAttack; //the radius at which AI begins to attack the player
        public float radiusStop; //radius at which AI stops
        [Header("Move speed")]
        public float moveSpeed; // AI move speed

        [Header("Settings")]
        public bool isReturnToStartPos; //if true, AI will return to the starting position when the player goes beyond radiusFollow
        public bool infinitiFollow; //if true, AI will follow the player endlessly.
        bool isMove;
        [HideInInspector] public bool isAttacked;

        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
            playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); //Find player

            startPosition = transform.position; //cached start position
        }

        private void Update()
        {
            if (!UIManager.Instance.isPause && GameManager.Instance.isGame) // check for pause and game state
            {
                Animation(); //Animation logic
                Move(); // Move logic
            }
        }

        void Animation()
        {
            animator.SetBool("Move", isMove); // Set bool in animator
        }
        void Move()
        {
            if (Vector2.Distance(transform.position, playerPos.position) < radiusStop) // If the player has entered the stop radius
            {
                isMove = false; //Stop
                Rotation(true); // Rotation(true - look at player, false loock at target)
            }
            else if (Vector2.Distance(transform.position, playerPos.position) < radiusFollow) //If the player has entered the pursuit radius
            {
                isAttacked = false;
                isMove = true;
                transform.position = Vector2.MoveTowards(transform.position, playerPos.position, moveSpeed * Time.deltaTime); // Begin to follow
                Rotation(true);
            }
            else if (isAttacked) // If we were attacked
            {
                transform.position = Vector2.MoveTowards(transform.position, playerPos.position, moveSpeed * Time.deltaTime);// Begin to follow
                Rotation(true);
            }
            else if (isReturnToStartPos && Vector2.Distance(transform.position, playerPos.position) > radiusFollow && Vector2.Distance(transform.position, startPosition) > 0) //If isReturnToStartPos is enabled, we return to the starting position
            {
                isMove = true;
                transform.position = Vector2.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime);
                Rotation(false);
            }
            else if (infinitiFollow && isMove)//infiniti follow
            {
                transform.position = Vector2.MoveTowards(transform.position, playerPos.position, moveSpeed * Time.deltaTime);
                Rotation(true);
            }
            else //Just stop
            {
                isMove = false;
            }

        }
        // Rotation(true - look at player, false loock at target)
        void Rotation(bool player)
        {
            if (player)
            {
                //look right
                if (playerPos.position.x - transform.position.x > 0)
                {
                    aiSprite.localScale = new Vector3(1, 1, 1);
                }
                //lock left
                else if (playerPos.position.x - transform.position.x < 0)
                {
                    aiSprite.localScale = new Vector3(-1, 1, 1);
                }
            }
            else
            {
                if (startPosition.x - transform.position.x > 0)
                {
                    aiSprite.localScale = new Vector3(1, 1, 1);
                }
                else if (startPosition.x - transform.position.x < 0)
                {
                    aiSprite.localScale = new Vector3(-1, 1, 1);
                }
            }

        }

        //Method for drawing radius
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, radiusFollow);
            Gizmos.DrawWireSphere(transform.position, radiusAttack);
            Gizmos.DrawWireSphere(transform.position, radiusStop);
        }

    }
}