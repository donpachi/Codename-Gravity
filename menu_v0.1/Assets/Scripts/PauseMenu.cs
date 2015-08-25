using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	private GameObject Controller;
	private GameObject Pause;

	public void Continue()
	{
		Controller = GameObject.Find("ControlCanvas");
		Pause = GameObject.Find("Pause");
		Time.timeScale = 1;
		Pause.SetActive(false);
		Controller.SetActive (true);
	}

	public void LoadMenu()
	{
		Application.LoadLevel(0);
	}
}
