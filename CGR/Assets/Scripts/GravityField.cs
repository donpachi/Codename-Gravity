using UnityEngine;
using System.Collections;

public class GravityField : MonoBehaviour {

    public float PulseTime = 4f;
    public float TransitionTime = 1f;
    public float UpperTrans = 0.4f;
    public float LowerTrans = 0.2f;
    public Color PortraitColor;
    public Color LandLeftColor;
    public Color PortraitUpColor;
    public Color LandRightColor;

    float alphaStep;
    SpriteRenderer rend;
    int alphaDir = 1;
    bool changing = false;

    // Use this for initialization
    void Start () {
        transform.localScale = transform.parent.GetComponent<BoxCollider2D>().size;
        rend = GetComponent<SpriteRenderer>();
        alphaStep = (UpperTrans - LowerTrans) / PulseTime;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (changing)
            return;
        Color alphaColor = rend.color;
	    if(alphaColor.a >= UpperTrans)
        {
            alphaDir = -1;
        }
        else if(alphaColor.a <= LowerTrans)
        {
            alphaDir = 1;
        }

        alphaColor.a += alphaStep * alphaDir * Time.deltaTime;
        rend.color = alphaColor;
	}

    IEnumerator transToDirection(Color newColor)
    {
        changing = true;
        float transition = 0;
        Color currentColor = rend.color;

        while (transition < 1)
        {
            transition += Time.deltaTime / TransitionTime;
            rend.color = Color.Lerp(currentColor, newColor, transition);

            yield return null;
        }

        changing = false;
    }

    public void SetDirection(int dir)
    {
        switch (dir)
        {
            case 0:
                StartCoroutine(transToDirection(PortraitColor));
                break;
            case 1:
                StartCoroutine(transToDirection(LandLeftColor));
                break;
            case 2:
                StartCoroutine(transToDirection(PortraitUpColor));
                break;
            case 3:
                StartCoroutine(transToDirection(LandRightColor));
                break;
        }
    }
}
