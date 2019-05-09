using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour {
	public GameObject main, first, second, continueText, secondStartText;
	public Image continueBackground,secondStartBackground;
	public Sprite continuePressedSprite;
	public void QuitPressed()
	{
		Application.Quit();
	}

	public void StartPressed()
	{
		main.SetActive(false);
		first.SetActive(true);
	}

	public void ContinuePressed()
	{
		StartCoroutine(ContinuePressedCoroutine());
	}
	
	private IEnumerator ContinuePressedCoroutine()
	{
		continueText.SetActive(false);
		continueBackground.sprite = continuePressedSprite;
		yield return new WaitForSeconds(0.7f);
		first.SetActive(false);
		second.SetActive(true);
	}

	public void SecondStartPressed()
	{
		StartCoroutine(SecondStartPressedCoroutine());
	}

	private IEnumerator SecondStartPressedCoroutine()
	{
		secondStartText.SetActive(false);
		secondStartBackground.sprite = continuePressedSprite;
		yield return new WaitForSeconds(0.7f);
		SceneManager.LoadScene("MainGame");
	}
}
