using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script defined the "collector" properties of the node
public class Collector : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// This is used to detect when a cube enters the node
	void OnTriggerEnter(Collider cube) {
		Destroy(cube.gameObject);
		//Debug.Log("Entered+Destroyed");

	}
	// Update is called once per frame
	void Update () {
		
	}
}
