using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/* keep in mind that a single xml file will hold the dialogue data for all levels
*/
public class DialogueHandler : MonoBehaviour {
    private string speech;
    private string currentlevel;    //use this as a formal indexer into the xml structure to grab dialogue
    private LevelCollection dialoguecontainer;
    private Level leveldialogue;
    private Dialogues dialogueGroup;
    private float savedTimeScale;
    private float typeSpeed;
    private bool resume, drawText;
    private Rect textboxConstraint;
    private bool waitingfortap;
    private int charcount;
    private int newlinecount;
    private int dialogueIndexer;
    private bool inTextSequence;

    public int MAXCHARCOUNT { get; private set; }
    public int MAXROWS { get; private set; }

    // Use this for initialization

    //hitting a collider box for a dialogue trigger will call this function and pass it certain index node
    //should bring up a graphical text box to display the name of the speaker and the text.
    //text will scroll until it hits a period or until the buffer space inside the text window
    //runs out.
    void Start()
    {
        //grab the starting level 
        currentlevel = SceneManager.GetActiveScene().name;
        dialoguecontainer = DialogueSerializer.DeserializeLevelDialogue();
        for (int i = 0; i < dialoguecontainer.Level.Length; i++)
        {
            if (currentlevel == dialoguecontainer.Level[i].levelname)
            {
                leveldialogue = dialoguecontainer.Level[i];
            }
        }
        eventEnable();
        resume = true;
        typeSpeed = 0.1f;
        newlinecount = 0;
        charcount = 0;
        MAXCHARCOUNT = 150;
        MAXROWS = 3;
        inTextSequence = false;
        dialogueIndexer = 0;
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
        if (inTextSequence)
        {
            string string2display = dialogueGroup.Dialogue[dialogueIndexer].speech;
            StartCoroutine(animateText(string2display));
        }
    }

    private void setOrientationAlignment()
    {
        switch (OrientationListener.instanceOf.currentOrientation())
        {
            case (OrientationListener.Orientation.PORTRAIT):
                textboxConstraint = new Rect(0, Screen.height * (0.75f), Screen.width, Screen.height / 4);
                break;
            case (OrientationListener.Orientation.INVERTED_PORTRAIT):
                textboxConstraint = new Rect(0, 0, Screen.width, Screen.height / 4);
                break;
            case (OrientationListener.Orientation.LANDSCAPE_LEFT):
                textboxConstraint = new Rect(Screen.width / 2, 0, Screen.width / 2, Screen.height);
                break;
            case (OrientationListener.Orientation.LANDSCAPE_RIGHT):
                textboxConstraint = new Rect(0, 0, Screen.width / 2, Screen.height);
                break;
            default:
                textboxConstraint = new Rect(0, 0, Screen.width / 2, Screen.height);
                break;
        }
    }

    void OnGUI()    //GUI elements have to react based on device orientation
    {
        if (resume == false)
        {
            GUI.Box(textboxConstraint, speech);   //change this to drawTexture when implementing the drawn chatbox
            if (drawText)
            {   
                //GUI.Label(new Rect()

            }

        }
    }
      
    /// <summary>
    /// stops the text gui from displaying
    /// </summary>
    public void hideTextGUI()
    {
        resume = true;
    }

    public void DisplayText(int index)
    {
        pauseGame();
        inTextSequence = true;
        dialogueGroup = leveldialogue.Dialogues[index];    //text to display for that node. Each index corresponds to a new speaker (like a conversation)
        DrawTextCanvas();
        //switch the speech function on (onGUI will constantly update so just set a trigger or boolean here, same in the generic display text)
        resumeGame();
    }

    public void DisplayText(string speaker, string msg)
    {
        pauseGame();
        inTextSequence = true;
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
        setOrientationAlignment();
        while (!resume) { }
        //wait until user taps the screen
    }

    // this method is assuming that unity will reference to the string variable in the parameter and not make its own copy of the string before displaying it
    // if this isnt the case, secondary method is to print the whole string but to change the text alpha char by char.
    /// <summary>
    /// coroutine method that will display text in the screen in a constrained box by modifying the string variable passed into the GUI element.
    /// the speed at which text is displayed is controlled by the typeSpeed property.
    /// </summary>
    /// <param name="fullstring">full length string that will be displayed on the screen.</param>
    /// <returns>Iterable object that the coroutine will handle</returns>
    IEnumerator animateText(string fullstring)
    {
        speech = "";
        for (int i = 0; i > fullstring.Length; i++)
        {
            if (charcount <= MAXCHARCOUNT || newlinecount <= MAXROWS)
            {
                waitingfortap = true;
                while (waitingfortap)
                {
                    yield return new WaitForSeconds(0.3f);//somehow stop execution here to wait for event from touch controller.. yield?
                }
                waitingfortap = true; //at this point, the program has recieved the tap and is executing normally. Reset the flag.
                charcount = 0;
                newlinecount = 0;
                speech = "";
            }
            else if (fullstring[i] == '\n')
                newlinecount++;
            else {
                speech += fullstring[i];
                charcount++;
                yield return new WaitForSeconds(typeSpeed);
            }
        }
    }

    void screenTapped(TouchInstanceData data)
    {
        waitingfortap = false;
    }

    void eventEnable()
    {
        TouchController.ScreenTouched += screenTapped;
    }

    void eventDisable()
    {
        TouchController.ScreenTouched -= screenTapped;
    }
}
