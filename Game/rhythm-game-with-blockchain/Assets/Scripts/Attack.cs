using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
	public float strength;
	protected float velocity, deltaScalePerFrame;

	private void Awake()
	{
		velocity = GameManager.instance.bulletVelocity;
		deltaScalePerFrame = GameManager.instance.deltaScalePerFrame;
	}
}
