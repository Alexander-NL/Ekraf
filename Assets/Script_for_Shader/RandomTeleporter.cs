using System.Collections;
using UnityEngine;

public class RandomTeleporter : MonoBehaviour
{
    [Header("Pengaturan Batas (Boundaries)")]
    [Tooltip("Pojok kiri bawah batas area")]
    public Vector2 minBounds;
    
    [Tooltip("Pojok kanan atas batas area")]
    public Vector2 maxBounds;

    [Header("Pengaturan Teleport")]
    [Tooltip("Total durasi efek teleport (dalam detik)")]
    public float totalDuration = 4f;
    
    [Tooltip("Seberapa sering objek berpindah (dalam detik)")]
    public float teleportInterval = 0.1f;

    private Vector2 originalPosition;
    private Coroutine teleportCoroutine;

    private void Awake()
    {
        originalPosition = transform.position;
    }

    // Gunakan ini untuk testing (tekan 'T' untuk memulai)
    // Untuk Versi sebenarnya akan dijalankan ketika boss mati (sekaligus trigger vfx)
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartTeleportEffect();
        }
    }

    //Public function buat dipanggil di script lain
    public void StartTeleportEffect()
    {
        if (teleportCoroutine != null)
        {
            StopCoroutine(teleportCoroutine);
        }

        teleportCoroutine = StartCoroutine(TeleportRoutine());
    }

    private IEnumerator TeleportRoutine()
    {
        // Timer
        float durationTimer = 0f;

        while (durationTimer < totalDuration)
        {
            float randomX = Random.Range(minBounds.x, maxBounds.x);
            float randomY = Random.Range(minBounds.y, maxBounds.y);

            transform.position = new Vector2(randomX, randomY);

            yield return new WaitForSeconds(teleportInterval);
            durationTimer += teleportInterval;
        }
    }
}