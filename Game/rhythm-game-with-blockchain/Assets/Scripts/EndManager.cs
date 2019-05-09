using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndManager: MonoBehaviour {
	public GameObject uploadButton, uploadingInfo, uploadSuccessInfo, txnHash;
	public InputField nameInput, privateKeyInput;
	private Player player;
	private GameObject playerInstance;

	private void Start()
	{
		playerInstance = GameObject.FindGameObjectWithTag("Player");
		player = playerInstance.GetComponent<Player>();
		Debug.Log("Weakness: "+ player.weakness);
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
		txnHash.SetActive(true);
		txnHash.GetComponent<Text>().text = "Txn Hash:\n" + genesisContractService.lastUploadTxnHash;
	}

	public void GameStart()
	{
		Destroy(playerInstance);
		SceneManager.LoadScene("MainGame");
	}
}
