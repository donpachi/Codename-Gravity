using UnityEngine;
using System.Collections;
/// <summary>
/// shrinks the ring around a vortex. requires pulse interval and vortex distance to be defined before first update cycle
/// </summary>
public class VortexRing : MonoBehaviour {


    float pulseInterval;
    float vortexDistance;
    float scaleValue;
    float currentScale;
    float minScale = 0.2f;
    float alphaStep;
    float alphaScale;
    SpriteRenderer rend;
	
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        alphaStep = 0.8f / pulseInterval;
        transform.localScale = new Vector2(vortexDistance, vortexDistance);
        rend.color = new Color(1.0f, 1.0f, 1.0f, alphaScale);
    }

    // Update is called once per frame
    void Update()
    {
        if (vortexDistance == 0)
        {
            Debug.LogError("Object has not been properly initialized");
            Destroy(gameObject);
            return;
        }

        currentScale -= Time.deltaTime * scaleValue;
        alphaScale += alphaStep * Time.deltaTime;

        if(alphaScale > 1)
            alphaScale = 1;

        if (currentScale <= minScale)
        { 
            currentScale = vortexDistance;
            alphaScale = alphaStep;
        }
        transform.localScale = new Vector2(currentScale, currentScale);
        rend.color = new Color(1.0f, 1.0f, 1.0f, alphaScale);
	}

    /// <summary>
    /// Needs to be called before first update loop
    /// </summary>
    /// <param name="distance"></param>
    /// <param name="interval"></param>
    public void Setup(float distance, float interval)
    {
        vortexDistance = distance;
        pulseInterval = interval;
        scaleValue = vortexDistance / pulseInterval;       //scale in milliseconds
    }
}
