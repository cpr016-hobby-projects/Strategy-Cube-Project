using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public void CustomOnClick() {
        GameObject.Find("MenuManager").GetComponent<MenuManager>().loadGame();
    }
}
