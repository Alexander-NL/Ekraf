using System.Collections;
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
    [SerializeField] float goUpDelay = 0.5f;
    [SerializeField] float delayBeforeSlam = 1f;

    [SerializeField] Vector3 originalPosition;
    [SerializeField] Vector3 slamTargetPosition;
    private bool isSlammingDown = false;
    private bool isReturningUp = false;

    void Start()
    {
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();

        originalPosition = transform.position;
        slamTargetPosition = originalPosition + Vector3.down * slamDistance;

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
        transform.position = Vector3.MoveTowards(transform.position, slamTargetPosition, slamDownSpeed * Time.deltaTime);

        if (transform.position == slamTargetPosition)
        {
            OnHitGround();
        }
    }

    private void ReturnUp()
    {
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, returnUpSpeed * Time.deltaTime);

        if (transform.position == originalPosition)
        {
            OnReturnedToOriginal();
        }
    }

    private void OnHitGround()
    {
        isSlammingDown = false;
        Debug.Log("Hit ground! Returning up slowly...");

        impulseSource.GenerateImpulse();

        StartCoroutine(DelayGoUp());
    }

    IEnumerator DelayGoUp()
    {
        yield return new WaitForSeconds(goUpDelay);
        isReturningUp = true;
    }

    private void OnReturnedToOriginal()
    {
        isReturningUp = false;

        if (isTrap)
        {
            Invoke("StartSlamming", delayBeforeSlam);
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
