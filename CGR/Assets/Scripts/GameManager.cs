using UnityEngine;

public class GameManager
{
    public static GameManager Instance { get { return null; } }

    public int Points { get; private set; }

    public void Reset()
    {
        
    }

    public void ResetPoints(int points)
    {
        Points = points;
    }

    public void AddPoints(int points)
    {

    }


}

