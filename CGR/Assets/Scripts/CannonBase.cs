using UnityEngine;
using System.Collections;

public class CannonBase : MonoBehaviour {
    private GravityCannon cannon;

	// Use this for initialization
	void Start () {
        cannon = GetComponentInChildren<GravityCannon>();
	}

    void EnableFireButton()
    {
        cannon.EnableFireButton();
    }
}
