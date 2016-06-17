using UnityEngine;
using System.Collections.Generic;

public class WindtunnelPush : MonoBehaviour {

    private WindTunnel[] windtunnels;
    private List<GameObject> pushed;
    private Dictionary<GameObject, float> pushItems;
    private const float turbineStartupTime = 1.5f;
    private float turbineRuntime;
    private float windForceRatio;

    public bool TurbineOn;

    private bool turbineState;

    void Start () {
        windtunnels = GetComponentsInChildren<WindTunnel>();
        foreach (var turbine in windtunnels)
        {
            turbine.TurbineOn = TurbineOn;
        }
        pushed = new List<GameObject>();
        setAnimationState(TurbineOn);
    }
	
	void FixedUpdate () {
        if(TurbineOn != turbineState)
        {
            changeState(TurbineOn);
        }
        if (!turbineState)
            return;

        if (turbineRuntime < turbineStartupTime)
        {
            turbineRuntime += Time.deltaTime;
            windForceRatio = turbineRuntime / turbineStartupTime;
        }
        else
            windForceRatio = 1;

        foreach (var turbine in windtunnels)
        {
            if(turbine.objList != null)
            {
                foreach(KeyValuePair<GameObject, float> entry in turbine.objList)
                {
                    if (!pushed.Contains(entry.Key))
                    {
                        addWindForce(entry.Key, entry.Value);
                        pushed.Add(entry.Key);
                    }
                }
            }           
        }
        pushed.Clear();
	}

    void addWindForce(GameObject obj, float force)
    {
        obj.GetComponent<Rigidbody2D>().AddForce(transform.up * force * windForceRatio);
        //Debug.Log("Added Force to: " + obj.name + " With Force: " + force);
    }

    /// <summary>
    /// Function looks throguh all the child objects and changes the animation state for each object
    /// </summary>
    /// 
    void setAnimationState(bool state)
    {
        Animator[] animators = gameObject.GetComponentsInChildren<Animator>();
        foreach (Animator anim in animators)
        {
            anim.SetBool("TurbineOn", state);
        }
    }

    void changeState(bool state)
    {
        turbineState = state;
        setAnimationState(turbineState);
        foreach (var turbine in windtunnels)
        {
            turbine.TurbineOn = state;
        }
        turbineRuntime = 0;
    }

    void toggleOnOff()
    {
        turbineState = !turbineState;
        setAnimationState(turbineState);
        foreach (var turbine in windtunnels)
        {
            turbine.TurbineOn = turbineState;
        }
    }

    void plateDepressed()
    {
        TurbineOn = !TurbineOn;
    }

    void plateReleased()
    {
        TurbineOn = !TurbineOn;
    }
}
