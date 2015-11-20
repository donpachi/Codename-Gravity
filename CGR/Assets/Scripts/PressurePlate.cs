using UnityEngine;
using System.Collections;
using System;

public class PressurePlate : MonoBehaviour {

    public delegate void PlateTriggeredHandler();
    public bool canBeUntriggered = false;
    public static event PlateTriggeredHandler Enter;
    public static event PlateTriggeredHandler Exit;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {

	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        OnEnter();
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (canBeUntriggered == true)
        {
            OnExit();
        }
    }

    void OnEnter()
    {
        if (Enter != null)
        {
            Enter();
        }
    }

    void OnExit()
    {
        if (Exit != null)
        {
            Exit();
        }
    }
}
