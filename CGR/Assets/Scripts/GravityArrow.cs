using UnityEngine;
using System.Collections;

public class GravityArrow : MonoBehaviour {

    public float TransitionTime = 1;

    private int currentDir = 0;

    IEnumerator transToDirection(float angle)
    {
        float stepSize = angle / TransitionTime;
        float remainingAngle = angle;
        bool changing = true;

        while (changing)
        {
            float angleChange = stepSize * Time.deltaTime;
            if(remainingAngle < angleChange)
            {
                changing = false;
                transform.Rotate(0, 0, -remainingAngle);
            }
            else
            {
                transform.Rotate(0, 0, -angleChange);
                remainingAngle -= angleChange;
            }

            yield return null;
        }
    }

    /// <summary>
    /// Only rotates clockwise.
    /// </summary>
    /// <param name="dir"></param>
    public void SetDirection(int dir)
    {
        if (dir == currentDir)
            return;

        int directionDiff = dir - currentDir;

        if (directionDiff == -3)
            directionDiff = 1;
        else
            directionDiff = Mathf.Abs(directionDiff);

        if (directionDiff == 1)
            StartCoroutine(transToDirection(90));
        else if(directionDiff == 2)
            StartCoroutine(transToDirection(180));
        else
            StartCoroutine(transToDirection(270));
        currentDir = dir;
    }
}
