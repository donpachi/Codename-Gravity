using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SnapToGrid : MonoBehaviour {
#if UNITY_EDITOR
    public bool snapToGrid = true;
    public bool sizeToGrid = false;
    public float snapValue = 0.5f;
    public float sizeValue = 0.25f;

	// Update is called once per frame
	void Update () {
        if (snapToGrid)
            transform.position = Round2Nearest(transform.position, snapValue);
        if (sizeToGrid)
            transform.localScale = Round2Nearest(transform.localScale, sizeValue);
	}

    private Vector3 Round2Nearest(Vector3 v, float snapValue)
    {
        return new Vector3(snapValue * Mathf.Round(v.x / snapValue), snapValue * Mathf.Round(v.y / snapValue), v.z);
    }
#endif
}
