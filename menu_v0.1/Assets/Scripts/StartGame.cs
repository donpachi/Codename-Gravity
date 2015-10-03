using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {
	
	public GameObject levelSelectCanvas;

    void Start()
    {
        levelSelectCanvas = GameObject.Find("LevelSelect");
    }

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

    public void LevelSelect()
    {
        this.GetComponent<Canvas>().enabled = false;
        levelSelectCanvas.GetComponent<Canvas>().enabled = true; 
    }
}