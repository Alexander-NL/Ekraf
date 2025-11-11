using Unity.Cinemachine;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    public bool isTrap;

    [Header("Slam Settings")]
    [SerializeField] private float slamDownSpeed = 10f;
    [SerializeField] private float returnUpSpeed = 3f;
    [SerializeField] private float slamDistance = 5f;

    [Header("References")]
    [SerializeField] private BoxCollider2D boxCollider;
    public CinemachineImpulseSource impulseSource;

    private Vector3 originalPosition;
    private bool isSlammingDown = false;
    private bool isReturningUp = false;

    void Start()
    {
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();

        originalPosition = transform.position;

        if (isTrap)
        {
            StartSlamming();
        }
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
    }

    private void SlamDown()
    {
        transform.Translate(Vector3.down * slamDownSpeed * Time.deltaTime);

        if (ReachedMaxDistance())
        {
            OnHitGround();
        }
    }

    private void ReturnUp()
    {
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, returnUpSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, originalPosition) < 0.1f)
        {
            OnReturnedToOriginal();
        }
    }

    private bool ReachedMaxDistance()
    {
        return Vector3.Distance(transform.position, originalPosition) >= slamDistance;
    }

    private void OnHitGround()
    {
        isSlammingDown = false;
        isReturningUp = true;
        Debug.Log("Hit ground! Returning up slowly...");

        impulseSource.GenerateImpulse();
    }

    private void OnReturnedToOriginal()
    {
        isReturningUp = false;

        if (isTrap)
        {
            Invoke("StartSlamming", 2f);
        }
    }

    public void CloseDoor()
    {
        if (!isSlammingDown && !isReturningUp)
        {
            StartSlamming();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (boxCollider != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * slamDistance);
        }
    }
}
