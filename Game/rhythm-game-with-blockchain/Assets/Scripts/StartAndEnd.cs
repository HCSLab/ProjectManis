using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartAndEnd : MonoBehaviour {
	public GameObject uploadButton, uploadingInfo, uploadSuccessInfo;
	public InputField nameInput, privateKeyInput;
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
		if (privateKeyInput.text != string.Empty)
			genesisContractService.UpdatePrivateKey(privateKeyInput.text);
		uploadButton.SetActive(false);
		uploadingInfo.SetActive(true);
		var name = nameInput.text == string.Empty ? "NONE" : nameInput.text;
		yield return genesisContractService.InsertCharacter(player,name);
		uploadingInfo.SetActive(false);
		uploadSuccessInfo.SetActive(true);
	}

	public void GameStart()
	{
		if(playerInstance!=null)Destroy(playerInstance);
		SceneManager.LoadScene("MainGame");
	}
}
