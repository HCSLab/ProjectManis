using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {
	static public Player instance;

	public int combo;
	public int exp;

	public List<Sprite> playerSprites;

	public int weakness;

	private Career oldCareer;

	private void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(this);
		storageFactor = 1f;
		combo = 0;
		exp = 0;
	}

	public void Act(int actionIndex)
	{
		GameManager.instance.playerActionIndex = actionIndex;

		combo += 1;

		switch (actionIndex)
		{
			case 0: DeployAttack();break;
			case 1: MoveTo(true);break;
			case 2: MoveTo(false);break;
			case 3: StoreStrength();break;
			case -1: FailToMove();break;
			case -2: Refresh();break;
		}
	}

	private void Refresh()
	{
		storageFactor = 1f;
		combo -= 1;
	}

	public void FailToMove()
	{
		combo = 0;
		ClearStrengthStorage();
	}


	private void DeployAttack()
	{
		float attackSize = (float)strength * storageFactor * (1f + 0.1f * (float)combo);
		if (UnityEngine.Random.Range(0f, 1f) <= (float)luck / 1000f)
			attackSize *= 2;

		GameManager.instance.playerAttackStrength = (int)attackSize;
		
		ClearStrengthStorage();
	}

	public void MoveTo(bool isLeft)
	{
		gameObject.transform.position += new Vector3((isLeft ? -1 : 1) * 30f, 0f, 0f);
		StartCoroutine(MoveBackTo(!isLeft));
	}

	IEnumerator MoveBackTo(bool isLeft)
	{
		yield return new WaitForSeconds(GameManager.instance.secondsPerBeat);
		gameObject.transform.position += new Vector3((isLeft ? -1 : 1) * 30f, 0f, 0f);
		yield break;
	}

	public void CheckLevelUp()
	{
		if (exp >= 3 * UnityEngine.Mathf.Log(level + 3f,2.71828f))
		{
			exp = 0;
			oldCareer = GetCareer();
			LevelUp();
			GameManager.instance.EnterLevelUpTurn();
		}
	}

	public void UpdateSprite()
	{
		if (oldCareer != GetCareer())
			GameManager.instance.ShowCareerChange(oldCareer.ToString(), GetCareer().ToString());
		gameObject.GetComponent<SpriteRenderer>().sprite = playerSprites[(int)GetCareer()];
	}

	protected override void TryToDestroy()
	{
		DontDestroyOnLoad(gameObject);
		weakness = GameManager.instance.GetWeakness();
		GameManager.instance.GameOver();
	}
}
