using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartAndEnd : MonoBehaviour {

	public void GameStart()
	{
		SceneManager.LoadScene("MainGame");
	}

}
