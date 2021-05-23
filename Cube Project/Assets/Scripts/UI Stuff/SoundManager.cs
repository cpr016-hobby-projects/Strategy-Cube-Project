using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {  

    public AudioSource[] audioSources;
    public AudioClip captureNode;
    public float captureNodeVol = 0.5f;
    public AudioClip loseNode;
    public float loseNodeVol = 0.5f;
    public AudioClip startGame;
    public float startGameVol = 0.5f;
    public AudioClip backgroundMusic;
    //public float backgroundMusicVol = 0.3f;

    void Start() {
        audioSources = this.gameObject.GetComponents<AudioSource>();
    }

    public void playCaptureNode() {
      audioSources[0].PlayOneShot(captureNode, captureNodeVol);
    }
    public void playLoseNode() {
      audioSources[0].PlayOneShot(loseNode, loseNodeVol);
    }

    public void playStartGame() {
        audioSources[0].PlayOneShot(startGame, startGameVol);
    }
    IEnumerator playBackgroundMusicThreeSeconds() {
        yield return new WaitForSeconds(3);
        audioSources[1].Play();
        audioSources[1].loop = true;
    }

    public void playBackgroundMusicDelayed() {
        StartCoroutine(playBackgroundMusicThreeSeconds());
    }
}
