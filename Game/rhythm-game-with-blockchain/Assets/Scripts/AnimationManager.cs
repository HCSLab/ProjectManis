using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour {
	static public AnimationManager instance;

	public GameObject protector,bloodParticleSystem;
	public GameObject[] screenSplash;

	private GameObject player, enemy;
	private float secondsPerBeat;

	private void Start()
	{
		instance = this;
		player = Player.instance.gameObject;
		secondsPerBeat = 60f / GameManager.instance.bpm;
	}

	public void SetEnemy(GameObject enemy)
	{
		this.enemy = enemy;
	}

	public void LetEnemyDefend()
	{
		StartCoroutine(EnemyDefend());
	}

	public void HurtEnemy()
	{
		StartCoroutine(Hurt(enemy));
		StartCoroutine(GenerateEnemysBlood());
	}

	public void ScreenSplash(int directionIndex)
	{
		StartCoroutine(GenerateScreeSplash(directionIndex));
	}

	public void HurtPlayer()
	{
		StartCoroutine(Hurt(player));
	}

	private IEnumerator EnemyDefend()
	{
		var currentProtector = Instantiate(protector);
		yield return new WaitForSeconds(secondsPerBeat * 2);
		Destroy(currentProtector);
		yield break;
	}
	
	private IEnumerator Hurt(GameObject target)
	{
		var spriteRenderer = target.GetComponent<SpriteRenderer>();
		spriteRenderer.color = Color.red;
		const int shakeTimes = 10;
		var deltaPosition = new Vector3(5f, 5f, 0);
		for(int i = 0; i < shakeTimes; ++i)
		{
			target.transform.position += (i % 2 == 0 ? 1 : -1) * deltaPosition;
			yield return new WaitForSeconds(secondsPerBeat*2/shakeTimes);
		}
		spriteRenderer.color = Color.white;
		yield break;
	}

	private IEnumerator GenerateEnemysBlood()
	{
		var currentBPS = Instantiate(bloodParticleSystem);
		yield return new WaitForSeconds(2 * secondsPerBeat);
		Destroy(currentBPS);
		yield break;
	}

	private IEnumerator GenerateScreeSplash(int directionIndex)//0 left, 1 right, 2 middle
	{
		var currentSplash = Instantiate(screenSplash[directionIndex]);
		yield return new WaitForSeconds(0.5f);
		Destroy(currentSplash);
		yield break;
	}
}
