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
    public bool CornerLaser;

    float frequencyCountDown = 0;
    float fireCountDown = 0;
    float chargeTime = 0.55f; //time it takes the fire animation to finish
    
    LineRenderer line;
    Animator anim;

	// Use this for initialization
	void Start () {
        line = gameObject.GetComponent<LineRenderer>();
        line.enabled = false;
        anim = GetComponentInParent<Animator>();
	}

	// Update is called once per frame
	void Update () {
        if (!On)
        {
            anim.SetBool("On", On);
            counterReset();
            return;
        }
        
        anim.SetBool("On", On);

        if(frequencyCountDown < FireFrequency)
        {
            if(frequencyCountDown > FireFrequency - chargeTime)
            {
                anim.SetBool("Fire", true);
            }
            frequencyCountDown += Time.deltaTime;
        }
        else if (fireCountDown < FireTime)
        {
            Fire = true;
            StopCoroutine("FireLaser");
            StartCoroutine("FireLaser");

            fireCountDown += Time.deltaTime;
        }
        else
        {
            counterReset();
            Fire = false;
        }
	}

    IEnumerator FireLaser()
    { 
        while (Fire && On)
        {
            Vector2 direction = transform.up;
            if (CornerLaser)
            {
                Quaternion offsetAngle = Quaternion.AngleAxis(45f, Vector3.forward);
                direction = offsetAngle * direction;
            }
                

            line.enabled = true;
            Ray2D ray = new Ray2D(transform.position, direction);
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
        anim.SetBool("Fire", false);
    }

    void plateDepressed()
    {
        On = !On;
    }

    void plateReleased()
    {

    }

    public void OnDrawGizmos()
    {
        Vector2 direction = transform.up;
        if (CornerLaser)
        {
            Quaternion offsetAngle = Quaternion.AngleAxis(45f, Vector3.forward);
            direction = offsetAngle * direction;
        }
        Gizmos.DrawRay(this.transform.position, direction*Length);
    }

}
