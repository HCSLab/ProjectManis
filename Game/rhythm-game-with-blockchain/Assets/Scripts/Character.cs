using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
	protected float health, strength, armour;

	public enum State
	{
		Left = 0, Right = 1, Other = 2
	}

	public State state = State.Other;

	protected void ReceiveAttack(float attakerStrength)
	{
		health -= attakerStrength - armour;
	}
}
