using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MenuTransition : MonoBehaviour {

    public bool released;

	// Use this for initialization
	void Start () {
        released = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (released)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, this.GetComponentsInChildren<Transform>()[1].position, 0);
        }
        if (Vector3.Distance(this.transform.position, this.GetComponentsInChildren<Transform>()[1].position) < 0.001f)
        {
            released = false;
        }
	}

    public void OnEndDrag(PointerEventData eventData)
    {
        released = true;
    }
}
