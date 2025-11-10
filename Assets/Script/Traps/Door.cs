using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Slam Settings")]
    [SerializeField] private float slamDownSpeed = 10f;
    [SerializeField] private float returnUpSpeed = 3f;
    [SerializeField] private float slamDistance = 5f;

    [Header("References")]
    [SerializeField] private BoxCollider2D boxCollider;

    private Vector3 originalPosition;
    private bool isSlammingDown = false;
    private bool isReturningUp = false;
    private bool hasHitGround = false;

    void Start()
    {
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();

        originalPosition = transform.position;
        StartSlamming();
    }

    void Update()
    {
        if (isSlammingDown)
        {
            SlamDown();
        }
        else if (isReturningUp)
        {
            ReturnUp();
        }
    }

    public void StartSlamming()
    {
        isSlammingDown = true;
        isReturningUp = false;
        hasHitGround = false;
    }

    private void SlamDown()
    {
        // Move down quickly
        transform.Translate(Vector3.down * slamDownSpeed * Time.deltaTime);

        // Check for ground collision
        if (CheckGroundCollision() || ReachedMaxDistance())
        {
            OnHitGround();
        }
    }

    private void ReturnUp()
    {
        // Move up slowly towards original position
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, returnUpSpeed * Time.deltaTime);

        // Check if returned to original position
        if (Vector3.Distance(transform.position, originalPosition) < 0.1f)
        {
            OnReturnedToOriginal();
        }
    }

    private bool CheckGroundCollision()
    {
        Vector2 boxSize = boxCollider.size;
        Vector2 boxCenter = (Vector2)transform.position + boxCollider.offset;
        float castDistance = 0.2f; // Increased from 0.1f to 0.2f

        RaycastHit2D hit = Physics2D.BoxCast(boxCenter, boxSize, 0f, Vector2.down, castDistance);

        if (hit.collider != null && hit.collider.CompareTag("Ground"))
        {
            return true;
        }

        return false;
    }

    private bool ReachedMaxDistance()
    {
        return Vector3.Distance(transform.position, originalPosition) >= slamDistance;
    }

    private void OnHitGround()
    {
        isSlammingDown = false;
        isReturningUp = true;
        hasHitGround = true;
        Debug.Log("Hit ground! Returning up slowly...");

        // You can add impact effects here (screen shake, sound, particles)
    }

    private void OnReturnedToOriginal()
    {
        isReturningUp = false;
        Debug.Log("Returned to original position");

        // Optionally restart the slam after a delay
        Invoke("StartSlamming", 2f);
    }

    // Public method to manually trigger slam
    public void TriggerSlam()
    {
        if (!isSlammingDown && !isReturningUp)
        {
            StartSlamming();
        }
    }

    // Visual debug in Scene view
    private void OnDrawGizmosSelected()
    {
        if (boxCollider != null)
        {
            // Draw slam distance
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * slamDistance);
        }

        // Draw original position
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(originalPosition, Vector3.one * 0.5f);
        }
    }
}
