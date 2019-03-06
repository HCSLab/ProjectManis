using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Attack {
	private void Update()
	{
		transform.position = transform.position + new Vector3(0f, velocity, 0f);
		transform.localScale = transform.localScale - new Vector3(deltaScalePerFrame, deltaScalePerFrame, 0f);
	}
	private void OnBecameInvisible()
	{
		Destroy(gameObject);
	}
}