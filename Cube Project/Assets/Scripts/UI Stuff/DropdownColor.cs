//Create a new Dropdown GameObject by going to the Hierarchy and clicking Create>UI>Dropdown. Attach this script to the Dropdown GameObject.
//Set your own Text in the Inspector window

using UnityEngine;
using UnityEngine.UI;

public class DropdownColor : MonoBehaviour
{
    Dropdown m_Dropdown;

    void Start()
    {
        //Fetch the Dropdown GameObject
        m_Dropdown = GetComponent<Dropdown>();
        //Add listener for when the value of the Dropdown changes, to take action
        m_Dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged();
        });

        //Initialise the Text to say the first value of the Dropdown
       // m_Text.text = "First Value : " + m_Dropdown.value;
    }

    //Ouput the new value of the Dropdown into Text
    void DropdownValueChanged()
    {
      //  m_Text.text =  "New Value : " + change.value;
      Debug.Log("Player color changed");
      if (this.transform.Find("Label").gameObject.GetComponent<Text>().text != "None") {
        GameObject.FindWithTag("MenuManager").GetComponent<MenuManager>().setPlayerFaction(this.transform.Find("Label").gameObject.GetComponent<Text>().text);
      } else {
        Debug.LogWarning("Player Faction not selected!");
        GameObject.FindWithTag("MenuManager").GetComponent<MenuManager>().setPlayerFaction("Neutral");
      }
    }
}