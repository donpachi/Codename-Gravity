using UnityEngine;
using System.Collections;

public class DeathMenu : MonoBehaviour {

	private GameObject Controller;
	private GameObject Death;

	void Start()
	{
		Player character = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		Death = GameObject.Find("DeathCanvas");
		Controller = GameObject.Find("ControlCanvas");
		AddListener (character);
	}

	private void AddListener (Player character)
	{
		character.OnPlayerDeath += HandleOnPlayerDeath;
	}

	private void HandleOnPlayerDeath()
	{
		Time.timeScale = 0;
		Death.GetComponent<Canvas>().enabled = true;
		Controller.GetComponent<Canvas>().enabled = false;
	}

	public void Restart()
	{
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
