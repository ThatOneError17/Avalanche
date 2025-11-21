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
    protected virtual void Awake()
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
    }

    protected IEnumerator HandleInvincibiltyFrames()
    {
        yield return new WaitForSeconds(invincibilityTimer);
        isPlayerInvincible = false;
    }

    protected virtual void onPlayerHit(playerController player)
    {
        // To be overridden in derived classes
    }
}
