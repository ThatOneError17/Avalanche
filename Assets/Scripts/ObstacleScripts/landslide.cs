using UnityEngine;
using System.Collections;

public class landslide : MonoBehaviour
{
    [SerializeField] private float speed = 2f;  //Later this will also factor in scroll speed of the level
    private bool playerStunned = false;
    private bool extraLifeUsed = false;
    private bool extraLifeBehaviorExecuted = false;

    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();

        var player = FindFirstObjectByType<playerController>();
        player.PlayerStun += (stunTime) =>
        {
            StartCoroutine(stunDuration(stunTime));
        };

        player.HasExtraLife += () =>
        {
            extraLifeBehavior();
        };
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (extraLifeBehaviorExecuted)
        {
            if (transform.position.x > -25)
            {
                rb.linearVelocity = new Vector2(-5 * speed, rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                extraLifeBehaviorExecuted = false;
                extraLifeUsed = false;
            }

            return;
        }

        if (playerStunned)
        {
            rb.linearVelocity = new Vector2(2 * speed, rb.linearVelocity.y);
        }

        else if (!playerStunned && transform.position.x > -25)
        {
            rb.linearVelocity = new Vector2(1 * -speed, rb.linearVelocity.y);
        }

        else
        {
            //stop moving when at or past x = -25
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    
    private IEnumerator stunDuration(float stunTime)
    {
        playerStunned = true;
        yield return new WaitForSeconds(stunTime);
        playerStunned = false;
    }

    private void extraLifeBehavior()
    {
        //Move the landslide back by a certain distance
        extraLifeUsed = true;
        extraLifeBehaviorExecuted = true;

    }
}
