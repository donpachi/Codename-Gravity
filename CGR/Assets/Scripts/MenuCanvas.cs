using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuCanvas : MonoBehaviour {

    Player character;
    Selectable continueButton;

	void Start()
	{
		Player character = GameObject.Find("Player").GetComponent<Player>();
		AddListener (character);
        GetButtons();
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !this.GetComponent<Canvas>().enabled)
        {
            pauseScreen();
        }

    }

	private void AddListener (Player character)
	{
		character.OnPlayerDeath += HandleOnPlayerDeath;
	}

	private void HandleOnPlayerDeath()
	{
		Time.timeScale = 0;
		this.GetComponent<Canvas>().enabled = true;
        this.GetComponentInChildren<Text>().text = "You Died";
        this.GetComponentInChildren<Text>().color = Color.red;
        continueButton.GetComponentInChildren<Text>().enabled = false;
        continueButton.GetComponent<Image>().enabled = false;
        continueButton.GetComponent<Button>().enabled = false;
	}

    private void GetButtons()
    {
        Selectable[] menuButtons = this.GetComponentsInChildren<Selectable>();
        for (int i = 0; i < menuButtons.Length; i++)
        {
            if (menuButtons[i].name == "Continue")
            {
                continueButton = menuButtons[i];
            }
        }
    }

    private void pauseScreen()
    {
        Time.timeScale = 0;
        this.GetComponent<Canvas>().enabled = true;
        this.GetComponentInChildren<Text>().text = "Paused";
        this.GetComponentInChildren<Text>().color = Color.yellow;
        continueButton.GetComponentInChildren<Text>().enabled = true;
        continueButton.GetComponent<Image>().enabled = true;
        continueButton.GetComponent<Button>().enabled = true;
        continueButton.GetComponentInChildren<Text>().text = "Continue";
        continueButton.GetComponent<Button>().onClick.RemoveAllListeners();
        continueButton.GetComponent<Button>().onClick.AddListener(delegate { Continue(); });
    }

    public void VictoryScreen()
    {
        Time.timeScale = 0;
        this.GetComponent<Canvas>().enabled = true;
        this.GetComponentInChildren<Text>().text = "You Win";
        this.GetComponentInChildren<Text>().color = Color.green;
        GameControl.Instance.SetLevelComplete(100);
        continueButton.GetComponentInChildren<Text>().enabled = true;
        continueButton.GetComponent<Image>().enabled = true;
        continueButton.GetComponent<Button>().enabled = true;
        continueButton.GetComponentInChildren<Text>().text = "Next Level";
        continueButton.GetComponent<Button>().onClick.RemoveAllListeners();
        continueButton.GetComponent<Button>().onClick.AddListener(delegate { NextLevel(); });
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCount - 1)
            SceneManager.LoadScene(0);
        else
        {
            LevelManager.Instance.GotoNextLevel(SceneManager.GetActiveScene().buildIndex + 1);
            this.GetComponent<Canvas>().enabled = false;
        }
        Time.timeScale = 1;
        
        //Application.LoadLevel(Application.loadedLevel + 1);
    }

    public void Continue()
    {
        Time.timeScale = 1;
        this.GetComponent<Canvas>().enabled = false;
    }

	public void Restart()
	{
		Time.timeScale = 1;
		this.GetComponent<Canvas>().enabled = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

    public void RestartFromCheckpoint()
    {
        LevelManager.Instance.ReloadFromCheckpoint();
        Time.timeScale = 1;
        this.GetComponent<Canvas>().enabled = false;

    }
	
	public void LoadMenu()
	{
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
		//Application.LoadLevel(0);
	}
}
