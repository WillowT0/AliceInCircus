using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("UI Reference")]
    public HealthUi healthUi;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        currentHealth = maxHealth;
        if (healthUi != null)
        {
            healthUi.SetMaxHearts(maxHealth);
            healthUi.UpdateHearts(currentHealth);

            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TEMPORARY: damage player when touching objects tagged as "Enemy"
        if (collision.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (healthUi != null)
        {
            healthUi.UpdateHearts(currentHealth);
        }

        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
        }

      

    }

    private void Die()
    {
        Debug.Log("Player died!");
        // TODO: Add respawn or game over logic here
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }
}
