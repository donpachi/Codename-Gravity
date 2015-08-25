using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	private GameObject Controller;
	private GameObject Pause;

	public void Continue()
	{
		Controller = GameObject.Find("ControlCanvas");
		Pause = GameObject.Find("PauseCanvas");
		Time.timeScale = 1;
		Pause.GetComponent<Canvas>().enabled = false;
		Controller.GetComponent<Canvas>().enabled = true;
	}

	public void LoadMenu()
	{
		Application.LoadLevel(0);
	}
}
