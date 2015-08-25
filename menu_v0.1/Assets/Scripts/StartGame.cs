using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {
	
	public GameObject loadingImage;
	
	public void LoadScene(int level)
	{
		Time.timeScale = 1;
		Application.LoadLevel(level);
	}
}