using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Threading;

/* keep in mind that a single xml file will hold the dialogue data for all levels
*/
public class DialogueHandler : MonoBehaviour
{
    private string speech;
    private string currentlevel;    //use this as a formal indexer into the xml structure to grab dialogue
    private LevelCollection levelCollection;
    private Level level;
    private DialogueNode dialogueNode;
    private float savedTimeScale;
    private int whichDialogue;
    private int numberOfDialogues;
    private bool enteredArea = false;
    public float typeSpeed;
    private bool resume, drawText;
    private bool doneBlock;
    private int dialogueIndexer;
    private bool inTextSequence;
    private GameObject pCObj, llCObj, lrCObj, ipCObj;
    private Text speakerText, speakerName;
    private int MAXCHAR, MAXNEWLINE, MAXCHARLINE, currentCharCount, currentLineCount, currentCharsPerLine;
    private float waitTimeInterval = 0.3f; 
    private bool doneText;
    AutoResetEvent oSignalEvent = new AutoResetEvent(false);


    public int PORTRAIT_MAXCHAR { get; set; }
    public int PORTRAIT_MAXNEWLINE { get; set; }
    public int PORTRAIT_MAXCHARLINE { get; set; }
    public int LANDSCAPE_MAXCHAR { get; set; }
    public int LANDSCAPE_MAXNEWLINE { get; set; }
    public int LANDSCAPE_MAXCHARLINE { get; set; }

    // Use this for initialization
    private void initBounds()
    {
        PORTRAIT_MAXCHAR = 230;
        PORTRAIT_MAXNEWLINE = 7;
        PORTRAIT_MAXCHARLINE = 33;
        LANDSCAPE_MAXCHAR = 230;
        LANDSCAPE_MAXNEWLINE = 5;
        LANDSCAPE_MAXCHARLINE = 3;
        currentCharCount = 0; currentLineCount = 0; currentCharsPerLine = 0;
    }
    
    
    void Start()
    {
        initBounds();
        resume = true;
        typeSpeed = 0.01f;
        inTextSequence = false;
        dialogueIndexer = 0;
    }

    void Awake()
    {
        //grab the starting level's dialogue
        currentlevel = SceneManager.GetActiveScene().name;
        levelCollection = DialogueSerializer.DeserializeLevelDialogue();
        //Debug.Log("Number of levels: " + levelCollection.levels.Count);
        foreach (Level lvl in levelCollection.levels)
        {
            if (currentlevel == lvl.levelname)
            {
                level = lvl;
            }
        }
        initCanvasObjects();
        hideCanvas();
    }

    private void initCanvasObjects()
    {
        foreach (var obj in GetComponentsInChildren<Canvas>())
        {
            switch (obj.gameObject.name)
            {
                case "Portrait":
                    pCObj = obj.gameObject;
                    break;
                case "InvPortrait":
                    ipCObj = obj.gameObject;
                    break;
                case "LSLeft":
                    llCObj = obj.gameObject;
                    break;
                case "LSRight":
                    lrCObj = obj.gameObject;
                    break;
            }
        }
    }

    private void setLocalBounds()
    {
        if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT ||
            OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT){
            MAXCHAR = PORTRAIT_MAXCHAR; MAXNEWLINE = PORTRAIT_MAXNEWLINE; MAXCHARLINE = PORTRAIT_MAXCHARLINE;
        }
        else
            MAXCHAR = LANDSCAPE_MAXCHAR; MAXNEWLINE = LANDSCAPE_MAXNEWLINE; MAXCHARLINE = LANDSCAPE_MAXCHARLINE;
    }

    private void hideCanvas()
    {
        pCObj.GetComponent<Canvas>().enabled = false;
        ipCObj.GetComponent<Canvas>().enabled = false;
        llCObj.GetComponent<Canvas>().enabled = false;
        lrCObj.GetComponent<Canvas>().enabled = false;
    }

    private void clearCanvasText(GameObject canvasObject)
    {
        foreach (var textBox in canvasObject.GetComponentsInChildren<Text>())
        {
            textBox.text = "";
        }
    }

    public void clearAllCanvasText()
    {
        foreach (var canvas in this.GetComponentsInChildren<Canvas>())
        {
            foreach (var textBox in canvas.GetComponentsInChildren<Text>())
            {
                textBox.text = "";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        setLocalBounds();
        
        if (currentlevel != SceneManager.GetActiveScene().name)
        {
            currentlevel = SceneManager.GetActiveScene().name;
            for (int i = 0; i < levelCollection.levels.Count; i++)
                if (currentlevel == levelCollection.levels[i].levelname)
                    level = levelCollection.levels[i];
        }
    }

    public void initiateDialogue(int dNodeIndex)
    {
        OnEnable();
        //pauseGame();
        DrawTextCanvas();       
        dialogueNode = level.levelDialogueNodes[dNodeIndex];
        numberOfDialogues = dialogueNode.dialogues.Count;
        //foreach (var dialogue in dNode.dialogues)
        //{
            //speakerName.text = dialogue.speaker;
            //StartCoroutine(animateText(dialogue.speech));
            displayText(dialogueNode);
        //}
        //resumeGame();
        OnDisable();
    }

    private void setActiveText(GameObject canvasObj)
    {
        speakerName = canvasObj.transform.Find("Name").GetComponent<Text>();
        speakerText = canvasObj.transform.Find("Dialogue").GetComponent<Text>();
        speakerName.color = Color.red;
        speakerText.color = Color.red;
    }

    /// <summary>
    /// Helper function to find the dialogue canvas to right on based on orientation
    /// </summary>
    private void align2Orientation()
    {
        hideCanvas();
        switch (OrientationListener.instanceOf.currentOrientation())
        {
            case (OrientationListener.Orientation.PORTRAIT):
                clearCanvasText(pCObj);
                pCObj.GetComponent<Canvas>().enabled = true;
                setActiveText(pCObj);
                break;
            case (OrientationListener.Orientation.INVERTED_PORTRAIT):
                clearCanvasText(ipCObj);
                ipCObj.GetComponent<Canvas>().enabled = true;
                setActiveText(ipCObj);
                break;
            case (OrientationListener.Orientation.LANDSCAPE_LEFT):
                clearCanvasText(llCObj);
                llCObj.GetComponent<Canvas>().enabled = true;
                setActiveText(llCObj);
                break;
            case (OrientationListener.Orientation.LANDSCAPE_RIGHT):
                clearCanvasText(lrCObj);
                lrCObj.GetComponent<Canvas>().enabled = true;
                setActiveText(lrCObj);
                break;
            default:
                clearCanvasText(llCObj);
                llCObj.GetComponent<Canvas>().enabled = true;
                setActiveText(llCObj);
                break;
        }
    }

    /// <summary>
    /// Convenience method that allows explicit calls to create a text sequence at any point 
    /// in the game (exlcuding dialogue nodes already in the scene)
    /// </summary>
    /// <param name="speaker"> Speaker's name to display</param>
    /// <param name="msg">Speaker's text to display</param>
    public void displayText(DialogueNode dNode)
    {
        
        // THIS STOPS WAITFORSECONDS AS WELL
        pauseGame();
        
        DrawTextCanvas();
        typeSpeed = 0.01f;
        StartCoroutine(animateText(dNode));
        //animateText(msg);
        //oSignalEvent.WaitOne();
            //if (doneText) <--This only runs once! Therefore doneText will always be false. Instead, create a property for doneText so that you can define what happens when it changes
            //{
            //    resumeGame();
            //}
    }

    private void pauseGame()   
    {
        Debug.Log("Pausing game");
        if (Time.timeScale != 0)
            savedTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    private void resumeGame()
    {
        Debug.Log("Resuming game");
        Time.timeScale = savedTimeScale;
    }


    private void DrawTextCanvas()
    {
        // animate player art (scroll in from the right)
        // animate textbox art (alpha in)
        // create box within dimensions of textbox art
        //....
        align2Orientation();
    }

    /// <summary>
    /// coroutine method that will display text in the screen in a constrained box by modifying the string variable passed into the GUI element.
    /// the speed at which text is displayed is controlled by the typeSpeed property.
    /// </summary>
    /// <param name="fullstring">full length string that will be displayed on the screen.</param>
    /// <returns>Iterable object that the coroutine will handle</returns>
    IEnumerator animateText(DialogueNode dNode)
    {
        string speaker = dNode.dialogues[whichDialogue].speaker;
        string fullstring = dNode.dialogues[whichDialogue].speech;
        speakerName.text = speaker;
        //if we want to do handling for letter overflow, we need to split the string into words and then check the next word to see if enough space on line
        //ABOVE NOT IMPLEMENTED
        doneTextFlag = false;
        for (int i = 0; i < fullstring.Length; i++)
        {
            if (currentCharCount >= MAXCHAR || currentLineCount >= MAXNEWLINE)
            {
                //need to puase execution of this here and wait for a tap
                //waitingfortap = true;
                //while (waitingfortap)
                //{
                //    yield return new WaitForSeconds(waitTimeInterval);
                //}
                currentCharCount = 0;
                currentLineCount = 0;
                currentCharsPerLine = 0;
                speakerText.text = "\n";
            }
            else if (fullstring[i] == '\n')
            {
                speakerText.text += "\n";
                currentLineCount++;
                currentCharsPerLine = 0;
            }
            else if (currentCharsPerLine == MAXCHARLINE-1 && fullstring[i] != '\n')
            {
                speakerText.text += "-\n";
                currentLineCount++;
            }
            else
            {
                speakerText.text += fullstring[i];
                currentCharCount++;
            }
            float start = Time.realtimeSinceStartup;         
            while (Time.realtimeSinceStartup < start + typeSpeed)       
                yield return null;
        }
        
        doneBlock = true;
    }

    void screenTapped(TouchInstanceData data)
    {
        if (doneBlock == true && enteredArea == true)
        {
            // If screen is tapped and all dialogues are finished, empty the text
            whichDialogue++;
            if (whichDialogue >= numberOfDialogues)
            {
                doneTextFlag = true;
                speakerText.text = "";
                speakerName.text = "";
                doneBlock = false;
                whichDialogue = 0;
                return;
            }
            displayText(dialogueNode);
            doneBlock = false;
        }
        else if (enteredArea == true)
        {
            typeSpeed = 0;
        }
        if (enteredArea == false)
            enteredArea = true;
    }

    void OnEnable()
    {
        TouchController.ScreenReleased += screenTapped;
    }

    void OnDisable()
    {
        TouchController.ScreenReleased -= screenTapped;
    }

    public bool doneTextFlag
    {
        get { return doneText; }
        set
        {
            doneText = value;
            if (doneText)
                resumeGame();
        }
    }
}
