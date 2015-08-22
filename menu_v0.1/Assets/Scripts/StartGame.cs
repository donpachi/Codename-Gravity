using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {
	
	public GameObject loadingImage;
	
	public void LoadScene(int level)
	{
		Application.LoadLevel(level);
	}
}