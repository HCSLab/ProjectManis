using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour {
	public Sprite[] leftWalls, rightWalls, floors, items;
	public GameObject emptyTile, emptyItem, screenBorderDetector;
	public int itemsPerScreen = 4;
	
	private const int spriteLength = 32;

	public void GenerateBackground()
	{
		//generate basic walls and floor
		for (int i = -4; i <= 4; ++i)
		{
			for (int j = -3; j <= 2; ++j)
			{
				GenerateObject(emptyTile, floors[0], new Vector3((j + 0.5f) * spriteLength, (i - 0.5f) * spriteLength, 0f));
			}
			GenerateObject(emptyTile, leftWalls[0], new Vector3(-3.5f * spriteLength, (i - 0.5f) * spriteLength, 0f));
			GenerateObject(emptyTile, rightWalls[0], new Vector3(3.5f * spriteLength, (i - 0.5f) * spriteLength, 0f));
		}

		GenerateItems();

		GenerateAScreenAbove();
	}

	private GameObject GenerateObject(GameObject prefab,Sprite sprite,Vector3 position)
	{
		GameObject newObject = Instantiate(prefab);
		newObject.GetComponent<SpriteRenderer>().sprite = sprite;
		newObject.transform.position = position;
		return newObject;
	}

	private void GenerateItems()
	{
		for(int i = 0; i < itemsPerScreen; ++i)
		{
			var position = new Vector3(Random.Range(spriteLength*1.5f,spriteLength*3f)*(i<4?1:-1), Random.Range(-120f, 120f), 0f);
			GenerateObject(emptyItem, items[Random.Range(0, items.Length)],position);
		}
	}

	public void GenerateAScreenAbove()
	{
		List<GameObject> screenAbove = new List<GameObject>();
		for (int i = -4; i <= 4; ++i)
		{
			for (int j = -3; j <= 2; ++j)
			{
				screenAbove.Add(GenerateObject(emptyTile, floors[0], new Vector3((j + 0.5f) * spriteLength, (i - 0.5f) * spriteLength, 0f)));
			}
			screenAbove.Add(GenerateObject(emptyTile, leftWalls[0], new Vector3(-3.5f * spriteLength, (i - 0.5f) * spriteLength, 0f)));
			screenAbove.Add(GenerateObject(emptyTile, rightWalls[0], new Vector3(3.5f * spriteLength, (i - 0.5f) * spriteLength, 0f)));
		}
		for (int i = 0; i < itemsPerScreen; ++i)
		{
			var position = new Vector3(Random.Range(spriteLength * 1.5f, spriteLength * 3f) * (i < 4 ? 1 : -1), Random.Range(-120f, 120f), 0f);
			screenAbove.Add(GenerateObject(emptyItem, items[Random.Range(0, items.Length)], position));
		}

		screenAbove.Add(Instantiate(screenBorderDetector));
		foreach (var go in screenAbove)
		{
			go.transform.position += new Vector3(0f, spriteLength * 4.5f, 0f);
		}
	}
}
