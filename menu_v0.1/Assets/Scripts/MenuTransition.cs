using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System;

public class MenuTransition : MonoBehaviour
{

    private bool released;
    private RectTransform contentViewer;
    private RectTransform closestScreenRect;
    private GameObject[] levelSelectScreens;

    private Sprite buttonImage;

    // Use this for initialization
    void Start () {
        contentViewer = this.GetComponent<RectTransform>();
        released = false;
        levelSelectScreens = GameObject.FindGameObjectsWithTag("Level Select");
        Array.Reverse(levelSelectScreens);
        buttonImage = Resources.Load<Sprite>("unity_builtin_extra/UISprite");   //change to better image
        GenerateLevelList();
    }
	
	// Update is called once per frame
	void Update () {
        if (released)
        {
            contentViewer.offsetMax = new Vector2 (Mathf.Lerp(contentViewer.offsetMax.magnitude,
                                                               closestScreenRect.offsetMax.magnitude, 
                                                               30.0f * Time.deltaTime), 0f) * -1;
            contentViewer.offsetMin = new Vector2 (Mathf.Lerp(contentViewer.offsetMin.magnitude,
                                                               closestScreenRect.offsetMax.magnitude, 
                                                               30.0f * Time.deltaTime), 0f) * -1;
        }

        if (released && Vector2.Distance(contentViewer.offsetMax, closestScreenRect.offsetMax * -1) < 0.005f)
        {
            released = false;
            this.GetComponent<RectTransform>().offsetMax = closestScreenRect.offsetMax * -1; //center position: MAY BE REDUNDANT
            this.GetComponent<RectTransform>().offsetMin = closestScreenRect.offsetMax * -1;
        }
	}

    public void EndDrag()
    {
        float distance = float.MaxValue;
        float tempDistance;
        for (int i = 0; i < levelSelectScreens.Length; i++)
        {
            tempDistance = Vector2.Distance(contentViewer.offsetMax, levelSelectScreens[i].GetComponent<RectTransform>().offsetMax * -1);

            if (distance > tempDistance)
            {
                distance = tempDistance;
                closestScreenRect = levelSelectScreens[i].GetComponent<RectTransform>();
            }
        }
        released = true;
    }

    public void BeginDrag()
    {
        released = false;
    }

    public void LoadLevel(int level)
    {
        Application.LoadLevel(level);
    }

    private void GenerateLevelList()
    {
        bool[] levelUnlocked = GameObject.Find("GameController").GetComponent<GameControl>().GetLevelUnlock();
        int[] levelHighScore = GameObject.Find("GameController").GetComponent<GameControl>().GetLevelHighScore();

        ColorBlock color;
        Selectable[] levelButtons;

        for (int i = 0; i < levelSelectScreens.Length; i++)
        {
            levelButtons = levelSelectScreens[i].GetComponentsInChildren<Selectable>();
            for (int j = 0; j < levelButtons.Length; j++)
            {
                int current = j + (i * levelButtons.Length);

                if (levelUnlocked.Length > current && levelUnlocked[current] == true)
                {
                    levelButtons[j].GetComponentInChildren<Text>().enabled = true;
                    levelButtons[j].image.sprite = buttonImage;
                    color = levelButtons[j].colors;
                    color.normalColor = Color.white;
                    color.pressedColor = Color.grey;
                    levelButtons[j].colors = color;
                }
            }
        }
    }
}
