using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DungeonKIT
{
    [CreateAssetMenu(fileName = "DialogConfig", menuName = "Dialog/DialogConfig")]
    public class DialogConfig : ScriptableObject
    {
        [Header("Setup")]
        public string name; //Name of NPC
        public Dialog[] dialogs; //List of dialogs
    }

    [Serializable]
    public class Dialog
    {
        [Multiline]
        public string dialogText; 
    }
}
