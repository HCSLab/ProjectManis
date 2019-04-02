using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	private void Update()
	{
		if (Enemy.instance == null)
		{
			transform.position -= new Vector3(0,1f,0);
		}
	}

	private void OnBecameInvisible()
	{
		Destroy(gameObject);
	}
}
