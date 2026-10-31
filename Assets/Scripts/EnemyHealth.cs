using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private bool isBoss;

    [Header("Audio")]
    [SerializeField] private AudioClip deathSound;

    public event Action OnDeath;

    private float currentHealth;
    private bool isDead;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead || damage <= 0f)
            return;

        currentHealth = Mathf.Max(currentHealth - damage, 0f);

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        OnDeath?.Invoke();

        if (deathSound != null)
            AudioSource.PlayClipAtPoint(deathSound, transform.position);

        if (isBoss)
        {
            GameEndUI gameEndUI = FindFirstObjectByType<GameEndUI>();

            if (gameEndUI != null)
                gameEndUI.ShowVictory();
            else
                Debug.LogError("GameEndUI was not found in the scene.");
        }

        Destroy(gameObject);
    }
}