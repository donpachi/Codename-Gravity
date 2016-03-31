using UnityEngine;
using System.Collections;


/// <summary>
/// This script applieds the rail node behaviour to a transform.
/// Once the script is attached to the transform, the node's neighbours must be manually defined through the Unity hierarchy
/// </summary>
public class RailNode : MonoBehaviour {
    public GameObject[] Neighbours;
    public Vector3[] vectorToNeighbour;
    public Vector3 down;

    private TouchController.TouchLocation _touchLocation; //determines which path node takes, may be a frame late

    void Start()
    {
        _touchLocation = TouchController.TouchLocation.NONE;

        if (Neighbours == null || Neighbours.Length == 0)
        {
            Debug.LogError("Rail Node does not have any neighbours", gameObject);
        }
        //set direction vectors to all nodes
        vectorToNeighbour = new Vector3[Neighbours.Length];
        for (int i = 0; i < vectorToNeighbour.Length; i++)
        {
            vectorToNeighbour[i] = (Neighbours[i].transform.position - transform.position).normalized;
        }
        
    }

    /// <summary>
    /// Gets the next node in the path to travel to, if there is no suitable node, it will return itself
    /// </summary>
    /// <returns>GameObject nextNode</returns>
    public GameObject PathToTake()
    {
        down = OrientationListener.instanceOf.getRelativeDownVector();

        GameObject nextNode = gameObject;
        Vector3 movementVector;

        if (_touchLocation == TouchController.TouchLocation.LEFT)
            movementVector = OrientationListener.instanceOf.getRelativeLeftVector();
        else if (_touchLocation == TouchController.TouchLocation.RIGHT)
            movementVector = OrientationListener.instanceOf.getRelativeRightVector();
        else
            movementVector = Vector3.zero;

        for (int i = 0; i < vectorToNeighbour.Length; i++)
        {
            if (down == vectorToNeighbour[i])
                return Neighbours[i];       //prioritize gravity vector
            if (movementVector == vectorToNeighbour[i])
                nextNode = Neighbours[i];
        }

        return nextNode;
    }

    public void OnDrawGizmos()
    {
        if (Neighbours == null || Neighbours.Length < 1)
            return;

        for (var i = 0; i < Neighbours.Length; i++)
        {
            Gizmos.DrawLine(transform.position, Neighbours[i].transform.position);
            Vector3 boxOffset = ((Neighbours[i].transform.position - transform.position).normalized) / 10;
            Gizmos.DrawWireCube(transform.position + boxOffset, new Vector3(0.1f,0.1f,0));
        }
    }

    void screenTouched(TouchInstanceData data)
    {
        _touchLocation = data.touchLocation;
    }

    void OnEnable()
    {
        TouchController.ScreenTouched += screenTouched;
    }

    void OnDisable()
    {
        TouchController.ScreenTouched -= screenTouched;
    }
}
