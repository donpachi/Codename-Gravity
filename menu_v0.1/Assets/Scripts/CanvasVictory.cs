using UnityEngine;
using System.Collections;

public class CanvasVictory : MonoBehaviour {

    private GameObject controller;

	// Use this for initialization
	void Start () {
        this.GetComponent<Canvas>().enabled = false;
        controller = GameObject.Find("ControlCanvas");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void NextLevel()
    {
        Time.timeScale = 1;
        Application.LoadLevel(Application.loadedLevel + 1);
        this.GetComponent<Canvas>().enabled = false;
        controller.GetComponent<Canvas>().enabled = true;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        this.GetComponent<Canvas>().enabled = false;
        controller.GetComponent<Canvas>().enabled = true;
        Application.LoadLevel(Application.loadedLevel);
    }

    public void LoadMenu()
    {
        Application.LoadLevel(0);
    }
}
