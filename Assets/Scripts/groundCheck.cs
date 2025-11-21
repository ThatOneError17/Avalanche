using UnityEngine;

public class groundCheck : MonoBehaviour
{
    Rigidbody2D rb;
    private playerController player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponentInParent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Grounded");

            player.isGrounded = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Not Grounded");
            player.isGrounded = false;
        }
    }
}
