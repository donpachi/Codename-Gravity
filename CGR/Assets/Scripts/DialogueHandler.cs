using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DialogueHandler : MonoBehaviour {
    private enum levels {};  //this holds the formal names for the levels to allow for easy indexing into the dialogue level array
    private string currentlevel;    //use this as a formal indexer into the xml structure to grab dialogue
    private DialogueSerializer dSerializer;
    private LevelCollection dialoguedata;
    private float savedTimeScale;
    private float typeSpeed;
	// Use this for initialization
	void Start () {
        //grab the starting level 
        currentlevel = SceneManager.GetActiveScene().name;
        dSerializer = new DialogueSerializer();
        dialoguedata =  dSerializer.DeserializeLevelDialogue();
	}
	
	// Update is called once per frame
	void Update () {
        if (currentlevel != SceneManager.GetActiveScene().name)
        {
            currentlevel = SceneManager.GetActiveScene().name;
        }
	}

    //hitting a collider box for a dialogue trigger will call this function and pass it certain index node
    //should bring up a graphical text box to display the name of the speaker and the text.
    //text will scroll until it hits a period or until the buffer space inside the text window
    //runs out.
    void DisplayNodeText(int index)
    {
        pauseGame();
        DrawTextCanvas();
        resumeGame();
    }

    void DisplayCustomText(string speaker, string msg)
    {
        pauseGame();
        DrawTextCanvas();
        resumeGame();
    }

    private void pauseGame()
    {
        savedTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    private void resumeGame()
    {
        Time.timeScale = savedTimeScale;
    }

    // animate player art (scroll in from the right)
    // animate textbox art (alpha in)
    // create box within dimensions of textbox art
    private void DrawTextCanvas()
    {

    }


}
