using UnityEngine;

public class stationaryObstacle : obstacleBase
{
   protected override void onPlayerHit(playerController player)
   {
       if (!player.shielded)
       {
           player.stun(stunTimer);
           Debug.Log("Player stunned by stationary obstacle!");
       }

       else
       {
            Debug.Log("Player shielded, no stun applied from stationary obstacle.");
            player.shielded = false; //Shield is consumed
       }
    }
}
