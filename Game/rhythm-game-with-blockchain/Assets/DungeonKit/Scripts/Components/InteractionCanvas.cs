using UnityEngine;
using UnityEngine.UI;

namespace DungeonKIT
{
    public class InteractionCanvas : MonoBehaviour
    {
        //Button
        public string interaction { get { return InputSettings.InteractionKey.ToString(); } } 

        [Header("Componetns")]
        public Text InteractionText;

        private void Update()
        {
            InteractionText.text = interaction; //Assign the text in the UI to our button from InputSettings
        }



    }
}