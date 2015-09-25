using UnityEngine;
using System.Collections;

public class CanvasDeath : MonoBehaviour {

	private GameObject Controller;

	void Start()
	{
		Player character = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        this.GetComponent<Canvas>().enabled = false;
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
		this.GetComponent<Canvas>().enabled = true;
		Controller.GetComponent<Canvas>().enabled = false;
	}

	public void Restart()
	{
		Time.timeScale = 1;
		this.GetComponent<Canvas>().enabled = false;
		Controller.GetComponent<Canvas>().enabled = true;
		Application.LoadLevel(Application.loadedLevel);
	}
	
	public void LoadMenu()
	{
		Application.LoadLevel(0);
	}
}
