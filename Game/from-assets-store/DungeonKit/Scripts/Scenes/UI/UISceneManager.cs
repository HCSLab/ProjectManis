using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonKIT
{
    public class UISceneManager : MonoBehaviour
    {
        private void Awake()
        {
            //Put the GameUI scene in the background
            ScenesManager.Instance.LoadAdditiveScene("GameUI");
        }

    }
}
