using UnityEngine;
using System.Collections;

public class DestroyObject : MonoBehaviour {
    public float DestroyTime = 2f;
    float lifeTime = 0;
	
	// Update is called once per frame
	void Update () {
        lifeTime += Time.deltaTime;
        if (lifeTime > DestroyTime)
            Destroy(gameObject);
	}

	void OnCollisionEnter2D (Collision2D collision){
		//Just destroys the object for now. may want to change to an animation
		Destroy (gameObject);
	}

}
