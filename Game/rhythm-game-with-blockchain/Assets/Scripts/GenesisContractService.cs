using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.JsonRpc.UnityClient;
using UnityEngine;
using System.Collections;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Hex.HexTypes;
using System.Numerics;

public class GenesisContractService : MonoBehaviour
{

	[FunctionOutput]
	public class CharacterDetailsDTO : IFunctionOutputDTO
	{
		[Parameter("string", "_name", 1)]
		public string _name { get; set; }
		[Parameter("uint", "_hp", 2)]
		public uint _hp { get; set; }
		[Parameter("uint", "_mp", 3)]
		public uint _mp { get; set; }
		[Parameter("uint", "_str", 4)]
		public uint _str { get; set; }
		[Parameter("uint", "_int", 5)]
		public uint _int { get; set; }
		[Parameter("uint", "_san", 6)]
		public uint _san { get; set; }
		[Parameter("uint", "_luck", 7)]
		public uint _luck { get; set; }
		[Parameter("uint", "_charm", 8)]
		public uint _charm { get; set; }
		[Parameter("uint", "_mt", 9)]
		public uint _mt { get; set; }
		[Parameter("string", "_optionalAttrs", 10)]
		public string _optionalAttrs { get; set; }
	}

	public CharacterDetailsDTO requestedCharacter = null;

	private static string _url = "https://kovan.infura.io";//main net: "https://mainnet.infura.io"
	private static string ABI = @"[{""constant"":true,""inputs"":[],""name"":""getCharacterNo"",""outputs"":[{""name"":""_characterNo"",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""_hp"",""type"":""uint256""},{""name"":""_mp"",""type"":""uint256""},{""name"":""_str"",""type"":""uint256""},{""name"":""_intelli"",""type"":""uint256""},{""name"":""_san"",""type"":""uint256""},{""name"":""_luck"",""type"":""uint256""},{""name"":""_charm"",""type"":""uint256""},{""name"":""_mt"",""type"":""uint256""}],""name"":""checkLegal"",""outputs"":[{""name"":""_checkresult"",""type"":""uint256""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""version"",""outputs"":[{""name"":"""",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""_id"",""type"":""uint256""},{""name"":""isPositiveEffect"",""type"":""uint256""}],""name"":""affectCharacter"",""outputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""getRand"",""outputs"":[{""name"":""_rand"",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[{""name"":""_characterId"",""type"":""uint256""}],""name"":""getCharacterDetails"",""outputs"":[{""name"":""_name"",""type"":""string""},{""name"":""_hp"",""type"":""uint256""},{""name"":""_mp"",""type"":""uint256""},{""name"":""_str"",""type"":""uint256""},{""name"":""_int"",""type"":""uint256""},{""name"":""_san"",""type"":""uint256""},{""name"":""_luck"",""type"":""uint256""},{""name"":""_charm"",""type"":""uint256""},{""name"":""_mt"",""type"":""uint256""},{""name"":""_optionalAttrs"",""type"":""string""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""_name"",""type"":""string""},{""name"":""_hp"",""type"":""uint256""},{""name"":""_mp"",""type"":""uint256""},{""name"":""_str"",""type"":""uint256""},{""name"":""_intelli"",""type"":""uint256""},{""name"":""_san"",""type"":""uint256""},{""name"":""_luck"",""type"":""uint256""},{""name"":""_charm"",""type"":""uint256""},{""name"":""_mt"",""type"":""uint256""},{""name"":""_optionalAttrs"",""type"":""string""}],""name"":""insertCharacter"",""outputs"":[{""name"":"""",""type"":""uint256""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""characterNo"",""outputs"":[{""name"":"""",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""_id"",""type"":""uint256""},{""name"":""_hp"",""type"":""uint256""},{""name"":""_mp"",""type"":""uint256""},{""name"":""_str"",""type"":""uint256""},{""name"":""_intelli"",""type"":""uint256""},{""name"":""_san"",""type"":""uint256""},{""name"":""_luck"",""type"":""uint256""},{""name"":""_charm"",""type"":""uint256""},{""name"":""_optionalAttrs"",""type"":""string""}],""name"":""setCharacterAttributes"",""outputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""constructor""}]";
	private static string contractAddress = "0x082e81fb00FAb5b83aA6dB202a97A9Fb94096aBe";//main net: "0x8cae244D8E274058d1C10C8ED731994A10b7e7d2"

	private string accountAddress = "0xD978a2B22B864D34689117cF6E870B43F237eabA";
	private string accountPrivateKey = "0x15f5ff9b0095650e0fb454e6760aa462334ba1230315b051ad53d1f7118e59ae";

	private Contract contract = new Contract(null, ABI, contractAddress);

	private int characterNo = 1;

	public void RequestCharacterNo()
	{
		StartCoroutine(GetCharacterNo());
	}

	public Coroutine RequestRandomCharacterCoroutine()
	{
		return StartCoroutine(GetCharacterDetails(UnityEngine.Random.Range(0, characterNo)));
	}

	public void UpdatePrivateKey(string newPrivateKey)
	{
		accountPrivateKey = newPrivateKey;
		accountAddress = Nethereum.Signer.EthECKey.GetPublicAddress(accountPrivateKey);
		Debug.Log("New pri key: " + newPrivateKey);
		Debug.Log("New pub key: " + accountAddress);
	}
	public IEnumerator RequestAndPrintCharacterDetails(int index)
	{
		yield return StartCoroutine(GetCharacterDetails(index));
		Debug.Log("Requested Character Info: " + requestedCharacter._name + " " +
			requestedCharacter._hp + " " +
			requestedCharacter._str + " " +
			requestedCharacter._int + " " +
			requestedCharacter._luck + " " +
			requestedCharacter._optionalAttrs);
	}

	private IEnumerator GetCharacterNo()
	{
		// We create a new pingsRequest as a new Eth Call Unity Request
		var characterNoRequest = new EthCallUnityRequest(_url);
		var characterNoCallInput = CreateCharacterNoCallInput();
		Debug.Log("Getting CharacterNo...");
		// Then we send the request with the pingsCallInput and the most recent block mined to check
		// And we wait for the response..
		yield return characterNoRequest.SendRequest(characterNoCallInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());
		if (characterNoRequest.Exception == null)
		{
			characterNo = DecodeCharacterNo(characterNoRequest.Result);
			Debug.Log("CharacterNo (HEX): " + characterNoRequest.Result);
			Debug.Log("CharacterNo (INT):" + characterNo);
		}
		else
		{
			Debug.Log("Error submitting GetCharacterNo tx: " + characterNoRequest.Exception.Message);
		}
	}

	private Function GetCharacterNoFunction()
	{
		return contract.GetFunction("characterNo");
	}

	private CallInput CreateCharacterNoCallInput()
	{
		// For this transaction to the contract we dont need inputs,
		// its only to retreive the quantity of Ping transactions we did. (the pings variable on the contract)
		var function = GetCharacterNoFunction();
		return function.CreateCallInput();
	}

	private int DecodeCharacterNo(string characterNo)
	{
		// We use this function later to parse the result of encoded pings (Hexadecimal 0x0f)
		// into a decoded output for easier readability (Integer 15)
		var function = GetCharacterNoFunction();
		return function.DecodeSimpleTypeOutput<int>(characterNo);
	}

	private IEnumerator GetCharacterDetails(int index)
	{
		Debug.Log("Fetching character " + index);
		requestedCharacter = null;
		var function = contract.GetFunction("getCharacterDetails");
		var input = function.CreateCallInput(index);
		var request = new EthCallUnityRequest(_url);
		yield return request.SendRequest(input, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());
		requestedCharacter = function.DecodeDTOTypeOutput<CharacterDetailsDTO>(request.Result);
		Debug.Log("Character fetched");
	}

	public Coroutine InsertCharacter(Character ch, string name)
	{
		return StartCoroutine(InsertCharacterRequest(ch,name));
	}

	private TransactionInput CreateInsertCharacterInput(Character ch, string name)
	{
		return contract.GetFunction("insertCharacter").CreateTransactionInput(
			accountAddress,
			new HexBigInteger(2000000),
			new HexBigInteger(200),
			new HexBigInteger(0),
			name, 
			ch.health / 10,
			50,
			ch.strength / 10,
			ch.armour / 10,
			50,
			ch.luck / 10,
			50,
			0,
			"NONE"
			);
	}

	private IEnumerator InsertCharacterRequest(Character ch,string name)
	{
		var transactionInput = CreateInsertCharacterInput(ch, name);
		Debug.Log("Inserting Character with luck of " + ch.luck +" and name of " + name);
		var transactionSignedRequest = new TransactionSignedUnityRequest(_url, accountPrivateKey);
		yield return transactionSignedRequest.SignAndSendTransaction(transactionInput);
		if(transactionSignedRequest.Exception == null)
		{
			Debug.Log("Character Successfully Inserted!");
		}
		else
		{
			Debug.Log("Error inserting character: " + transactionSignedRequest.Exception.Message);
		}
	}
}