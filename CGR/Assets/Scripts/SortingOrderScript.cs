using UnityEngine;
using System.Collections;
/// <summary>
/// Script Changes the SortingLayer or sorting order of the attached game object to the given value
/// </summary>
public class SortingOrderScript : MonoBehaviour {

    private SpriteRenderer rend;

	// Use this for initialization
	void Start ()
    {
        rend = GetComponent<SpriteRenderer>();
	}

    public void SetOrderTo(int order)
    {
        rend.sortingOrder = order;
    }

    public void SetLayerTo(string layer)
    {
        rend.sortingLayerName = layer;
    }
}
