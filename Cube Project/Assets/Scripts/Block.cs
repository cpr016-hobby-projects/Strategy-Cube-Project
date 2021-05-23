using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	public string faction = "";
	public GameObject destination = null;
	public float speed = 1f;

	public void Initialization(GameObject destination, string faction) {
		this.destination = destination;
		this.faction = faction;
		//Debug.Log("speed = " + speed);
	}
	
	// Update is called once per frame
	void Update () {
		float step = speed * Time.deltaTime;

		transform.position = Vector3.MoveTowards(transform.position, destination.transform.position, step);
	}

	public float GetBlockSpeed() {
		return speed * Time.deltaTime;
	}
}
