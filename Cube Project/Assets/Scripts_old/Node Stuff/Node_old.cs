// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;

// public class Node_old : MonoBehaviour {

// 	// public string faction;
// 	public int number;

// 	bool selecting = false;

// 	string sourceFaction = "";


// 	//public Vector3 thing;
// 	public float mousex;
// 	public float mousez;

// 	public Vector3 mouse;
// 	public float mousey;

// 	public float screenx;
// 	public float screeny;
// 
// 	//Ray ray;
//   //  RaycastHit hit;

// 	//	Vector3 point = new Vector3();
//       //  Event   currentEvent = Event.current;
//       //  Vector2 mousePos = new Vector2();

// 	// Use this for initialization
// 	void Start () {
// 		faction = "Blue";
// 		setMatColor(faction);
// 		number = 50;
		
// 	}
	
// 	 Input: 1000
// 	max: 1500
// 	want: world position relative to camera
// 	

// 	// Update is called once per frame
// 	void Update () {
// 		screeny = Camera.main.pixelHeight;
// 		screenx = Camera.main.pixelWidth;
// 		mousex = Input.mousePosition.x / screenx;
// 		mousez = Input.mousePosition.y / screeny;
// 		mousey = 3.0f;
// 		
// 		//thing = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
// 		
// 		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
// 		if (Physics.Raycast(ray, out hit)) {
// 			Debug.Log("Hit "+hit.collider.name);

// 		}
// 		//ray = Camera.main.ScreenPointToRay(Camera.main.ScreenToWorldPoint(Input.mousePosition));
// 		//mouse = new Vector3(mousex, mousey, mousez);
// 		//ray = new Ray(mouse, Vector3.down);
// 		//Debug.DrawRay(ray, Vector3.down*50, Color.green, 40.0f);
// 		//Debug.DrawRay(mouse, Vector3.down, Color.green, 40.0f);
// 		//Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition), ray.direction, Color.green, 40.0f);
		


//         // Get the mouse position from Event.
//         // Note that the y position from Event is inverted.
// 		
//         mousePos.x = currentEvent.mousePosition.x;
//         mousePos.y = Camera.main.pixelHeight - currentEvent.mousePosition.y;

//         point = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
// 		Debug.DrawRay(point, Vector3.down, Color.green, 40.0f);
// 	
// 	}

// 	
// 	 //CONTROLLER SECTION
// 	

// 	private void OnMouseDown() {
// 		Debug.Log("OnMouseDown Called");
// 		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
// 		if (Physics.Raycast(ray, out hit)) {
// 			source = hit.collider.gameObject;
// 			Debug.Log(source.ToString()+ "Source");
// 			sourceFaction = source.GetComponent<Node>().faction;
// 			selecting = true;
// 		}
// 	}

// 	private void OnMouseDrag() {
// 		selecting = true;
// 	}

// 	private void OnMouseUp() {
// 		Debug.Log("OnMouseUp Called");
// 		ray = Camera.main.ScreenPointToRay(Input.mousePosition);

// 		if (Physics.Raycast(ray, out hit)) {
// 			target = hit.collider.gameObject;
// 			Debug.Log(target.ToString());
// 			if (target != null && source != null) {
// 				//call spawn function
// 				if (target.GetComponent<Node>().faction == sourceFaction) {
// 					source.GetComponent<Node>().Spawn(sourceFaction, target);
// 					Debug.Log("Spawned");
// 				}
// 			}
// 		} else {
// 			target = null;
// 			source = null;
// 			sourceFaction = "";
// 		}

// 		selecting = false;
// 	}






// 	
// 		//HELPER METHODS
// 	

// 	public void setMatColor(string faction) {
// 		string[] factionArray = {"Red", "Blue", "Neutral", "Green", "Yellow"};
// 		Color[] colorArray = {Color.red, Color.blue, Color.white, Color.green, Color.yellow};

// 		// find color index
// 		int index = System.Array.IndexOf(factionArray, faction);

// 		// assign color to material
// 		Material material = this.transform.Find("Center").gameObject.GetComponent<Renderer>().material;
// 		material.color = colorArray[index];

// 		//Debug.Log(this.transform.Find("Center"));
// 		//Debug.Log(faction);
// 		//Debug.Log(colorArray[index]);

// 	}

// 	public void Spawn(string faction, GameObject target) {
// 		int blockCount = number / 2;
// 		Object block = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Block.prefab", typeof(Block));
		

// 		for (; blockCount > 0; blockCount--) {
// 			Block clone = Instantiate(block, new Vector3(0, 2, 0), Quaternion.identity) as Block;
// 			clone.Initialization(target, faction);
// 		}
// 	}

// 	public void OnTriggerEnter(Collider block) {
		
// 	}

// 	public void SetTarget(GameObject target)
// 	{
// //		this.target = target;
// 	}
// }
