using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{

    void Awake()
    {
        if (StaticVariables.getCanvasBool() == true) {
            Destroy(this.gameObject);
            StaticVariables.setCanvasBool(false);
        }
    }
}
