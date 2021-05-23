using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDelete : MonoBehaviour {

public float nextTime = 1.0f;
void FixedUpdate() {
    if (nextTime < Time.time) {
        //Debug.Log("Do Something");
        
    }
}
}
