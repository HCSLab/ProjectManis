using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	public void MoveDownward(int distance)
	{
		transform.position -= new Vector3(0f, distance, 0f);
	}

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
