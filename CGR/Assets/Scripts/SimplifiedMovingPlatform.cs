using UnityEngine;
using System.Collections;

public class SimplifiedMovingPlatform : MonoBehaviour {

    public int numberOfMovements = -1;

	// Use this for initialization
	void Start () {
        GameObject[] pushableObjects = GameObject.FindGameObjectsWithTag("Untagged");
        foreach (GameObject pushableObject in pushableObjects)
        {
            if (pushableObject.GetComponent<Collider2D>())
            {
                foreach (Collider2D collider in pushableObject.GetComponents<Collider2D>())
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider);
            }
        } 
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<SliderJoint2D>().jointTranslation >= GetComponent<SliderJoint2D>().limits.max || GetComponent<SliderJoint2D>().jointTranslation <= GetComponent<SliderJoint2D>().limits.min && numberOfMovements != 0) 
        {
            float newMotorSpeed = GetComponent<SliderJoint2D>().motor.motorSpeed * -1;
            JointMotor2D newMotor = new JointMotor2D();
            newMotor.motorSpeed = newMotorSpeed;
            newMotor.maxMotorTorque = GetComponent<SliderJoint2D>().motor.maxMotorTorque;
            GetComponent<SliderJoint2D>().motor = newMotor;
            numberOfMovements--;
        }
	}
}
