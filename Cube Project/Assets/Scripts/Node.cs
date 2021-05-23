using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Node : MonoBehaviour {

	public string faction;
	[SerializeField]private int number;

	private string[] factionArray;
	private Color[] colorArray;

	private float nextTime = 0.0f;
	private float interval = 1.0f;

	public void Initialization(int number, string faction, string[] factionArray, Color[] colorArray) {
		this.faction = faction;
		this.factionArray = factionArray;
		this.colorArray = colorArray;
		this.number = number;

		setMatColor(faction, factionArray, colorArray);

		Debug.Log("faction="+faction+" number="+number+" nodeName="+this.name);

		nextTime = (int)Time.time; // so that the player doesn't have the restart the game to play again. Needs to be int or mod3 wont work.
		
	}
	
	// Update is called once per frame
	void Update () {
		// call every 1 second(change method if i change interval)
		if (nextTime < Time.time) {
			RegenerateNumber();
			nextTime += interval;
		}

		this.transform.Find("TextObject").gameObject.GetComponent<TextMesh>().text = number.ToString();
	}

		////////////////////////////
		//HELPER METHODS
		///////////////////////////

	public void OnTriggerEnter(Collider block) {
		//ONLY DOES ANYTHING BELOW if this.gameObject = block.GetComponent<Block>().destination
		if (block.GetComponent<Block>().destination == this.gameObject) {
			//destroys any block, increases count or decreases depending on faction
			// also, 
		
			// change faction, rescan ai + player
			if (number == 0) {
				faction = block.gameObject.GetComponent<Block>().faction;
				setMatColor(faction, factionArray, colorArray);
				Debug.Log("Faction Changed Team!");
				ReScanAllAI();
				//play capture node sound
				if (faction == GameObject.Find("GameManager").GetComponent<Controller>().GetPlayerFaction()) {
					GameObject.Find("MenuManager").GetComponent<SoundManager>().playCaptureNode();
				}
			}
			//change number
			if (block.gameObject.GetComponent<Block>().faction == faction) {
				number++;
			} else {
				number--;
			}
			//erase faction if number is 0. also rescan ai + player
			if (number == 0) {
				//play lose node sound
				if (faction == GameObject.Find("GameManager").GetComponent<Controller>().GetPlayerFaction()) {
					GameObject.Find("MenuManager").GetComponent<SoundManager>().playLoseNode();
				}
				faction = "Neutral";
				setMatColor(faction, factionArray, colorArray);
				Debug.Log("Faction Changed to Neutral!");
				ReScanAllAI();
			}

			//destroy block
			Destroy(block.gameObject);
		}

	}

	void ReScanAllAI() {
		GameObject[] AIArray = GameObject.FindGameObjectsWithTag("AI");
		for (int i = 0; i < AIArray.Length; i++) {
			AIArray[i].GetComponent<AIManager>().ReScanNodes();
		}

	}

	private void RegenerateNumber() {
		if (faction == "Neutral") {
			if ((nextTime % 3) == 0) {
				number += Random.Range(0,2);
			}
		} else {
			number++;
		}
	}

	public void setMatColor() {
		if (faction==null || factionArray==null || colorArray==null) {
			Debug.LogError("Node.setMatColor() called with null parameters!");
			Debug.Break();
		}
		setMatColor(faction, factionArray, colorArray);
	}

	public void setMatColor(string faction, string[] factionArray, Color[] colorArray) {

		// find color index
		int index = System.Array.IndexOf(factionArray, faction);

		// assign color to material
		Material material = this.transform.Find("Center").gameObject.GetComponent<Renderer>().material;
		material.color = colorArray[index];

		//Debug.Log(this.transform.Find("Center"));
		//Debug.Log(faction);
		//Debug.Log(colorArray[index]);

	}
	public void Spawn(GameObject target) {
		int blockCount = number / 2;
		number = number - blockCount;
		GameObject block = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Block.prefab", typeof(GameObject));
		Object spawnerPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Spawner.prefab", typeof(GameObject));

		//Debug.Log("Node.Spawn Called");
		//Debug.Log("Node.BlockCount="+blockCount);

		GameObject spawner = (GameObject)Instantiate(spawnerPrefab, this.transform.position, Quaternion.identity, this.transform);
		//spawner.GetComponent<Spawner>().Init
		spawner.GetComponent<Spawner>().Spawn(blockCount, block, target, this.faction);

		
	}

	public int GetNumber() {
		return number;
	}
}
