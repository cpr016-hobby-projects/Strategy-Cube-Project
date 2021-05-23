using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GenerateMap : MonoBehaviour {

	// General Variables - these don't change
	private GameObject nodePrefab;
	private int nodeID = 0;
	[SerializeField]private int nodeCount = 0;
	private string[] nodeNames;
	private bool playerDead = false;

	private Object AIPrefab;

	//private int factionCount = 0;

	private bool playerSet = false; //whether the player has been set yet
	private int AISetCount = 0; 
	GameObject[] AI;


	// Settings - these can change
		// Factions
	private string[] factionArray = {"Magenta", "Teal", "Neutral", "Green", "Yellow"};
	private Color[] colorArray = {Color.magenta, new Color(0,1,1,1), Color.white, Color.green, Color.yellow};
		// Node Creation
	private string defaultFaction = "Neutral";
	private int defaultBlockNumber = 2;
	private float maxGridX = 20.0f;
	private float maxGridZ = 20.0f;
	private float chanceOfCreation = 0.9f;
		// AI Creation
	private int AICount; //hook to menu? for now, default lessthan 3 because there are 4 corners.
	private string[] AIFactions = {"Magenta", "Teal", "Green", "Yellow"};
	private float minimumDistance = 4.0f;
		// Player Creation
	private string playerFaction; // hook to menu




	void Start () {
		setPlayerFaction(GameObject.FindWithTag("MenuManager").GetComponent<MenuManager>().getPlayerFaction());
		setAICount(GameObject.FindWithTag("MenuManager").GetComponent<MenuManager>().getAICount());
		Debug.Log("AI Count: "+GameObject.FindWithTag("MenuManager").GetComponent<MenuManager>().getAICount());
		PopulateGrid();
		PopulateNodeArray();
		CreateAI();
		CreatePlayer(playerFaction);
		PlacePlayerAndAI(AICount); //actually, instead of corners, I want to just spawn them above a minimum distance from the player and other AI. 
		ActivateAI();
	}

	void PopulateGrid()
	{
		GameObject nodePrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Node.prefab", typeof(GameObject));
		for (float x=0; x<maxGridX; x+=5) {
			for (float z=0; z<maxGridZ; z+=5) {
				float xOffset = z + Random.Range(-2f, 1.5f);
				float zOffset = x + Random.Range(-2f, 1.5f);
				if (Random.Range (0.0f, 1.0f) < chanceOfCreation) {
					GameObject nodeTemp = Instantiate(nodePrefab, new Vector3 (xOffset, 0.3f, zOffset), Quaternion.identity);
					nodeTemp.name = "Node" + nodeID;
					nodeTemp.GetComponent<Node>().Initialization(defaultBlockNumber, defaultFaction, factionArray, colorArray);

					nodeID++;
					nodeCount++;
				}

			}
		}

		Debug.Log("Grid Populated!");
	}

	void PopulateNodeArray() {
		nodeNames = new string[nodeCount];
		for (int i = 0;i < nodeCount;i++) {
			nodeNames[i] = "Node" + i;
		}

		Debug.Log("Node Array Populated!");
	}
	
	void CreateAI() {
		AIPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/AI.prefab", typeof(GameObject));

		// Set Array of AI to store AI.
		AI = new GameObject[AICount];

		for (int i = 0;i<AICount;i++) {
			GameObject AItemp = (GameObject)Instantiate(AIPrefab);
			AItemp.name = "AI " + i;
			//AItemp.GetComponent<AIManager>().SetNodeNames(nodeNames);
			//AItemp.GetComponent<AIManager>().ReScanNodes(); // DELETED because it doesn't work this early. Putting it in a new method
			


			//set AI faction and color.
			//take AIFactions, loop through and set AIFactions[i] to null if it equals player faction.
			for (int k = 0;k < AIFactions.Length; k++) {
				if (AIFactions[k] == playerFaction) {
					AIFactions[k] = null;
				}
			}

			//Make loop, generate random int in range 0 to AIFactions.Length
			// If AIFactions[rand] is not null, set the AI Faction and set AIFactions[rand] to null, and set AI[i] to AItemp
			// Loop ends when AItemp faction is set
			// Also, as an infinite loop precaution, set a counter that will break out of the loop if it hits a cap
			int maxLoopCap = 100;
			int loopCount = 0;
			bool loop = true;
			while (loop) {
				int rand = Random.Range(0, AIFactions.Length); // max is AIFactions.Length - 1, according to definition. 
					//But online documentation says otherwise. Because the loop I am using doesn't allow null,
					//It should work anyway.
				if (AIFactions[rand] != null) {
					AItemp.GetComponent<AIManager>().setAIFaction(AIFactions[rand]);
					
					Debug.Log("Set AI"+rand+" to "+AIFactions[rand]);

					AIFactions[rand] = null;
					loop = false;
					// add AItemp to array of active AI
					AI[i] = AItemp;

					
				}

				//infinite loop precautions
				loopCount++;
				if (loopCount > maxLoopCap) {
					Debug.Log("ERROR: Infinite Loop maxLoopCap reached. Breaking. Details: In the while loop of CreateAI() in GenerateMap.cs");
					Debug.Break();
				}
			}

		}
	}

	void CreatePlayer(string playerFaction) {
		this.gameObject.GetComponent<Controller>().SetPlayerFaction(playerFaction);
		Debug.Log("Player/Controller Created!");
	}


	void PlacePlayerAndAI(int AICount) {
		// create empty array of node gameobjects. 
		GameObject[] takenNodes = new GameObject[AICount+1]; 

		// use for loop. while playerSet is false OR AISetCount < AICount
		for (int loopTries = 0; loopTries < 300 && (playerSet == false || AISetCount < AICount); loopTries++) {
			// use random.range to select a random node
			bool taken = false;

			int rand = Random.Range(0, nodeCount+1);
			GameObject nodeToPlace = GameObject.Find("Node" + rand);

			Debug.Log("Loop once"+AICount+AISetCount);

			// check if random node is taken or not. If yes, continue looping. If no, enter body
			for (int i = 0;i<takenNodes.Length;i++) {
				if (takenNodes[i]==nodeToPlace) {
					taken = true;
				}
			}
			if (taken == false) {
				// find the closest node. this will make sure there are no other TAKEN nodes within the minimum distance
						//GameObject closestNode = FindClosestNode(nodeToPlace, takenNodes);
				bool withinMinDistance = CheckMinDistance(nodeToPlace, takenNodes, minimumDistance);
				// if not within minimum distance, enter if body. If it is, do not enter body
				if (withinMinDistance == false) {
					//if playerSet==false, set the player node faction, add to taken,
					if (playerSet == false) {
						nodeToPlace.GetComponent<Node>().faction = playerFaction;
						takenNodes[0] = nodeToPlace;
						playerSet = true;
						nodeToPlace.GetComponent<Node>().setMatColor();
						Debug.Log("Player Placed! At "+nodeToPlace.name+" with Color: "+playerFaction);
					//else, set the AI node faction, increment AISetCount, add to taken
					} else {
						nodeToPlace.GetComponent<Node>().faction = AI[AISetCount].GetComponent<AIManager>().GetFaction();
						AISetCount++;
						takenNodes[AISetCount] = nodeToPlace;
						nodeToPlace.GetComponent<Node>().setMatColor();
						Debug.Log("AI Placed! At "+nodeToPlace.name+" with Color: "+nodeToPlace.GetComponent<Node>().faction);

					}
					// else, continue loop

				}
			}

			//infinite loop precautions
				if (loopTries == 298) {
					Debug.LogError("ERROR: Unable to find ANY NODES to Place AI or Player. *Could Not Place AI Or Player*. Infinite Loop maxLoopCap reached. Breaking. Details: In the while loop of PlacePlayerAndAI() in GenerateMap.cs");
				}

		}

	}

	void ActivateAI() {
		GameObject[] AIArray = GameObject.FindGameObjectsWithTag("AI");
		for (int i = 0; i < AIArray.Length; i++) {
			AIArray[i].GetComponent<AIManager>().ReScanNodes();
		}
	}

	bool CheckMinDistance(GameObject nodeToPlace, GameObject[] takenNodes, float minimumDistance) {
		bool withinMinDistance = false;
		

		// loop to test all takenNodes compared to nodeToPlace
		for (int i = 0; i < takenNodes.Length; i++) {
			if (takenNodes[i] != null) {
				if ((Vector3.Distance(nodeToPlace.transform.position, takenNodes[i].transform.position) <= minimumDistance)) {
					withinMinDistance = true;
				}
			}
		}
		//Debug.Log("WithinMinDistance= "+withinMinDistance);
		return withinMinDistance;
	}

	public int GetNodeCount() {
		return nodeCount;
	}

	public string[] GetNodeNames() {
		return nodeNames;
	}

	public void PlayerIsDead() {
		playerDead = true;
		Debug.LogWarning("player died");
	}

    public void setPlayerFaction(string faction) {
        this.playerFaction = faction;
    }

	public void setAICount(int AICount) {
        this.AICount = AICount;
    }

// move ALL OF THIS plus PlayerIsDead() to MenuManager
	public void DecrementAICount() {
		AICount--;
		GameObject menuManager = GameObject.Find("MenuManager");

		if ((AICount == 0)&&(playerDead==false)) {
			//Player Wins
			Debug.LogError("Player Wins");
			menuManager.GetComponent<MenuManager>().playerWins();
		}
		if ((AICount == 1||AICount == 0)&&(playerDead==true)) {
			//player loses
			Debug.LogError("Player Loses");
			menuManager.GetComponent<MenuManager>().playerLoses();
		}
	}
	

}
