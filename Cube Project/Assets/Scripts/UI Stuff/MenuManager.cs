using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    private string playerFaction = "Neutral";
    private int AICount = 0;
    private GameObject winOrLose;
    private GameObject canvas;
    //private int rand;

    // Start is called before the first frame update
    void Awake()
    {
        if (StaticVariables.getMenuBool() == true) {
            Destroy(this.gameObject);
        } else {
            Init();
        }
    }
    private void Init() {
        winOrLose = GameObject.FindWithTag("WinOrLose");
        canvas = winOrLose.transform.parent.gameObject;
        DontDestroyOnLoad(canvas);
        canvas.SetActive(false);

       // StaticVariables.setCanvasBool(true);
        StaticVariables.setMenuBool(true);

        // Debug.Log("Canvas "+StaticVariables.getCanvasBool());
        // Debug.Log("Menu "+StaticVariables.getMenuBool());
        // rand = Random.Range(0, 10);
        // Debug.Log(rand);
        
        DontDestroyOnLoad(this.gameObject);
    }

    public void loadGame () {
        SceneManager.LoadScene("Main Scene", LoadSceneMode.Single);
        this.gameObject.GetComponent<SoundManager>().playStartGame();
        this.gameObject.GetComponent<SoundManager>().playBackgroundMusicDelayed();
        canvas.SetActive(false);
    }

    public void setPlayerFaction (string faction) {
        this.playerFaction = faction;
    }

    public void setAICount (int AICount) {
        this.AICount = AICount;
    }

    public string getPlayerFaction() {
        return playerFaction;
    }

    public int getAICount() {
        return AICount;
    }

    public void playerWins() {
        Debug.Log(StaticVariables.getCanvasBool());
        Debug.Log(canvas);
        resetGame();
        Debug.Log(canvas);
        //also stop audio loop here.

        // GameObject[] canvasArray = GameObject.FindGameObjectsWithTag("Canvas");
        // Debug.Log(canvasArray);
        // for (int i = 0; i < canvasArray.Length; i++) {
        //     if (canvas != canvasArray[i]) {
        //         Destroy(canvasArray[i]);
        //     }
        // }
        winOrLose.transform.Find("Text").gameObject.GetComponent<Text>().text = "Player Wins!";
    }
    public void playerLoses() {
        Debug.Log(StaticVariables.getCanvasBool());
        Debug.Log(canvas);
        resetGame();
        Debug.Log(canvas);
        // GameObject[] canvasArray = GameObject.FindGameObjectsWithTag("Canvas");
        // for (int i = 0; i < canvasArray.Length; i++) {
        //     if (canvas != canvasArray[i]) {
        //         Destroy(canvasArray[i]);
        //     }
        // }
        winOrLose.transform.Find("Text").gameObject.GetComponent<Text>().text = "Player Lost";

        
    }

    public void resetGame() {
        canvas.SetActive(true);
        this.gameObject.GetComponent<SoundManager>().audioSources[1].Stop();
        Debug.Log(StaticVariables.getCanvasBool());
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        StaticVariables.setCanvasBool(true);
    }

}
