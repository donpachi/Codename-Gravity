using UnityEngine;
using System.Collections;
using System;

public class PressurePlate : MonoBehaviour
{
    public bool CanBeUntriggered = false;
    public float ReleaseDelay = 0;
    public GameObject[] List;

    float timer = 0;
    bool timerCountingDown = false;
    bool pressing = false;
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
		if (collider.tag == "Boulder" || collider.tag == "Pushable" || (collider.tag == "Minion" && collider.GetComponent<Minion>().IsFollowing == false))
            anim.SetInteger("State", 1);
        pressing = true;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
		if (collider.tag == "Boulder" || collider.tag == "Pushable" || (collider.tag == "Minion" && collider.GetComponent<Minion>().IsFollowing == false))
        {
            if (CanBeUntriggered == true)
                timerCountingDown = true;
        }
        pressing = false;
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
    }
    void OnDisable()
    {
        LevelManager.OnCheckpointLoad -= checkpointLoad;
        LevelManager.OnCheckpointSave -= checkpointSave;
    }
}

class ButtonState
{
    public int state;
    public float timer;
    public bool timerCountingDown;
}