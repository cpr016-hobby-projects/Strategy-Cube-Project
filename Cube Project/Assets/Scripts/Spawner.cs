using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is responsible for the "spawner" properties of a node
public class Spawner : MonoBehaviour {

	[SerializeField]private float nextTime = 0.0f;
	private float interval = 0.1f;
	private bool spawned = false;
	
	private int blockCount;
	private GameObject prefabToSpawn;
	private GameObject destination;
	private string faction;


	// Use this for initialization
	void Awake () {
		Initialization();
	}

	public void Initialization() {

		blockCount = 0;
		prefabToSpawn = null;
		destination = null;
		faction = "";
	//	Debug.Log("Spawner Init");
	}

	public void Spawn(int blockCount, GameObject prefabToSpawn, GameObject destination, string faction) {
		this.blockCount = blockCount;
		this.prefabToSpawn = prefabToSpawn;
		this.destination = destination;
		this.faction = faction;
		//Debug.Log("Spawner Created. blockCount=" + this.blockCount + " prefab=" + prefabToSpawn + " destination=" + destination + " faction=" + faction);

	}

	
	// Update is called once per frame
	void Update () {
		//Debug.Log("Spawner Checkin. blockCount=" + blockCount + " prefab=" + prefabToSpawn + " destination=" + destination + " faction=" + faction);
		if (nextTime < Time.time) {
			//Debug.Log("YEET");
			if (spawned == false) {
				nextTime = Time.time;
			}
			if (blockCount > 0) {
				//Debug.Log("YOTE");
				GameObject clone = (GameObject)Instantiate(prefabToSpawn, this.transform.position, Quaternion.identity, this.transform.parent);
				//Debug.Log("SPAWNED BLOCK: Spawner Transform=" + this.transform.position);
			
				clone.GetComponent<Block>().Initialization(destination, faction);
				//Debug.Log("COLLIDERSpawner, post-init:" + clone.GetComponent<Collider>().enabled);	
				blockCount--;
				spawned = true; // this is here to prevent 
			}
			nextTime += interval;
		}

		if (blockCount == 0 && spawned == true) {
			Destroy(this.gameObject);
		}
	}




}
