using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonKIT
{
    public class DialogManager : MonoBehaviour
    {
        //Components
        public Text dialogNameText, dialogText;

        public void SetDialogConfig(DialogConfig dialogConfig) //Set up config
        {
            dialogNameText.text = dialogConfig.name; //Dialog name from dialog config
            SetDialogState(dialogConfig, 0); //Set dialog state
        }

        public void SetDialogState(DialogConfig dialogConfig, int state) //Method for change dialogState
        {
            dialogText.text = dialogConfig.dialogs[state].dialogText; //UI change dialog text
        }


    }
}
