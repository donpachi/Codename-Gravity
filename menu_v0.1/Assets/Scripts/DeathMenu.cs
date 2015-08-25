using UnityEngine;
using System.Collections;

public class DeathMenu : MonoBehaviour {

	private GameObject Death;
	private GameObject Controller;

	public void Restart()
	{
		Death = GameObject.Find("DeathCanvas");
		Controller = GameObject.Find("ControlCanvas");
		Death.GetComponent<Canvas>().enabled = false;
		Controller.GetComponent<Canvas>().enabled = true;
		Time.timeScale = 1;
		Application.LoadLevel(Application.loadedLevel);
	}
	
	public void LoadMenu()
	{
		Application.LoadLevel(0);
	}
}
