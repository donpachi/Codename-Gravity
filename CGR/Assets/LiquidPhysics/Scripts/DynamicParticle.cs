using UnityEngine;
using System.Collections;
/// <summary>
/// Dynamic particle.
/// 
/// The dynamic particle is the backbone of the liquids effect. Its a circle with physics with 3 states, each state change its physic properties and its sprite color ( so the shader can separate wich particle is it to draw)
/// The particles scale down and die, and have a scale  effect towards their velocity.
/// 
/// Visit: www.codeartist.mx for more stuff. Thanks for checking out this example.
/// Credit: Rodrigo Fernandez Diaz
/// Contact: q_layer@hotmail.com
/// </summary>

public class DynamicParticle : MonoBehaviour {	
	public enum STATES{WATER,GAS,LAVA,NONE}; //The 3 states of the particle
	STATES currentState=STATES.NONE; //Defines the currentstate of the particle, default is water
	public GameObject currentImage; //The image is for the metaball shader for the effect, it is onle seen by the liquids camera.
	public GameObject[] particleImages; //We need multiple particle images to reduce drawcalls
	float GAS_FLOATABILITY=7.0f; //How fast does the gas goes up?
	float particleLifeTime=4.0f,startTime;//How much time before the particle scalesdown and dies	
    float RAYCASTCOLLISIONDISTANCE = 5f;
    float rayOriginOffset = 0;
    float absoluteSize = 0;
    

    void Awake(){ 
		if (currentState == STATES.NONE)
			SetState (STATES.WATER);
        
	}

	//The definitios to each state
	public void SetState(STATES newState){
		if(newState!=currentState){ //Only change to a different state
			switch(newState){
				case STATES.WATER:													
					GetComponent<Rigidbody2D>().gravityScale=1.0f; // To simulate Water density
				break;
				case STATES.GAS:		
					particleLifeTime=particleLifeTime/2.0f;	// Gas lives the time the other particles
					GetComponent<Rigidbody2D>().gravityScale=0.0f;// To simulate Gas density
					gameObject.layer=LayerMask.NameToLayer("Gas");// To have a different collision layer than the other particles (so gas doesnt rises up the lava but still collides with the wolrd)
				break;					
				case STATES.LAVA:
					GetComponent<Rigidbody2D>().gravityScale=0.3f; // To simulate the lava density
				break;	
				case STATES.NONE:
					Destroy(gameObject);
				break;
			}
			if(newState!=STATES.NONE){
				currentState=newState;
				startTime=Time.time;//Reset the life of the particle on a state change
				GetComponent<Rigidbody2D>().velocity=new Vector2();	// Reset the particle velocity	
				currentImage.SetActive(false);
				currentImage=particleImages[(int)currentState];
				currentImage.SetActive(true);
			}
		}		
	}
	void FixedUpdate () {
        switch (currentState){
			case STATES.WATER: //Water and lava got the same behaviour
				MovementAnimation(); 
				ScaleDown();
			break;
			case STATES.LAVA:
				MovementAnimation();
				ScaleDown();
			break;
			case STATES.GAS:
				if(GetComponent<Rigidbody2D>().velocity.y<50){ //Limits the speed in Y to avoid reaching mach 7 in speed
					GetComponent<Rigidbody2D>().AddForce (new Vector2(0,GAS_FLOATABILITY)); // Gas always goes upwards
				}
				ScaleDown();
			break;
		}
        if (this.GetComponent<Rigidbody2D>().velocity.x == 0 && this.GetComponent<Rigidbody2D>().velocity.y == 0)
        {
            spawnWaterBody();
            Destroy(this);
        }
	}
	// This scales the particle image acording to its velocity, so it looks like its deformable... but its not ;)
	void MovementAnimation(){
		Vector3 movementScale=new Vector3(1.0f,1.0f,1.0f);//TamaÃ±o de textura no de metaball			
		movementScale.x+=Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x)/30.0f;
		movementScale.z+=Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y)/30.0f;
		movementScale.y=1.0f;		
		currentImage.gameObject.transform.localScale=movementScale;
	}
	// The effect for the particle to seem to fade away
	void ScaleDown(){ 
		float scaleValue = 1.0f-((Time.time-startTime)/particleLifeTime);
		Vector2 particleScale=Vector2.one;
		if (scaleValue <= 0) {
            spawnWaterBody();
            Destroy(gameObject);
		} else{
			particleScale.x=scaleValue;
			particleScale.y=scaleValue;
			transform.localScale=particleScale;
		}
	}

	// To change particles lifetime externally (like the particle generator)
	public void SetLifeTime(float time){
		particleLifeTime=time;	
	}
    void spawnWaterBody()
    {
        // Logic here for creating a new waterbody
        Debug.Log("Spawning Water Body");

        Vector2 leftRayOrigin = new Vector2(this.transform.position.x, this.transform.position.y);
        Vector2 rightRayOrigin = new Vector2(this.transform.position.x, this.transform.position.y);
        RaycastHit2D leftsidedownhit = Physics2D.Raycast(leftRayOrigin, OrientationListener.instanceOf.getRelativeDownVector(), RAYCASTCOLLISIONDISTANCE, 1 << LayerMask.NameToLayer("Walls"));
        RaycastHit2D rightsidedownhit = Physics2D.Raycast(rightRayOrigin, OrientationListener.instanceOf.getRelativeDownVector(), RAYCASTCOLLISIONDISTANCE, 1 << LayerMask.NameToLayer("Walls"));
        RaycastHit2D leftsidehit = Physics2D.Raycast(leftRayOrigin, OrientationListener.instanceOf.getRelativeLeftVector(), Mathf.Infinity, 1 << LayerMask.NameToLayer("Walls"));
        RaycastHit2D rightsidehit = Physics2D.Raycast(rightRayOrigin, OrientationListener.instanceOf.getRelativeRightVector(), Mathf.Infinity, 1 << LayerMask.NameToLayer("Walls"));
        Debug.Log("leftRayOrigin " + leftRayOrigin);
        Debug.Log("RayCastHitLeft " + leftsidehit.point.y);
        Debug.Log("RayCastHitRight " + rightsidehit.point.y);

        while (leftsidedownhit.collider != null && rightsidedownhit.collider != null)
        {
            if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT)
            {
                leftRayOrigin = new Vector2(this.transform.position.x - rayOriginOffset, this.transform.position.y);
                rightRayOrigin = new Vector2(this.transform.position.x + rayOriginOffset, this.transform.position.y);
            }
            else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT)
            {
                leftRayOrigin = new Vector2(this.transform.position.x + rayOriginOffset, this.transform.position.y);
                rightRayOrigin = new Vector2(this.transform.position.x - rayOriginOffset, this.transform.position.y);
            }
            else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_LEFT)
            {
                leftRayOrigin = new Vector2(this.transform.position.x + rayOriginOffset, this.transform.position.y);
                rightRayOrigin = new Vector2(this.transform.position.x - rayOriginOffset, this.transform.position.y);
            }
            else
            {
                leftRayOrigin = new Vector2(this.transform.position.x, this.transform.position.y - rayOriginOffset);
                rightRayOrigin = new Vector2(this.transform.position.x, this.transform.position.y + rayOriginOffset);
            }
            leftsidedownhit = Physics2D.Raycast(leftRayOrigin, OrientationListener.instanceOf.getRelativeDownVector(), RAYCASTCOLLISIONDISTANCE, 1 << LayerMask.NameToLayer("Walls"));
            rightsidedownhit = Physics2D.Raycast(rightRayOrigin, OrientationListener.instanceOf.getRelativeDownVector(), RAYCASTCOLLISIONDISTANCE, 1 << LayerMask.NameToLayer("Walls"));
            rayOriginOffset++;
        }

        // Calculate which is closer, the raycast wall collision or floor collision
        // Left raycast, temporarily just for Landscape Right
        Vector2 closestCollisionLeft = new Vector2(this.transform.position.x, leftsidehit.point.y);//Mathf.Max(leftsidehit.point.y, leftsidedownhit.point.y));
        Vector2 closestCollisionRight = new Vector2(this.transform.position.x, rightsidehit.point.y);//Mathf.Min(rightsidehit.point.y, rightsidedownhit.point.y));
        absoluteSize = Mathf.Abs(leftsidehit.point.y) + Mathf.Abs(rightsidehit.point.y);//Mathf.Abs(closestCollisionLeft.y) + Mathf.Abs(closestCollisionRight.y);

        // Create a water body
        GameObject waterBody = (GameObject)Instantiate(Resources.Load("Prefabs/Water"));
        waterBody.transform.localScale = new Vector2(0.1f, 0.1f);
        waterBody.transform.position = new Vector2(this.transform.position.x, (leftsidehit.point.y + rightsidehit.point.y)/2);
        
        waterBody.transform.localScale = resizeWater(waterBody);
        //Debug.Log("WaterBodyLeftSize " + waterBody.GetComponent<SpriteRenderer>().bounds.min.y);
    }

    Vector2 resizeWater(GameObject waterBody)
    {
        Vector2 localScale = waterBody.transform.localScale;
        Vector2 position = waterBody.transform.position;
        
        if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_RIGHT)
        {
            localScale.y = absoluteSize * localScale.y / waterBody.GetComponent<SpriteRenderer>().bounds.size.y;
        }
        return localScale;
    }

    // Here we handle the collision events with another particles, in this example water+lava= water-> gas

    void OnCollisionEnter2D(Collision2D other)
    {
        //if(currentState==STATES.WATER && other.gameObject.tag=="DynamicParticle"){ 
        //	if(other.collider.GetComponent<DynamicParticle>().currentState==STATES.LAVA){
        //		SetState(STATES.GAS);
        //	}
        //}

        //Logic here for combining into a waterbody
        if (other.gameObject.tag == "Water" && this.transform.localScale.x < 0.7f)
        {
            Vector2 localScale = other.transform.localScale;
            if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_RIGHT)
            {
                localScale.x = localScale.x + 0.1f;
                other.transform.localScale = localScale;
            }
            Destroy(this);
        }

    }

}
