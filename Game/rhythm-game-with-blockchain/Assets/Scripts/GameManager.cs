using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	static public GameManager instance;

	public int bpm;

	public GameObject enemy;
	public GameObject beatIndicator;
	public GameObject background;
	public List<Sprite> enemies;

	public Text firstForecastText, secondForecastText, playerStatus, enemyStatus, combo;

	public Text debug;

	private struct KeyPress
	{
		public float timing;
		public int key;
		public KeyPress(int key,float timing)
		{
			this.key = key;
			this.timing = timing;
		}
	}
	private KeyPress[] keyPresses;

	private float deltaTime;
	private int pressIndex;
	private readonly int[,] correctInput = { { 1, 2, 1, 2 }, { 0,0,0,0 }, { 3,3,3,3 }, { 3, 0, 3, 0 } };
	private readonly KeyCode[] keys = { KeyCode.Q, KeyCode.W, KeyCode.O, KeyCode.P };

	[HideInInspector]
	public float secondsPerHalfBeat;
	[HideInInspector]
	public float secondsPerBeat;

	[HideInInspector]
	public int playerActionIndex, enemyActionIndex, playerAttackStrength, enemyAttackStrength;

	private int halfBeatCnt;
	private bool isLevelUpTurn = false;

	private BackgroundManager backgroundManager;

	public bool gameOver = false;

	private void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(this);

		keyPresses = new KeyPress[4];
		deltaTime = 0;
		pressIndex = 0;

		secondsPerBeat = 60f / (float)bpm;
		secondsPerHalfBeat = secondsPerBeat / 2f;
		halfBeatCnt = 16;
	}

	private void Start()
	{
		backgroundManager = gameObject.GetComponent<BackgroundManager>();
		backgroundManager.GenerateBackground();
		StartCoroutine(HalfBeat());
	}

	IEnumerator HalfBeat()
	{
		int lastActionOfPlayer = -1;

		while (true)
		{
			halfBeatCnt = halfBeatCnt % 16 + 1;

			if (halfBeatCnt % 2 == 1)
			{
				GetIndicatorInvisible();
			}
			else GetIndicatorVisible();

			if (halfBeatCnt == 1)
				beatIndicator.GetComponent<SpriteRenderer>().color = Color.grey;
			if (halfBeatCnt == 9)
				beatIndicator.GetComponent<SpriteRenderer>().color = Color.green;
			

			if (isLevelUpTurn)
			{
				if(halfBeatCnt == 16)
				{
					if (Player.instance.skillPoint == 0)
					{
						isLevelUpTurn = false;
						Player.instance.UpdateSprite();
					}
				}
					
			}
			else if (halfBeatCnt == 1)
			{
				GenerateEnemy();
				lastActionOfPlayer = CheckInput();
				ClearInput();
			}
			else if (halfBeatCnt == 2)
			{
				if(!gameObject.GetComponent<AudioSource>().isPlaying)
					gameObject.GetComponent<AudioSource>().Play();

				if (Enemy.instance != null)
				{
					Player.instance.Act(lastActionOfPlayer);
					Enemy.instance.Act();
				}
				else Player.instance.Act(-2); //refresh player's state
				HandleActions();
				Player.instance.CheckLevelUp();
			}

			yield return new WaitForSeconds(secondsPerHalfBeat);
		}
	}

	private void GetIndicatorVisible()
	{
		beatIndicator.SetActive(true);
	}

	private void GetIndicatorInvisible()
	{
		beatIndicator.SetActive(false);
	}

	private void GenerateEnemy()
	{
		if (Enemy.instance != null) return;
		GameObject newEnemy = Instantiate(enemy);
		newEnemy.GetComponent<Enemy>().career = (Character.Career)UnityEngine.Random.Range(0, 4);
		newEnemy.GetComponent<SpriteRenderer>().sprite = enemies[(int)newEnemy.GetComponent<Enemy>().career];
		newEnemy.GetComponent<Enemy>().InitializeProperties();
		AnimationManager.instance.SetEnemy(enemy);
	}

	private void ClearInput()
	{
		pressIndex = 0;
		for (int i = 0; i < 4; ++i)
		{
			keyPresses[i].timing = 0f;
			keyPresses[i].key = -1;
		}
		deltaTime = 0f;	
	}

	private int times = 0;

	private int CheckInput()
	{
		debug.text = times + "\n";

		List<int> possibleMoves = new List<int>();
		Queue<int> movesToRemove = new Queue<int>();
		for (int i = 0; i < 4; ++i) possibleMoves.Add(i);
		for (int i = 0; i < 4; ++i)
		{
			foreach(int j in possibleMoves)
				if (correctInput[j, i] != keyPresses[i].key)
					movesToRemove.Enqueue(j);
			while (movesToRemove.Count > 0)
				possibleMoves.Remove(movesToRemove.Dequeue());
		}
		if (possibleMoves.Count == 0) return -1;
		int move = possibleMoves[0];

		float error = 0f;
		float maxError = secondsPerHalfBeat * secondsPerHalfBeat * 4f;

		for (int i = 0; i < 4; ++i)
		{
			float temp = keyPresses[i].timing - secondsPerBeat * i - secondsPerHalfBeat*9;
			error += temp * temp;
		}

		debug.text += playerActionIndex + "\n" + enemyActionIndex + "\n" + enemyAttackStrength
			+ "\n" + error
			+ "\n" + maxError;
		for (int i = 0; i < 4; ++i)
			debug.text += "\n"+keyPresses[i].key;
		times++;
		

		if (error < maxError) return move;
		else return -1;
	}

	private void HandleActions()
	{
		bool isPlayerHurt = false, isEnemyHurt = false;

		if (playerActionIndex == 0)//player attack
		{

			if (enemyActionIndex == 0)
			{
				//enemy defended
			}
			else
			{
				Enemy.instance.ReceiveAttack(playerAttackStrength);
				Enemy.instance.ClearStrengthStorage();
				isEnemyHurt = true;

				if (enemyActionIndex == 1 || enemyActionIndex == 2)
				{
					Player.instance.ReceiveAttack(enemyAttackStrength);
					Player.instance.FailToMove();
					isPlayerHurt = true;
				}
			}
		}
		else if (playerActionIndex == 1)
		{
			if (enemyActionIndex == 1)
			{
				Player.instance.ReceiveAttack(enemyAttackStrength);
				Player.instance.FailToMove();
				isPlayerHurt = true;
			}
			else if (enemyActionIndex == 2)
			{
				//player missed
			}
		}
		else if (playerActionIndex == 2)
		{
			if (enemyActionIndex == 2)
			{
				Player.instance.ReceiveAttack(enemyAttackStrength);
				Player.instance.FailToMove();
				isPlayerHurt = true;
			}
			else if (enemyActionIndex == 1)
			{
				//player missed
			}
		}
		else if (playerActionIndex == 3 || playerActionIndex == -1)
		{
			if (enemyActionIndex == 1 || enemyActionIndex == 2)
			{
				Player.instance.ReceiveAttack(enemyAttackStrength);
				Player.instance.FailToMove();
				isPlayerHurt = true;
			}
		}

		if (playerActionIndex == 0)
			AnimationManager.instance.ScreenSplash(2);
		if (isPlayerHurt)
			AnimationManager.instance.HurtPlayer();
		if (enemyActionIndex == 1 || enemyActionIndex == 2)
			AnimationManager.instance.ScreenSplash(enemyActionIndex - 1);
		else if (enemyActionIndex == 0)
			AnimationManager.instance.LetEnemyDefend();
		if (isEnemyHurt)
			AnimationManager.instance.HurtEnemy();
	}

	private void UpdateUI()
	{
		if (Enemy.instance != null)
		{
			firstForecastText.text = Enemy.instance.GetForecast(0);
			secondForecastText.text = Enemy.instance.GetForecast(1);

			enemyStatus.text = "Enemy\n"
			+ "HP: " + Enemy.instance.health + "\n"
			+ "STR: " + Enemy.instance.strength + "\n"
			+ "DEF: " + Enemy.instance.armour + "\n"
			+ "LUCK: " + Enemy.instance.luck + "\n";
		}
		else firstForecastText.text = secondForecastText.text = "";

		playerStatus.text = "Player\n"
			+ "HP: " + Player.instance.health + "\n"
			+ "STR: " + Player.instance.strength + "\n"
			+ "DEF: " + Player.instance.armour + "\n"
			+ "LUCK: " + Player.instance.luck + "\n";

		combo.text = "COMBO: " + Player.instance.combo;
	}

	private void Update()
	{
		UpdateUI();

		if (isLevelUpTurn&&Player.instance.skillPoint>=5)
		{
			for (int i = 0; i < 4; ++i)
			{
				if (Input.GetKeyDown(keys[i]))
				{
					Player.instance.skillPoint -= 5;
					switch (i)
					{
						case 0: Player.instance.health += 5;break;
						case 1: Player.instance.strength += 5;break;
						case 2: Player.instance.armour += 5;break;
						case 3: Player.instance.luck += 5;break;
					}
					break;
				}
			}
		}

		deltaTime += Time.deltaTime;

		if (!isLevelUpTurn && pressIndex<4 && halfBeatCnt>8)
		{
			for(int i = 0; i < 4; ++i)
			{
				if (Input.GetKeyDown(keys[i]))
				{
					keyPresses[pressIndex++] = new KeyPress(i, deltaTime);
					break;
				}
			}
		}
	}

	public void EnterLevelUpTurn()
	{
		isLevelUpTurn = true;
	}

	public void GameOver()
	{
		gameOver = true;
		SceneManager.LoadScene("Start");
	}
}
