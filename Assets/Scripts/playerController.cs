using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public event System.Action GetCoin;
    public event System.Action PlayerDeath;
    public event System.Action HasExtraLife;
    public event System.Action <float> PlayerStun;
    [SerializeField] private float speed = 5.0f;    //Temporary, later the level will move towards the player
    [SerializeField] private float jumpForce = 7.0f;
    [SerializeField] private int extraLifes = 0;
    [SerializeField] public bool shielded = false;
    [SerializeField] private int extraJumps = 2;
    [SerializeField] private int maxExtraJumps = 2;
    [SerializeField] private bool stunned = false;

    private bool isKillable = true;

    public bool isGrounded;
    private bool jumpPressed;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    [SerializeField] private GameObject gameOverUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        //rb.linearVelocity = new Vector2(1 * speed, rb.linearVelocity.y);   //Auto run to the right, for ease of movement in the game
        jumpPressed = false;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !stunned)
            jumpPressed = true;

        if (Input.GetMouseButtonDown(0) && !stunned)
            jumpPressed = true;

        if (isGrounded)  
            extraJumps = maxExtraJumps;
        

        if (jumpPressed && isGrounded)
            Jump();

        else if (jumpPressed && extraJumps > 0 && !isGrounded && !stunned)
        {
            Jump();
            extraJumps--;

        }


    }

   

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            Debug.Log("Coin Collected");
            Destroy(collision.gameObject);
            GetCoin?.Invoke();
        }

        if (collision.CompareTag("PowerUp"))
        {
            PowerUp powerUp = collision.GetComponent<PowerUp>();
            switch (powerUp.type)
            {
                case PowerUp.PowerUpType.speedBoost:
                    //Gonna implement this in TileLevelGen later
                    Debug.Log("Speed Boost Collected! (No effect implemented)");
                    break;
                case PowerUp.PowerUpType.extraLife:
                    gotExtraLife();
                    break;
                case PowerUp.PowerUpType.extraJump:
                    gotExtraJump();
                    break;
                case PowerUp.PowerUpType.shield:
                    gotShield();
                    break;
                case PowerUp.PowerUpType.star:
                    gotStar();
                    break;
                default:
                    Debug.LogWarning("Unknown PowerUp type collected.");
                    break;
            }
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Death"))
        {
            if (extraLifes > 0)
            {
                StartCoroutine(HandleDeathInvincibility());
                HasExtraLife?.Invoke();
                extraLifes--;
                Debug.Log("Extra Life used! Remaining Extra Lives: " + extraLifes);
            }
            else if (isKillable)
            {
                death();
            }
        }
    }

    private void gotExtraLife()
    {
        extraLifes++;
        Debug.Log("Extra Life Gained! Total Extra Lifes: " + extraLifes);
    }

    private void gotExtraJump()
    {
        maxExtraJumps++;
        extraJumps++;
        Debug.Log("Extra Jump Gained! Total Jumps: " + maxExtraJumps);
    }

    private void gotShield()
    {
        shielded = true;
        Debug.Log("Shield Gained! Player is now shielded.");
    }

    private void gotStar()
    {
        maxExtraJumps++;
        extraJumps++;
        shielded = true;
        extraLifes++;
        Debug.Log("Star Gained! Jumps, Shield, and Extra life gained.");
    }

    public void stun(float stunTime)
    {
        Debug.Log("Player Stunned for " + stunTime + " seconds.");
        stunned = true;
        PlayerStun?.Invoke(stunTime);
        StartCoroutine(StunDuration(stunTime));
    }

    private IEnumerator StunDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        stunned = false;
        Debug.Log("Player is no longer stunned.");
    }

    private void death()
    {
        Debug.Log("Player has died.");
        Time.timeScale = 0f; // Pause the game
        PlayerDeath?.Invoke();

    }   

    private IEnumerator HandleDeathInvincibility()
    {
        isKillable = false;
        yield return new WaitForSeconds(2f);
        isKillable = true;
    }
}
