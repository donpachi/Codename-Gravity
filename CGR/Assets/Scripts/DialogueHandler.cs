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
    private float typeSpeed;
    private bool resume, drawText;
    private bool waitingfortap;
    private int dialogueIndexer;
    private bool inTextSequence;
    private GameObject pCObj, llCObj, lrCObj, ipCObj;
    private Text speakerText, speakerName;
    private int MAXCHAR, MAXNEWLINE, MAXCHARLINE, currentCharCount, currentLineCount, currentCharsPerLine;
    private float waitTimeInterval = 0.3f; 
    private bool doneText;

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
        for (int i = 0; i < levelCollection.levelCollection.Length; i++)
        {
            if (currentlevel == levelCollection.levelCollection[i].levelname)
            {
                level = levelCollection.levelCollection[i];
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
        if (currentlevel != SceneManager.GetActiveScene().name)
        {
            currentlevel = SceneManager.GetActiveScene().name;
            for (int i = 0; i < levelCollection.levelCollection.Length; i++)
            {
                if (currentlevel == levelCollection.levelCollection[i].levelname)
                {
                    level = levelCollection.levelCollection[i];
                }
            }
        }
    }

    public void initiateDialogue(int dNodeIndex)
    {
        pauseGame();
        DrawTextCanvas();
        eventEnable();
        DialogueNode dNode = level.levelDialogueNodes[dNodeIndex];
        foreach (var dialogue in dNode.dialogueArray)
        {
            speakerName.text = dialogue.speaker;
            StartCoroutine(animateText(dialogue.speech));
        }
        resumeGame();
        eventDisable();
    }

    private void setActiveText(GameObject canvasObj)
    {
        speakerName = canvasObj.transform.Find("Name").GetComponent<Text>();
        speakerText = canvasObj.transform.Find("Dialogue").GetComponent<Text>();
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
    public void displayText(string speaker, string msg)
    {
        pauseGame();
        DrawTextCanvas();
        speakerName.text = speaker;
        StartCoroutine(animateText(msg));
        if (doneText)
        {
            resumeGame();
        }
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
    IEnumerator animateText(string fullstring)
    {
        //if we want to do handling for letter overflow, we need to split the string into words and then check the next word to see if enough space on line
        //ABOVE NOT IMPLEMENTED
        doneText = false;
        for (int i = 0; i > fullstring.Length; i++)
        {

            if (currentCharCount >= MAXCHAR || currentLineCount >= MAXNEWLINE)
            {
                //need to puase execution of this here and wait for a tap
                waitingfortap = true;
                while (waitingfortap)
                {
                    yield return new WaitForSeconds(waitTimeInterval);
                }
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
            yield return new WaitForSeconds(typeSpeed);
        }
        doneText = true;

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
