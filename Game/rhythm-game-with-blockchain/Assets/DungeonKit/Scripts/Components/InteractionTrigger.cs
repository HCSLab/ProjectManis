using UnityEngine;

namespace DungeonKIT
{
    public class InteractionTrigger : MonoBehaviour
    {
        InteractionCanvas canvas;

        [HideInInspector] public bool inTrigger; //Tracking trigger status

        private void Start()
        {
            canvas = GetComponentInChildren<InteractionCanvas>(true);
        }

        private void OnTriggerEnter2D(Collider2D collision) //if player ENTER in trigger
        {
            if (collision.gameObject.tag == "Player") //if its player
            {
                inTrigger = true;
                canvas.gameObject.SetActive(true); //UI enable
            }
        }

        private void OnTriggerExit2D(Collider2D collision)//if player Exit in trigger
        {
            if (collision.gameObject.tag == "Player")
            {
                inTrigger = false;
                canvas.gameObject.SetActive(false); //UI disable
            }
        }
    }
}
