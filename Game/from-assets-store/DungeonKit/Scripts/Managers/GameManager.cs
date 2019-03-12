using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonKIT
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance; //Singleton

        NextLevelDoor nextLevelDoor; //Next level door object

        [Header("Settings")]
        public bool isGame = true;
        public bool levelComplete;

        private void Awake()
        {
            //Set sigleton
            Instance = this;
        }

        private void Start()
        {
            //Find next level door
            nextLevelDoor = GameObject.Find("NextLevelDoor").GetComponent<NextLevelDoor>();
        }

        //GameOver method
        public void GameOver()
        {
            isGame = false; //Game status is false, and all actions on scene is stop
            UIManager.Instance.GameOver(); //Show GameOver screen
        }

        //Complete level method
        public void LevelComplete()
        {
            levelComplete = true; //Set bool for Check door status
            nextLevelDoor.lockedDoor = false; //Unlock door
            nextLevelDoor.CheckLockStatus(); //Check door status
        }
    }
}