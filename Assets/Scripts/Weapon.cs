
using UnityEngine;

[RequireComponent(typeof(InputReader))]
public class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private float damage = 25f;
    [SerializeField] private float range = 100f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip hitSound;

    [Header("References")]
    [SerializeField] private Camera playerCamera;

    private InputReader inputReader;

    private void Awake()
    {
        inputReader = GetComponent<InputReader>();
    }

    private void Update()
    {
        if (GameEndUI.GameEnded)
            return;

        if (inputReader.ShootPressed)
            Shoot();
    }

    private void Shoot()
    {
        if (shootSound != null)
            audioSource.PlayOneShot(shootSound, 0.3f);

        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();

                if (enemy != null)
                {
                    enemy.TakeDamage(damage);

                    if (hitSound != null)
                        audioSource.PlayOneShot(hitSound);
                }
            }
        }
    }
}
