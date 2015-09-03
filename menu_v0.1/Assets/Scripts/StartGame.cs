using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {
	
	public GameObject loadingImage;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}
	
	public void LoadScene(int level)
	{
		Time.timeScale = 1;
		Application.LoadLevel(level);
	}
}