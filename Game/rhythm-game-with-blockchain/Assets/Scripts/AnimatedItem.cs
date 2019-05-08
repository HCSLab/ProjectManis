using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedItem : Tile {
	public Sprite[] sprites;

	private float secondsPerFlash;
	private SpriteRenderer spriteRenderer;

	private void Start () {
		secondsPerFlash = GameManager.instance == null?1f/16f:GameManager.instance.secondsPerBeat * 4f / sprites.Length;
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		StartCoroutine(Flash());
	}
	
	private IEnumerator Flash()
	{
		int idx = 0,len = sprites.Length;
		while (true)
		{
			spriteRenderer.sprite = sprites[idx];
			idx = (idx + 1) % len;
			yield return new WaitForSeconds(secondsPerFlash);
		}
	}
}
