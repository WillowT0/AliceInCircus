using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float bounceVelocity = 30f; // Higher than normal jump

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (rb != null)
            {
                // Make bounce higher than normal jump
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceVelocity);

                // Reset jump state in PlayerController
                if (player != null)
                    player.OnJumpBounce();
            }
        }
    }
}
