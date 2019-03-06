using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {
	static public Player instance;

	public GameObject bullet;
	public int score;
	
	private void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(this);
		score = 0;
		health = 3;
		strength = 2;
		armour = 0;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		GameObject attacker = collision.gameObject;
		if (attacker.tag == "Player") return;
		ReceiveAttack(attacker.GetComponent<Barrier>().strength);
		Destroy(attacker);
		if (health <= 0) GameManager.instance.GameOver();
	}

	public void Shoot(float factor)
	{
		state = State.Other;
		Instantiate(bullet).GetComponent<Bullet>().strength = factor*strength;
	}

	public void MoveTo(bool isLeft)
	{
		state = isLeft ? State.Left : State.Right;
		StartCoroutine(Move(GameManager.instance.playerVelocity, isLeft));
	}

	IEnumerator Move(float velocity,bool isLeft)
	{
		float directionFactor = isLeft ? -1f : 1f;
		for(int i = 0; i < 15; ++i)
		{
			transform.position = transform.position + new Vector3(velocity * directionFactor, 0f, 0f);
			yield return new WaitForSeconds(0.02f);
		}
	}

	public void MoveBack()
	{
		if(state!=State.Other) MoveTo(state == State.Right);
		state = State.Other;
	}

	public void Init(int health, int strength, int armour)
	{
		this.health = health;
		this.strength = strength;
		this.armour = armour;
	}

	public float Health
	{
		get
		{
			return health;
		}
	}
}
