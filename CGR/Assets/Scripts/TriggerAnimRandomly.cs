using UnityEngine;
using System.Collections;

public class TriggerAnimRandomly : MonoBehaviour {

    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}

    void playAnim()
    {
        StartCoroutine(waitForSomeTime(Random.Range(3f, 15f)));
    }

    IEnumerator waitForSomeTime(float time)
    {
        yield return new WaitForSeconds(time);
        anim.SetTrigger("PlayAnim");
    }
}
