using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RPGText : MonoBehaviour {
    public float typespeed;
    public GameObject interactee;
    private string string2type;
    private float savedTimeScale;

    // Use this for initialization
    void Start () {
        typespeed = 0.2f;
        string2type = "";
	}
	
	// Update is called once per frame
	void Update () {

    }

    void print(string str)
    {
        pauseGame();
        // check orientation
        // then
        // animate player art (scroll in from the right)
        // animate textbox art (alpha in)
        // create box within dimensions of textbox art
        // print out string2type char by char with typespeed intervals
        resumeGame();
    }

    private void pauseGame()
    {
        savedTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    private void resumeGame()
    {
        Time.timeScale = savedTimeScale;
    }
}
