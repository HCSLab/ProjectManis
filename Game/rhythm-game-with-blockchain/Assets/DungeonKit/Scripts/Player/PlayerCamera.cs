using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonKIT
{
    public class PlayerCamera : MonoBehaviour
    {
        //Player position
        Transform playerTransform;

        [Header("Variables")]
        public float smoothFollow = 1; //Smooth parameter of camera
        public Vector3 offset; //Camera position offset

        private void Start()
        {
            //Find player
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void FixedUpdate()
        {
            CameraFollow();
        }
        //Camera follow method
        void CameraFollow()
        {
            Vector3 desiredPosition = playerTransform.position + offset; //Make offset
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothFollow); //Smooth move
            transform.position = smoothedPosition; //Set position
        }
    }

}
