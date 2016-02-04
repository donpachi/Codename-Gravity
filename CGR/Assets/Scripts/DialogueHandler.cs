using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/* keep in mind that a single xml file will hold the dialogue data for all levels
*/
public class DialogueHandler : MonoBehaviour {
    private string speech = "test";
    private string currentlevel;    //use this as a formal indexer into the xml structure to grab dialogue
    private DialogueSerializer dSerializer;
    private LevelCollection dialoguecontainer;
    private Level leveldialogue;
    private float savedTimeScale;
    private float typeSpeed;
    private bool resume, drawText;

    // Use this for initialization

    //hitting a collider box for a dialogue trigger will call this function and pass it certain index node
    //should bring up a graphical text box to display the name of the speaker and the text.
    //text will scroll until it hits a period or until the buffer space inside the text window
    //runs out.
    void Start()
    {
        //grab the starting level 
        currentlevel = SceneManager.GetActiveScene().name;
        dialoguecontainer = dSerializer.DeserializeLevelDialogue();
        for (int i = 0; i < dialoguecontainer.Level.Length; i++)
        {
            if (currentlevel == dialoguecontainer.Level[i].levelname)
            {
                leveldialogue = dialoguecontainer.Level[i];
            }
        }
        resume = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentlevel != SceneManager.GetActiveScene().name)
        {
            currentlevel = SceneManager.GetActiveScene().name;
            for (int i = 0; i < dialoguecontainer.Level.Length; i++)
            {
                if (currentlevel == dialoguecontainer.Level[i].levelname)
                {
                    leveldialogue = dialoguecontainer.Level[i];
                }
            }
        }
    }

    void OnGUI()    //GUI elements have to react based on device orientation
    {
        if (resume == false)
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height / 2), speech);
        }

        if (drawText)
        {   //need to draw differently based on orientation of the screen
            //GUI.Label(new Rect()
        }
    }

    public void releaseGame()
    {
        resume = true;
    }

    public void DisplayText(int index)
    {
        pauseGame();
        Dialogues text2display = leveldialogue.Dialogues[index];    //text to display for that node. Each index corresponds to a new speaker (like a conversation)
        DrawTextCanvas();
        //switch the speech function on (onGUI will constantly update so just set a trigger or boolean here, same in the generic display text)
        resumeGame();
    }

    public void DisplayText(string speaker, string msg)
    {
        pauseGame();
        DrawTextCanvas();
        resumeGame();
    }

    private void pauseGame()    //need a way to figure out how to pause (freeze) the game without affecting time.
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
        resume = false;
        while (!resume) { }
        //wait until user taps the screen
    }


}
