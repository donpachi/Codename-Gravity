using UnityEngine;
using System.Collections;
using System;

public class PressurePlate : MonoBehaviour
{
    public bool CanBeUntriggered = false;
    public bool Lever;
    public float ReleaseDelay = 0;
    public GameObject[] List;

    float timer = 0;
    bool timerCountingDown = false;
    bool pressing = false;
    bool inArea;
    ButtonState checkpointState;
    private Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        timer = ReleaseDelay;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timerCountingDown == true)
            timer -= Time.deltaTime;
        if (CanBeUntriggered == true && !pressing)
            checkIfRelease();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        inArea = true;
        if (Lever)
            return;
		if (collider.tag == "Boulder" || collider.tag == "Pushable" || (collider.tag == "Minion" && collider.GetComponent<Minion>().IsFollowing == false))
        {
            anim.SetInteger("State", 1);
            pressing = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        inArea = false;
		if (collider.tag == "Boulder" || collider.tag == "Pushable" || (collider.tag == "Minion" && collider.GetComponent<Minion>().IsFollowing == false))
        {
            if (CanBeUntriggered == true && pressing == true)
                timerCountingDown = true;
            pressing = false;
        }
    }

    void checkSwipe(TouchController.SwipeDirection direction)
    {
        if (!Lever)
            return;
        if(direction == TouchController.SwipeDirection.UP && inArea)
        {
            if (anim.GetInteger("State") == 0)
                anim.SetInteger("State", 1);
            else
                anim.SetInteger("State", 0);
            pressing = true;
        }
    }

    void checkIfRelease()
    {
        if (timer <= 0)
            anim.SetInteger("State", 0);
    }

    void broadcastDepress()
    {
        timerCountingDown = false;
        timer = ReleaseDelay;
        foreach (GameObject item in List)
            item.SendMessage("plateDepressed");
    }

    void broadcastRelease()
    {
        timerCountingDown = false;
        foreach (GameObject item in List)
            item.SendMessage("plateReleased");
    }

    void checkpointSave()
    {
        checkpointState = new ButtonState();
        checkpointState.state = anim.GetInteger("State");

        if (CanBeUntriggered)
        {
            checkpointState.timer = timer;
            checkpointState.timerCountingDown = timerCountingDown;
        }
    }

    void checkpointLoad()
    {
        if (CanBeUntriggered)
        {
            timer = checkpointState.timer;
            timerCountingDown = checkpointState.timerCountingDown;
        }
        else
        {
            anim.SetInteger("State", checkpointState.state);
        }
    }

    void OnEnable()
    {
        LevelManager.OnCheckpointLoad += checkpointLoad;
        LevelManager.OnCheckpointSave += checkpointSave;
        TouchController.OnSwipe += checkSwipe;
    }
    void OnDisable()
    {
        LevelManager.OnCheckpointLoad -= checkpointLoad;
        LevelManager.OnCheckpointSave -= checkpointSave;
        TouchController.OnSwipe -= checkSwipe;
    }
}

class ButtonState
{
    public int state;
    public float timer;
    public bool timerCountingDown;
}