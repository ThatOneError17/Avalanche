using UnityEngine;
using System.Collections;

public class obstacleBase : MonoBehaviour
{
    [SerializeField] protected float stunTimer = 0f;
    [SerializeField] protected float speed = 0f;
    [SerializeField] protected float invincibilityTimer = 3f;
    protected bool isPlayerInvincible = false;

    protected Rigidbody2D rb;   //This will be used later for moving obstacles and for the auto movement of the level.

    //Pretty sure I'll have to add a default speed to make it move with TileLevelGen later, but for now it's fine.



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController player = collision.gameObject.GetComponent<playerController>();
            if (player != null)
            {
                isPlayerInvincible = true;
                onPlayerHit(player);
                StartCoroutine(HandleInvincibiltyFrames());
                Debug.Log("Player hit an obstacle!");
            }
        }

        if (collision.gameObject.CompareTag("Cleaner"))
        {
            Destroy(this.gameObject);
            Debug.Log("Obstacle destroyed by Cleaner.");
        }
    }

    protected IEnumerator HandleInvincibiltyFrames()
    {
        yield return new WaitForSeconds(invincibilityTimer);
        isPlayerInvincible = false;
    }

    protected void onPlayerHit(playerController player)
    {
        if (!player.shielded)
        {
            player.stun(stunTimer);
            StartCoroutine(accountForSpeedChange());
            Debug.Log("Player stunned by stationary obstacle!");
        }

        else
        {
            Debug.Log("Player shielded, no stun applied from stationary obstacle.");
            player.shielded = false; //Shield is consumed
        }
    }

    private IEnumerator accountForSpeedChange()
    {
        speed = speed + 2; //Will need to adjust with scroll speed later 
        yield return new WaitForSeconds(stunTimer);
        speed = speed - 2;  //Will need to adjust with scroll speed later
    }
}
