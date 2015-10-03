using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasLevelSelect : MonoBehaviour {
    GameObject mainMenu;
	// Use this for initialization
	void Start () {
        mainMenu = GameObject.Find("MainMenu");
        this.GetComponent<Canvas>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Back()
    {
        this.GetComponent<Canvas>().enabled = false;
        mainMenu.GetComponent<Canvas>().enabled = true;
    }
}
