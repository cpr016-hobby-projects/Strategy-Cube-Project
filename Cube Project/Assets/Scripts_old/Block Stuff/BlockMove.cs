using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMove : MonoBehaviour {

	// The target
	public Transform target;

	// The speed of this object
	public float speed = 1f;

	// Use this for initialization
	void Start () {
		GameObject node = GameObject.FindWithTag("Node1");
		target = node.transform;//Transform.position(cube);
		//Debug.Log("speed = " + speed);
	}
	
	// Update is called once per frame
	void Update () {

		float step = speed * Time.deltaTime;

		transform.position = Vector3.MoveTowards(transform.position, target.position, step);

	}
}
