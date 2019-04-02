using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBorderDetector : Tile {
	private void OnDestroy()
	{
		if (Player.instance != null && !GameManager.instance.gameOver)
			GameManager.instance.gameObject.GetComponent<BackgroundManager>().GenerateAScreenAbove();
	}
}
