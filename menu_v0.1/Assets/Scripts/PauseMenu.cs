using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	private GameObject Controller;
	private GameObject Pause;

	private void Start()
	{
		Controller = GameObject.Find("ControlCanvas");
		Pause = GameObject.Find("PauseCanvas");
	}

	public void Continue()
	{
		Time.timeScale = 1;
		Pause.GetComponent<Canvas>().enabled = false;
		Controller.GetComponent<Canvas>().enabled = true;
	}

	public void Restart()
	{
		Time.timeScale = 1;
		Pause.GetComponent<Canvas>().enabled = false;
		Controller.GetComponent<Canvas>().enabled = true;
		Application.LoadLevel(Application.loadedLevel);
	}

	public void LoadMenu()
	{
		Application.LoadLevel(0);
	}
}
