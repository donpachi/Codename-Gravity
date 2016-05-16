using UnityEngine;
using System.Collections.Generic;

public class PlatformCheck : MonoBehaviour {

    private GroundCheck groundCheck;
    private GameObject currentMovingPlatform;

    void Start()
    {
        groundCheck = GetComponent<GroundCheck>();
    }

    // Update is called once per frame
    void Update ()
    {
        CheckForPlatform();
	    if (currentMovingPlatform != null)
        {
            Vector2 moveDistance = currentMovingPlatform.GetComponent<MovingPlatform>().getMoveDifference();
            transform.position = new Vector2(transform.position.x - moveDistance.x, transform.position.y - moveDistance.y);
            currentMovingPlatform = null;
        }
	}

    void CheckForPlatform()
    {
        GameObject[] collidedObjects = groundCheck.getCollidedObjects();
        Dictionary<GameObject, int> currentMovingPlatforms = new Dictionary<GameObject, int>();
        int highestNumOfCollisions = 0;
        currentMovingPlatform = null;
        

        foreach (var obj in collidedObjects)
        {
            if (obj != null && obj.tag == "Dynamic")
            {
                int numOfCollisions = 1;

                if (currentMovingPlatforms.ContainsKey(obj))
                {
                    currentMovingPlatforms.TryGetValue(obj, out numOfCollisions);
                    numOfCollisions++;
                    currentMovingPlatforms[obj]++;
                }
                else
                    currentMovingPlatforms.Add(obj, 1);

                if (numOfCollisions > highestNumOfCollisions)
                {
                    highestNumOfCollisions = numOfCollisions;
                    currentMovingPlatform = obj;
                }
            }
        }
    }
}
