using UnityEngine;
using System.Collections;

public class CanvasPause : MonoBehaviour {

	private GameObject controller;
    private GameObject death;
    private GameObject victory;

	private void Start()
	{
        controller = GameObject.Find("ControlCanvas");
        death = GameObject.Find("DeathCanvas");
        victory = GameObject.Find("VictoryCanvas");
        this.GetComponent<Canvas>().enabled = false;
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !death.GetComponent<Canvas>().enabled && !victory.GetComponent<Canvas>().enabled)
        {
            Time.timeScale = 0;
            this.GetComponent<Canvas>().enabled = true;
            controller.GetComponent<Canvas>().enabled = false;
        }
    }

	public void Continue()
	{
		Time.timeScale = 1;
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
