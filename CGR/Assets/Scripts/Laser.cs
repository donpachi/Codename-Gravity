using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

    [Tooltip("Length of laser")]
    public float Length;
    [Tooltip("Fire's every x seconds")]
    public float FireFrequency;
    [Tooltip("Fire's for y amount of time")]
    public float FireTime;
    public bool On;
    public bool Fire;

    float frequencyCountDown = 0;
    float fireCountDown = 0;
    
    LineRenderer line;

	// Use this for initialization
	void Start () {
        line = gameObject.GetComponent<LineRenderer>();
        line.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!On)
            return;

        if (fireCountDown < FireTime)
        {
            Fire = true;
            StopCoroutine("FireLaser");
            StartCoroutine("FireLaser");

            fireCountDown += Time.deltaTime;
        }
        else
        {
            Fire = false;
            if(frequencyCountDown > FireFrequency)
            {
                counterReset();
            }
            frequencyCountDown += Time.deltaTime;
        }
	}

    IEnumerator FireLaser()
    {
        line.enabled = true;
        while (Fire && On)
        {
            Ray2D ray = new Ray2D(transform.position, transform.right);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Length);

            line.SetPosition(0, transform.localPosition);
            
            if (hit)
            {
                line.SetPosition(1, transform.InverseTransformPoint(hit.point));
                if (hit.rigidbody)
                {
                    if (hit.collider.gameObject.GetComponent<Player>())
                        hit.collider.gameObject.GetComponent<Player>().TriggerDeath("Laser");
                }
            }
            else
            {
                line.SetPosition(1, transform.InverseTransformPoint(ray.GetPoint(Length)));
            }

            yield return null;
        }
        line.enabled = false;
    }

    void counterReset()
    {
        frequencyCountDown = 0;
        fireCountDown = 0;
    }

    void plateDepressed()
    {
        On = !On;
    }

    void plateReleased()
    {

    }

}
