using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace DungeonKIT
{
    public class ShopSystem : MonoBehaviour
    {
        [Header("Items")]
        public ShopItem[] shopItems;

        PlayerStats playerStats;

        private void Start()
        {
            playerStats = PlayerStats.Instance;
        }

        //Buy method
        public void Buy(int itemID)
        {
            if (playerStats.money >= shopItems[itemID].price) //If player money > item price
            {
                playerStats.money -= shopItems[itemID].price; //Player money - price

                switch (shopItems[itemID].itemType) //Check item type
                {
                    case ShopItem.ItemType.bottle: //Bottles type 
                        playerStats.bottles++; //Bottles + 1
                        break;
                }
            }

            UIManager.Instance.UpdateUI(); //Update UI
        }

    }

    [Serializable]
    public class ShopItem
    {
        public enum ItemType { bottle } //here you can add type new items
        public ItemType itemType;
        public int price;
    }
}