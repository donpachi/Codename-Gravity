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

    void Update()
    {
        if (inTextSequence)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).tapCount >= 1)
                {
                    GameControl.Instance.getDialogueHandler().releaseGame();
                    inTextSequence = false;
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D info)
    {
        if (info.gameObject.name == "Player" && !visited)
        {
            inTextSequence = true;
            GameControl.Instance.getDialogueHandler().DisplayText(dialogueIndex);
            visited = true;
        }
    }
}
