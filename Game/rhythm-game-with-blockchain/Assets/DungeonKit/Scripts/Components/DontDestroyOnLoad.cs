using UnityEngine;

namespace DungeonKIT
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        //This script is needed to ensure that the object on which it lies is not deleted when going through scenes.
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
