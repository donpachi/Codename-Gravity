using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SimplifiedMovingPlatform : MonoBehaviour {

    public int numberOfMovements = -1;
    public float timer = 0;

	// Use this for initialization
	void Start () {

        GameObject[] immovableObjects = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> objs = new List<GameObject>(immovableObjects);
        
        for (int i = 0; i < objs.Count; i++)
        {
            if (objs[i].layer != LayerMask.NameToLayer("Walls"))
                objs.Remove(objs[i]);
        }

        foreach (GameObject obj in objs)
        {
            if (obj.GetComponent<Collider2D>())
            {
                foreach (Collider2D collider in obj.GetComponents<Collider2D>())
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider);
            }
        }

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //if (timer > 0)
            //timer -= Time.deltaTime;
        if (GetComponent<SliderJoint2D>().jointTranslation >= GetComponent<SliderJoint2D>().limits.max || GetComponent<SliderJoint2D>().jointTranslation <= GetComponent<SliderJoint2D>().limits.min && numberOfMovements != 0)// && timer <= 0) 
        {
                float newMotorSpeed = GetComponent<SliderJoint2D>().motor.motorSpeed * -1;
                JointMotor2D newMotor = new JointMotor2D();
                newMotor.motorSpeed = newMotorSpeed;
                newMotor.maxMotorTorque = GetComponent<SliderJoint2D>().motor.maxMotorTorque;
                GetComponent<SliderJoint2D>().motor = newMotor;
                numberOfMovements--;
                //timer = 0.5f;
        }
	}
}
