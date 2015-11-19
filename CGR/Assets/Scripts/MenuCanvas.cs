using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuCanvas : MonoBehaviour {

    Player character;
    Selectable continueButton;

	void Start()
	{
		Player character = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		AddListener (character);
        GetButtons();
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !this.GetComponent<Canvas>().enabled)
        {
            Time.timeScale = 0;
            this.GetComponent<Canvas>().enabled = true;
            this.GetComponentInChildren<Text>().text = "Paused";
            this.GetComponentInChildren<Text>().color = Color.yellow;
            continueButton.GetComponentInChildren<Text>().text = "Continue";
            continueButton.GetComponent<Button>().onClick.RemoveAllListeners();
            continueButton.GetComponent<Button>().onClick.AddListener(delegate { Continue(); });
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
                break;
            }
        }
    }

    public void VictoryScreen()
    {
        Time.timeScale = 0;
        this.GetComponent<Canvas>().enabled = true;
        this.GetComponentInChildren<Text>().text = "You Win";
        this.GetComponentInChildren<Text>().color = Color.green;
        GameObject.Find("GameController").GetComponent<GameControl>().SetLevelComplete(100);
        continueButton.GetComponentInChildren<Text>().text = "Next Level";
        continueButton.GetComponent<Button>().onClick.RemoveAllListeners();
        continueButton.GetComponent<Button>().onClick.AddListener(delegate { NextLevel(); });
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        Application.LoadLevel(Application.loadedLevel + 1);
        this.GetComponent<Canvas>().enabled = false;
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
        continueButton.GetComponentInChildren<Text>().enabled = true;
        continueButton.GetComponent<Image>().enabled = true;
        continueButton.GetComponent<Button>().enabled = true;
		Application.LoadLevel(Application.loadedLevel);
	}
	
	public void LoadMenu()
	{
        continueButton.GetComponentInChildren<Text>().enabled = true;
        continueButton.GetComponent<Image>().enabled = true;
        continueButton.GetComponent<Button>().enabled = true;
		Application.LoadLevel(0);
	}
}
