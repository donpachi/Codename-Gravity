using UnityEngine;
using System.Collections;

public class FlameScript : MonoBehaviour {
    public float Frequency;
    public float Duration;

    private Animator anim;
    private float freqTimer;
    private float durTimer;
    private bool high;
    private PolygonCollider2D[] flameColliders;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        flameColliders = GetComponents<PolygonCollider2D>();
        freqTimer = 0;
        durTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (high)
        {
            durTimer += Time.deltaTime;
            if (durTimer >= Duration)
            {
                flameColliders[0].enabled = false;
                flameColliders[1].enabled = true;
                durTimer = 0;
                high = false;
                anim.SetBool("High", high);
            }
        }
        else
        {
            freqTimer += Time.deltaTime;
            if (freqTimer >= Frequency)
            {
                flameColliders[0].enabled = true;
                flameColliders[1].enabled = false;
                freqTimer = 0;
                high = true;
                anim.SetBool("High", high);
            }
        }


	}
}
