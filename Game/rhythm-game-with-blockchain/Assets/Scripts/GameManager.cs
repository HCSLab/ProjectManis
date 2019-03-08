using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour {
	static public GameManager instance;

	public float bulletVelocity, barrierVelocity, deltaScalePerFrame, playerVelocity;

	public int bpm;

	public GameObject enemy;
	public GameObject beatIndicator;
	public GameObject background;
	public List<Sprite> backgrounds;

	private int backgroundIndex = 0;

	private bool isTryingProgressing = false;

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
	private readonly int[,] correctInput = { { 0, 1, 2, 3 }, { 3, 2, 1, 0 }, { 1, 2, 1, 2 } };
	private readonly KeyCode[] keys = { KeyCode.Q, KeyCode.W, KeyCode.O, KeyCode.P };

	public event EventHandler Beat;
	public float secondsPerBeat;
	private int beatCnt;

	private Character.State enemyStateToDisplay;

	public Text instructionText;
	public Text informationText;
	public Text correctnessText;

	private void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(this);

		keyPresses = new KeyPress[4];
		deltaTime = 0;
		pressIndex = 0;

		secondsPerBeat = 60f / (float)bpm;
		Beat += OnBeat;
		beatCnt = 8;
	}

	private void Start()
	{
		StartCoroutine(SendBeat());
	}

	IEnumerator SendBeat()
	{
		while (true)
		{
			if (isTryingProgressing)
			{
				bpm += 5;
				secondsPerBeat = 60f / (float)bpm;

				float factor = (float)bpm / (float)(bpm - 5);

				bulletVelocity *= factor;
				barrierVelocity *= factor;
				deltaScalePerFrame *= factor;
				playerVelocity *= factor;

				isTryingProgressing = false;
			}

			if (Beat != null)
				Beat(this, EventArgs.Empty);

			yield return new WaitForSeconds(secondsPerBeat);
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

	void OnBeat(object sender, EventArgs eventArgs)
	{
		background.GetComponent<SpriteRenderer>().sprite = backgrounds[backgroundIndex];
		backgroundIndex = (backgroundIndex + 1) % 2;

		GetIndicatorVisible();
		Invoke("GetIndicatorInvisible", secondsPerBeat / 2f);

		beatCnt = (beatCnt) % 8 + 1;

		if (beatCnt == 1)
		{
			UpdateInstructionText();

			CheckInput();

			pressIndex = 0;
			for (int i = 0; i < 4; ++i)
				keyPresses[i].key = 0;
			beatIndicator.GetComponent<SpriteRenderer>().color = Color.cyan;
		}
		else if (beatCnt == 3)
			Player.instance.MoveBack();
		else if (beatCnt == 4)
			deltaTime = 0;
		else if (beatCnt == 5)
			beatIndicator.GetComponent<SpriteRenderer>().color = Color.green;
		else if (beatCnt == 7)
		{
			if (Enemy.instance == null)
			{
				Instantiate(enemy).GetComponent<Enemy>().Init(2f, 1f, 1f);
			}
		}
	}

	private void CheckInput()
	{
		if (Enemy.instance == null||!Enemy.instance.enteredFirstBar) return;
		Character.State enemyState = Enemy.instance.lastState;
		float error = 0, raw;
		for(int i = 0; i < 4; ++i)
		{
			if (keyPresses[i].key != correctInput[(int)enemyState, i])
			{
				error += 23333f;
				break;
			}
			raw = keyPresses[i].timing - secondsPerBeat * (float)(i+1);
			error += raw * raw;
		}

		float upperBound = secondsPerBeat * secondsPerBeat;//upper bound = 4*(secondsPerbeat/2)^2

		if (enemyState == Character.State.Other)
		{
			if (error >= upperBound) StartCoroutine(ShowCorrectness(0));
			else if (error <= upperBound * 0.75f)
			{
				Player.instance.Shoot(1f);
				StartCoroutine(ShowCorrectness(2));
			}
			else
			{
				Player.instance.Shoot(0.5f);
				StartCoroutine(ShowCorrectness(1));
			}
		}
		else
		{
			if (error >= upperBound) StartCoroutine(ShowCorrectness(0));
			else if (error <= upperBound * 0.75f)
			{
				Player.instance.MoveTo(enemyState == Character.State.Right);
				StartCoroutine(ShowCorrectness(2));
			}
			else
			{
				Player.instance.MoveTo(enemyState == Character.State.Right);
				StartCoroutine(ShowCorrectness(1));
			}	
		}
	}

	IEnumerator ShowCorrectness(int degree)
	{
		correctnessText.gameObject.transform.parent.gameObject.SetActive(true);
		switch (degree)
		{
			case 0:
				correctnessText.text = "MISS";
				correctnessText.color = Color.red;
				break;
			case 1:
				correctnessText.text = "FINE";
				correctnessText.color = Color.cyan;
				break;
			case 2:
				correctnessText.text = "GREAT";
				correctnessText.color = Color.green;
				break;
		}

		yield return new WaitForSeconds(secondsPerBeat * 2);
		correctnessText.gameObject.transform.parent.gameObject.SetActive(false);
		yield break;
	}

	private void Update()
	{
		UpdateInformationText();

		deltaTime += Time.deltaTime;

		if (beatCnt>=4 &&pressIndex<4)
		{
			for(int i = 0; i < 4; ++i)
			{
				if (Input.GetKeyDown(keys[i]))
				{
					keyPresses[pressIndex++] = new KeyPress(i, deltaTime);

					AudioManager.instance.PlayNote(i);

					break;
				}
			}
		}
	}
	
	private void UpdateInstructionText()
	{
		if (Enemy.instance == null)
		{
			instructionText.text = "";
			return;
		}

		switch (Enemy.instance.state)
		{
			case Character.State.Left:
				instructionText.text = "Q -> W -> O -> P";
				break;
			case Character.State.Right:
				instructionText.text = "P -> O -> W -> Q";
				break;
			case Character.State.Other:
				instructionText.text = "W -> O -> W -> O";
				break;
		}
	}

	private void UpdateInformationText()
	{
		informationText.text = "SCORE: " + Player.instance.score
			+ "\n" + "BPM: " + bpm
			+ "\n" + "HEALTH: " + Player.instance.Health;
	}

	public void ClearInstructionText()
	{
		if (instructionText == null) return;
		instructionText.text = "";
	}

	public void TryToProgress()
	{
		isTryingProgressing = true;
	}

	public void GameOver()
	{
		SceneManager.LoadScene("Start");
	}
}
