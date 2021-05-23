using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {

	private string[] ownedNodes;
	[SerializeField] private string faction;
	private float maximumDistance = 8.0f; // 15 is wayyy too much. 8 is a litle too short imo. 10? (increase with larger map sizes?)
	GameObject[] possibleTargets;
	//private string[] nodeNames;
	private float nextTime = 0.0f;
	private float interval = 1.0f; //interval has to be greater than 1 for nextThink to work correctly. 
	private float nextThink = 0.0f;

	private int playerNodes;
	private int playerBlocks;
	private int ownedBlockCount;
	private bool markForDestruction = false;
	private bool markPlayerForDestruction = false;


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (nextTime < Time.time) {
			if (nextThink < 0) {
				SoCalledIntelligence(ownedNodes);
				nextThink = Random.Range(3.0f, 7.0f);
			} 
			nextThink--;
			nextTime += interval;
		}	

		if (markForDestruction == true) {
			GameObject[] blocksArray = GameObject.FindGameObjectsWithTag("Block");
			ownedBlockCount = 0;
			for (int i = 0; i < blocksArray.Length; i++) {
				if (blocksArray[i].GetComponent<Block>().faction == this.faction) {
					ownedBlockCount++;
				}
			}
			if (ownedBlockCount==0) {
				//decrement AIcount, destroy this
				GameObject.FindGameObjectWithTag("GameManager").GetComponent<GenerateMap>().DecrementAICount();
				Destroy(this.gameObject);
			}
		}

		if (markPlayerForDestruction == true) {
			playerBlocks = 0;
			GameObject[] blocksArray = GameObject.FindGameObjectsWithTag("Block");
			for (int i = 0; i < blocksArray.Length; i++) {
				if (blocksArray[i].GetComponent<Block>().faction == GameObject.FindGameObjectWithTag("GameManager").GetComponent<Controller>().GetPlayerFaction()) {
					playerBlocks++;
				}
			}
			if (playerBlocks == 0) {
				GameObject.FindGameObjectWithTag("GameManager").GetComponent<GenerateMap>().PlayerIsDead();
				GameObject.FindGameObjectWithTag("GameManager").GetComponent<GenerateMap>().DecrementAICount();
			}
		}
		
	}



	// public void SetNodeNames(string[] nodeNames) {
	// 	this.nodeNames = nodeNames;
	// }

	public void setAIFaction(string faction) {
		this.faction = faction;
	}
	// Find all Owned Nodes
	// ALSO call FindNearestNodes()
	public void ReScanNodes() {
		int ownedCount = 0;
		playerNodes = 0;
		playerBlocks = 0;
		string[] nodeNames = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GenerateMap>().GetNodeNames();

		//find gameobject with nodeNames[i], check if faction equals AI faction, increment ownedCount
		for (int i = 0;i < nodeNames.Length;i++) {
			if (GameObject.Find(nodeNames[i]).GetComponent<Node>().faction == faction) {
				ownedCount++;
			}
		}
		//Debug.LogWarning("ownedCount =" +ownedCount);
		//create ownedNodes[ownedCount]
		ownedNodes = new string[ownedCount];
		//find gameobject with nodeNames[i], check if faction equals AIFaction, if yes: set ownedNodes[k] to nodeNames[i]
		for (int i = 0, k = 0;i < nodeNames.Length;i++) {
			if (GameObject.Find(nodeNames[i]).GetComponent<Node>().faction == faction) {
				ownedNodes[k] = nodeNames[i];
				k++;
			}
			if (GameObject.Find(nodeNames[i]).GetComponent<Node>().faction == GameObject.FindGameObjectWithTag("GameManager").GetComponent<Controller>().GetPlayerFaction()) {
				playerNodes++;
			}
		}

		//check if AI OR PLAYER is dead
		GameObject[] blocksArray = GameObject.FindGameObjectsWithTag("Block");
		for (int i = 0; i < blocksArray.Length; i++) {
			if (blocksArray[i].GetComponent<Block>().faction == this.faction) {
				ownedBlockCount++;
			}
			if (blocksArray[i].GetComponent<Block>().faction == GameObject.FindGameObjectWithTag("GameManager").GetComponent<Controller>().GetPlayerFaction()) {
				playerBlocks++;
			}
		}

		//check if Player is dead
		if (playerNodes==0) {
			markPlayerForDestruction = true;
		}
		// check if AI is dead
		if (ownedNodes.Length == 0) {
			// mark AI for destruction
			markForDestruction = true;
			Debug.Log("AI "+this.gameObject.name+ "marked for destruction");
		} else {
			markForDestruction = false;
		}

	//	this.possibleTargets = findNearestNodes(maximumDistance, ownedNodes);
	}

	// CALL FROM UPDATE
	// A method where the AI will find all nodes within a maximum distance
	// then will decide whether to call Node.Spawn()
	void SoCalledIntelligence(string[] ownedNodes) {		
		//AI will decide whether to call Node.Spawn()

		//Loop: For each ownedNode, find unique possibleTargets, loop through ALL possibleTargets, call mathematicallyPerfectTiming(). If true, call ownedNode.Spawn()
		for (int k = 0; k < ownedNodes.Length; k++) {
			this.possibleTargets = findNearestNodesPerNode(maximumDistance, ownedNodes, GameObject.Find(ownedNodes[k])); // called ONCE EACH NODE
			for (int i = 0; i < possibleTargets.Length; i++) {
				if (possibleTargets[i]!=null) { // this is because possibleTargets has many null references
					if (mathematicallyPerfectTiming(GameObject.Find(ownedNodes[k]), possibleTargets[i].gameObject)) {
						GameObject.Find(ownedNodes[k]).GetComponent<Node>().Spawn(possibleTargets[i]);
					}
				}
			}
		}


	}


	bool mathematicallyPerfectTiming(GameObject ownedToCalculate, GameObject targetToCalculate) {
		bool perfectTiming = false;
		// if target.number <= perfectNumber, it will spawn.
		// PerfectNumber will be (target.number*2) + (the distance between source and target)*speed(which is 1)+1
		//(Will want to check this on paper)
		// FOR NOW I AM ADDING +(distance/2) to balance it out
		int perfectNumber = (targetToCalculate.GetComponent<Node>().GetNumber()*2)+2+(int)Vector3.Distance(ownedToCalculate.transform.position, targetToCalculate.transform.position)+((int)Vector3.Distance(ownedToCalculate.transform.position, targetToCalculate.transform.position)/2);

		//check if ownedToCalculate number is GREATER than or equal to perfectNumber
		if (ownedToCalculate.GetComponent<Node>().GetNumber() >= perfectNumber) {
			perfectTiming = true;
			//Debug.LogWarning("AI THONKING. perfectNumber = "+perfectNumber+" targetNum, distance = ("+targetToCalculate.GetComponent<Node>().GetNumber()+", "+(int)Vector3.Distance(ownedToCalculate.transform.position, targetToCalculate.transform.position)+") ownedNumber = "+ownedToCalculate.GetComponent<Node>().GetNumber()+"OwnedNode = "+ownedToCalculate.name);
		}
		


		return perfectTiming;
	}

	//AI will find all nodes within a maximum distance
	GameObject[] findNearestNodesPerNode(float maximumDistance, string[] ownedNodes, GameObject currentNode) {
		//How will it find the nearest nodes?
		//Create a new array of length(nodeCount), 
		int nodeCount = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GenerateMap>().GetNodeCount();
		GameObject[] nearestNodes = new GameObject[nodeCount];

		//loop through all Nodes and find any that AREN'T of AI Faction and are within the maximum distance
		//add them to nearestNodes
		for (int i = 0; i < nodeCount; i++) {
			GameObject tempNode = GameObject.Find("Node" + i);
			// need to compare the current owned node to the tempNode.
			// make sure the temp node isn't the current node OR of the same faction
			if ((currentNode != tempNode)&&(currentNode.GetComponent<Node>().faction != tempNode.GetComponent<Node>().faction)) { 
				if (Vector3.Distance(currentNode.transform.position, tempNode.transform.position) <= maximumDistance) { // need to call per node
					nearestNodes[i] = tempNode;
				} 
			}

		}
		//Debug.LogWarning("nearestNodes="+nearestNodes.ToString());
		return nearestNodes; //returned with null references
	}

	public string GetFaction() {
		return faction;
	}

	// // I wrote this wrong. This is a useful script for the AI to use to try and take the closest node. 
	// GameObject FindClosestNode(GameObject nodeToPlace, GameObject[] takenNodes) {
	// 		// create an array of "failures" for nodes that did not meet requirements
	// 		GameObject[] failures = new GameObject[nodeCount];

	// 		// given nodeToPlace and takenNodes array
	// 		// make a loop to compare (nodeToCompare) to all other nodes except rand
	// 		// check to see if that node is within the minimum distance OR if the node is taken
	// 		// if no, return that node. if yes, continue looping, increment tries, and add that node to the "failures" array

	// 		// make a loop that finds the closest node to nodeToPlace. Then test it for taken or minimum distance. 
	// 		// If it fails, add to failures. If it passes, return that node. Requires TWO loops.

	// 		// loop to test the closest node
	// 		for (int tries = 0; tries < 100; tries++) {
	// 			// loop to find the closest node
	// 			GameObject closestNode = null;
	// 			for (int i = 0; i < nodeCount; i++) {
	// 				GameObject nodeToCompare = GameObject.Find("Node"+i);

	// 				if (Vector3.Distance(transform.position, .transform.position) <= Vector3.Distance(transform.position, closestObject.transform.position));
	// 			}

				
				
	// 			bool taken = false;

	// 			//test if taken
	// 			for (int i = 0;i<takenNodes.Length;i++) {
	// 				if (takenNodes[i]==nodeToCompare) {
	// 					taken = true;
	// 				}
	// 			}

	// 			// is it within minimum distance?
	// 			bool withinMinDistance = ;

	// 			// enter loop body if it passes the above tests. if not, add to failures(is failures needed? Yes. 
	// 			if (withinMinDistance == false && taken == false) {
	// 				// now we 
	// 			} else {
	// 				failures[tries] = 
	// 			}
				

				


	// 			// Errors:
	// 			if (tries == 98) {
	// 				Debug.LogWarning("ERROR: Could Not Find Closest Node for nodeToPlace. Restarting with new Node. Details: GenerateMap.PlacePlayerAndAI(), unable to find GameObject closestNode.");
	// 			}
	// 		}
	// }


}
