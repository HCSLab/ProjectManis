using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonKIT
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance; //Singleton

        //Ceched components
        PlayerStats playerStats;
        DialogManager dialogManager;

        [Header("Components")]
        List<GameObject> HPUIObjects = new List<GameObject>(); //UI HP list
        public Transform hpParent; //HP parent, position for spawn
        public GameObject hpIconPrefab; //HP prefab for spawn
        public Sprite hpActiveSprite, hpDisableSprite; //Sprites for HP( 1 hpActive - you have 1 hp, 1 hpDisabled - you have taken damage  )

        public Text moneyText, bottleText; //UI text

        [Header("Screens GameObjects")]
        public GameObject dialogGO, shopGO;
        public GameObject pauseGo;
        public GameObject gameoverGO;
        public GameObject mobileUIGO;
        [Header("Status")]
        public bool isPause;

        public event EventHandler dialogClosed; //Close dialog event

        //Singleton method
        void SingletonInit()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Awake()
        {
            SingletonInit();
        }

        private void Start()
        {
            //Check active platform
#if UNITY_ANDROID || UNITY_IOS //mobile 

            mobileUIGO.SetActive(true); //Enable mobile UI
#endif
            dialogManager = GetComponent<DialogManager>();

            dialogClosed += CloseShopMenu; //Add event

            playerStats = PlayerStats.Instance; //Set playerstats in static object of PlayerStats
            UpdateUI(); //UpdateUI

        }

        //Update ui method
        public void UpdateUI()
        {
            UpdateHP(); //Update HP 
            moneyText.text = playerStats.money.ToString(); //Update ui money text
            bottleText.text = playerStats.bottles.ToString(); //Update ui bottle text
        }

        //Update hp method
        public void UpdateHP()
        {
            //Loop for clear old hp
            for (int i = 0; i < HPUIObjects.Count; i++)
            {
                Destroy(HPUIObjects[i]);
            }
            HPUIObjects.Clear(); //Clear list

            //Loop for spawn new 
            for (int i = 0; i < playerStats.HP.max; i++)
            {
                Image hpIcon = Instantiate(hpIconPrefab, hpParent).GetComponent<Image>(); //Spawn prefab

                if (playerStats.HP.current > i) //check player hp
                {
                    hpIcon.sprite = hpActiveSprite; //Set Active hp
                }
                else
                {
                    hpIcon.sprite = hpDisableSprite; //Set disable hp
                }
                HPUIObjects.Add(hpIcon.gameObject); //Add object to list 
            }
        }

        //Show dialog menu method
        public void ShowDialogMenu(DialogConfig dialogConfig)
        {
            isPause = true; //set pause

            dialogGO.SetActive(true); //Show dialog screen gameobject
            dialogManager.SetDialogConfig(dialogConfig); //set config to dialog
        }
        //Show shop menu method
        public void ShowShopMenu()
        {
            shopGO.SetActive(true); //Show shop screen
        }
        //Close dialog method
        public void CloseDialogMenu()
        {
            isPause = false; //disable pause

            dialogClosed(this, new EventArgs()); //Activate event

            dialogGO.SetActive(false); //Disable dialog screen
        }
        //Close shop menu method
        public void CloseShopMenu(object sender, EventArgs e)
        {
            shopGO.SetActive(false); //Disable shop screen
        }
        //Pause method
        public void Pause()
        {
            isPause = !isPause; //Reverse pause status
            pauseGo.SetActive(!pauseGo.activeSelf); //Reverse pause screen active status 
        }
        //UI GameOver method
        public void GameOver()
        {
            gameoverGO.SetActive(true); //gameover screen enable
        }

        //Load main menu method
        public void LoadMainMenu()
        {
            ScenesManager.Instance.LoadLoadingScene("MainMenu"); //Load main menu scene
        }


        //Check active platform
#if UNITY_ANDROID || UNITY_IOS //mobile 
        public void HealthBtn()
        {
            playerStats.Health(); //Player health hp
        }
        public void InteractiveBtn()
        {
            InputManager.Interaction = true;
        }
#endif
    }
}
