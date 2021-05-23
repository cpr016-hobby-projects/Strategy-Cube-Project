using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// This script defines the action of "Spawning" done by the spawner part of the node
public class SpawnRate : MonoBehaviour {
	public double interval = 0.1;
	double nextTime = 0;

	public int cubeCount = 10;
	int cubeID = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if ((cubeCount > 0) && (Time.time >= nextTime)) {
			cubeID++;
			spawnBlock();
			spawnBlock();
			spawnBlock();
			nextTime += interval;
			cubeCount -= 3;
		}
	}


	void spawnBlock() {
			Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Block.prefab", typeof(GameObject));
			GameObject clone = Instantiate(prefab, new Vector3(0, 2, 0), Quaternion.identity) as GameObject;
			//Debug.Log("cubeCount = " + cubeCount);
			clone.name = "Cube" + cubeID;
	}
}
