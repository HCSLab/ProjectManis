using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Enemy : Character {
	static public Enemy instance;

	public Career career;

	private int firstMove, secondMove;

	private readonly float[,] actionPossibility = { 
		{ 0.25f, 0.25f, 0.25f, 0.25f }, //warrier
		{ 0.25f, 0.2f, 0.2f, 0.35f }, //tank
		{ 0.35f, 0.2f, 0.2f, 0.25f }, //magician
		{ 0.15f, 0.3f, 0.3f, 0.25f } //thief
	};

	static readonly int[,] deltaProperties = {
		{ 10 + 10, 10 + 10, 5 + 5, 5 + 5 },
		{ 10 + 6, 10 + 12, 5 + 4, 5 + 8 },
		{ 10 + 5, 10 + 14, 5 + 9, 5 + 2 },
		{ 10 + 5, 10 + 14, 5 + 9, 5 + 2 }
	};

	public void InitializeProperties()
	{
		int deltaLevel = Player.instance.level - 1;
		level += deltaLevel;
		health += deltaLevel * deltaProperties[(int)career, 0];
		strength += deltaLevel * deltaProperties[(int)career, 1];
		luck += deltaLevel * deltaProperties[(int)career, 2];
		armour += deltaLevel * deltaProperties[(int)career, 3];
	}

	private void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(gameObject);

		firstMove = secondMove = 0;
		storageFactor = 1f;
	}

	public void Act()
	{
		DeployAction(firstMove);
		firstMove = secondMove;
		secondMove = GenerateNewMove();
	}

	private int GenerateNewMove()
	{
		float[] cdf = new float[5];
		cdf[0] = 0f;
		for (int i = 0; i < 4; ++i)
			cdf[i + 1] = cdf[i] + actionPossibility[(int)career, i];
		float rand = UnityEngine.Random.Range(0f, 1f);

		int ret = 0;
		for (int i = 0; i < 4; ++i)
			if (cdf[i] <= rand && rand <= cdf[i + 1])
			{
				ret = i;
				break;
			}

		return ret;
	}

	private void DeployAction(int actionIndex)
	{
		GameManager.instance.enemyActionIndex = actionIndex;

		switch (actionIndex)
		{
			case 0: break;//defend
			case 1: DeployAttack(); break;//attack left
			case 2: DeployAttack(); break;//attack right
			case 3: StoreStrength(); break;
			case -1: break;//do nothing
		}
	}

	private void DeployAttack()
	{
		float attackSize = (float)strength * storageFactor;
		if (UnityEngine.Random.Range(0f, 1f) <= (float)luck / 1000f)
			attackSize *= 2;

		GameManager.instance.enemyAttackStrength = (int)attackSize;

		ClearStrengthStorage();
	}

	public string GetForecast(int index)
	{
		if (index == 0) return IndexToAction(firstMove);
		if (index == 1) return IndexToAction(secondMove);
		return "Nothing";
	}

	private string IndexToAction(int actionIndex)
	{
		switch (actionIndex)
		{
			case 0: return "Defend";
			case 1: return "Attack L";
			case 2: return "Attack R";
			case 3: return "Store";
			default: return "Nothing";
		}
	}

	private void OnDestroy()
	{
		Player.instance.exp += 1;
	}
}
