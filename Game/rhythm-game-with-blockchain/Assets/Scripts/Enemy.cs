using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Enemy : Character {
	static public Enemy instance;

	public GameObject leftBarrier, rightBarrier;
	public int beatCnt, attackCnt;
	public State lastState;

	public bool enteredFirstBar = false;

	private void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(gameObject);

		attackCnt = 0;
		beatCnt = 7;
		state = State.Other;
		GameManager.instance.Beat += OnBeat;
	}

	private void OnBeat(object sender, EventArgs eventArgs)
	{
		beatCnt = beatCnt % 8 + 1;

		if (beatCnt == 1)
		{
			enteredFirstBar = true;

			if (lastState != Character.State.Other)
				Instantiate(lastState == Character.State.Left ? leftBarrier : rightBarrier).GetComponent<Barrier>().strength
					= strength;
		}

		if (beatCnt == 8)
		{
			lastState = state;

			++attackCnt;
			if (attackCnt == 3)
			{
				attackCnt = 0;
				state = Character.State.Other;
			}
			else state = UnityEngine.Random.Range(0, 2) == 0 ? Character.State.Left : Character.State.Right;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (lastState != State.Other) return;
		GameObject attacker = collision.gameObject;
		if (attacker.tag == "Enemy") return;
		ReceiveAttack(attacker.GetComponent<Bullet>().strength);
		Destroy(attacker);
		if (health <= 0)	
			Destroy(gameObject);
	}

	private void OnDestroy()
	{
		GameManager.instance.TryToProgress();
		GameManager.instance.ClearInstructionText();
		Player.instance.score += 1; //1 as the deltaScore
		GameManager.instance.Beat -= OnBeat;
	}

	public void Init(float health,float strength,float armour)
	{
		this.health = health;
		this.strength = strength;
		this.armour = armour;
	}
}
