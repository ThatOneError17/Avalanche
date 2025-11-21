using UnityEngine;

public class movingObstacle : obstacleBase
{
    
    void Update()
    {
        rb.linearVelocity = new Vector2(1 * speed, rb.linearVelocity.y);   //Gonna need to change this later to fit with TileLevelGen's movement system.
    }
}

