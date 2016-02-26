using UnityEngine;
using System.Collections;

public class TextNode : MonoBehaviour {
    public int dialogueIndex = 0;
    public bool visited;
    private bool inTextSequence;

    void Start()
    {
        inTextSequence = false; visited = false;
    }

    void OnCollisionEnter2D(Collision2D info)
    {
        if (info.gameObject.name == "Player" && !visited)
        {
            inTextSequence = true;
            visited = true;
            GameObject.Find("GameController").GetComponent<DialogueHandler>().DisplayText(dialogueIndex);
        }
    }
}
