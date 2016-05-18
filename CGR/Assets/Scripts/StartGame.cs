using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {
	
	private GameObject levelSelectCanvas;
    private Selectable[] menuButtons;

    void Start()
    {
        levelSelectCanvas = GameObject.Find("LevelSelect");
        menuButtons = this.GetComponentsInChildren<Selectable>(); //Collects buttons in-order from editor hierarchy
    }

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}
	
	public void LoadScene()
	{
        int latestLevel = GameControl.Instance.GetLatestLevel();
        Debug.Log(latestLevel);
        SceneManager.LoadScene(latestLevel);
	}

    public void LevelSelect()
    {
        this.GetComponent<Canvas>().enabled = false;
        levelSelectCanvas.GetComponent<Canvas>().enabled = true;
    }

    public void LoadSettings()
    {
        this.GetComponentInChildren<Text>().text = "Settings";

        menuButtons[0].GetComponentInChildren<Text>().enabled = false;       //0 is the start game button
        menuButtons[0].GetComponent<Image>().enabled = false;
        menuButtons[0].GetComponent<Button>().enabled = false;

        menuButtons[1].GetComponentInChildren<Text>().enabled = false;       //1 is the level select button
        menuButtons[1].GetComponent<Image>().enabled = false;
        menuButtons[1].GetComponent<Button>().enabled = false;

        menuButtons[2].GetComponentInChildren<Text>().enabled = false;       //2 is the settings button
        menuButtons[2].GetComponent<Image>().enabled = false;
        menuButtons[2].GetComponent<Button>().enabled = false;

        menuButtons[3].GetComponentInChildren<Text>().enabled = true;       //3 is the clear save data button
        menuButtons[3].GetComponent<Image>().enabled = true;
        menuButtons[3].GetComponent<Button>().enabled = true;

        menuButtons[4].GetComponentInChildren<Text>().enabled = true;       //4 is the back button
        menuButtons[4].GetComponent<Image>().enabled = true;
        menuButtons[4].GetComponent<Button>().enabled = true;
    }

    public void ClearSaveData()
    {
        GameControl.Instance.NewGame();
        Back();
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}