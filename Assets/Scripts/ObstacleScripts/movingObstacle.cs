using UnityEngine;

public class movingObstacle : obstacleBase
{
    protected override void onPlayerHit(playerController player)
    {
        if (!player.shielded)
        {
            player.stun(stunTimer);
            Debug.Log("Player stunned by moving obstacle!");
        }

        else
        {
            Debug.Log("Player shielded, no stun applied from moving obstacle.");
            player.shielded = false; //Shield is consumed
        }
    }
    void Update()
    {
        rb.linearVelocity = new Vector2(1 * speed, rb.linearVelocity.y);   //Gonna need to change this later to fit with TileLevelGen's movement system.
    }
}

