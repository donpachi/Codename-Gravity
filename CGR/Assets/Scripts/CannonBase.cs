using UnityEngine;
using System.Collections;

/// <summary>
/// Fire Modes:
/// 0 - 180 degrees
/// 1 - 90 deg right
/// 2 - 90 deg middle
/// 3 - 90 deg left
/// </summary>
public class CannonBase : MonoBehaviour {
    public int CannonMode = 0;

    private GravityCannon cannon;

	// Use this for initialization
	void Start () {
        cannon = GetComponentInChildren<GravityCannon>();
        GetComponent<Animator>().SetInteger("CannonMode", CannonMode);
	}

    void EnableFireButton()
    {
        cannon.EnableFireButton();
    }

    public void SetFireMode(int mode)
    {
        CannonMode = mode;
        GetComponent<Animator>().SetInteger("CannonMode", CannonMode);
    }
}
