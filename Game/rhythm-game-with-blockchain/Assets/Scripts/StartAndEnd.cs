using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartAndEnd : MonoBehaviour {
	public GameObject uploadButton, uploadingInfo, uploadSuccessInfo;

	private Character player;
	private GameObject playerInstance;

	private void Start()
	{
		playerInstance = GameObject.FindGameObjectWithTag("Player");
		if (playerInstance == null) return;
		player = playerInstance.GetComponent<Character>();
		playerInstance.SetActive(false);
	}

	public void UploadCharacter()
	{
		StartCoroutine(UploadCharacterCoroutine());
	}

	private IEnumerator UploadCharacterCoroutine()
	{
		var genesisContractService = GetComponent<GenesisContractService>();
		uploadButton.SetActive(false);
		uploadingInfo.SetActive(true);
		yield return genesisContractService.InsertCharacter(player);
		uploadingInfo.SetActive(false);
		uploadSuccessInfo.SetActive(true);
	}

	public void GameStart()
	{
		if(playerInstance!=null)Destroy(playerInstance);
		SceneManager.LoadScene("MainGame");
	}
}
