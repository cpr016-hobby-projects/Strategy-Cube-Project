using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour {
	private Ray ray;
    private RaycastHit hit;

	private GameObject target = null;
	private GameObject source = null;
	private Object highlightPrefab;
	private GameObject highlight;

	// Player Information
	private string playerFaction;
	//private int playerNodeCount;

	// AI Information




	// Use this for initialization
	void Awake () {
		highlightPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Highlight.prefab", typeof(GameObject));

		//playerFaction = "Blue"; // hook to menu
	//	AICount = 1; // hook to menu

		//Spawn AI based on AIcount and player color
		//SpawnAI(AICount, playerFaction);

	}
	
	// Update is called once per frame

	public void SetPlayerFaction(string playerFaction) {
		this.playerFaction = playerFaction;
	}

	public string GetPlayerFaction() {
		return playerFaction;
	}

	void Update () {

		if (Input.GetKeyDown(KeyCode.Space)) {
			Debug.Break();
		}

		///////////////////////////////////
		// Player Selecting Nodes
		///////////////////////////////////
		// if source and target are both selected, call spawn function
		 if (Input.GetMouseButtonDown(0)) {
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				Debug.Log("Hit "+hit.collider.name);

				// if a node is hit, and node is of the player faction, set source, put highlight around it
				Node node = hit.collider.gameObject.GetComponent<Node>();
				if (node != null && node.faction == playerFaction) {
					source = hit.collider.gameObject;
					highlight = (GameObject)Instantiate(highlightPrefab, source.transform, instantiateInWorldSpace:false);
					//Debug.Log("highlight = "+highlight);
					//Debug.Log("source = "+source);
				}
			}
		}

		if (Input.GetMouseButtonUp(0)) {
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			// destroy highlight always,
			Destroy(highlight);

			// and if a node is hit AND source isn't null AND hit.collider.gameObject!=source, set target, call spawn. 
			// else, set source and target to null
			if (Physics.Raycast(ray, out hit) && (source != null) && (hit.collider.gameObject != source)) {
				Debug.Log("Hit "+hit.collider.name);

				Node node = hit.collider.gameObject.GetComponent<Node>();
				if (node != null) {
					target = hit.collider.gameObject;
					source.GetComponent<Node>().Spawn(target);
					
				} 
				source = null;
				target = null;
			}
		}

	}

}
