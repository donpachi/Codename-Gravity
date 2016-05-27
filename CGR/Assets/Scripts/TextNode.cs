using UnityEngine;
using System.Collections;

public class TextNode : MonoBehaviour {
    public int dialogueIndex = 0;
    public bool visited;
    public bool repeatable;

    void Start()
    {
        visited = false;
    }

    void OnTriggerEnter2D(Collider2D info)
    {
        if (info.gameObject.name == "Player" && (!visited || repeatable))
        {
            visited = true;
            GameObject.Find("DialogueCanvas").GetComponent<DialogueHandler>().initiateDialogue(dialogueIndex);
        }
    }
}
