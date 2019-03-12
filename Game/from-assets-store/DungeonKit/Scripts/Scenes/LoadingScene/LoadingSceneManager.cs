using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungeonKIT
{
    public class LoadingSceneManager : MonoBehaviour
    {
        private void Start()
        {
            LoadScene(); //Call load scene method whe LoadingScene is loaded
        }

        //Loading scene method
        void LoadScene()
        {
            //Call static SceneManager to load scene
            SceneManager.LoadScene(ScenesManager.Instance.sceneToLoad); //Get scene name from SceneManager.sceneToLoad
        }
    }
}