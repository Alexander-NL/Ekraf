using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite fallingSprite;
    [SerializeField] private Sprite impactSprite;

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
    [SerializeField] float delayBeforeSlam = 1.5f;

    [SerializeField] Vector3 originalPosition;
    [SerializeField] Vector3 slamTargetPosition;
    private bool isSlammingDown = false;
    private bool isReturningUp = false;

    void Start()
    {
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = normalSprite;

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
        spriteRenderer.sprite = fallingSprite;
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
        spriteRenderer.sprite = impactSprite;
        isSlammingDown = false;
        Debug.Log("Hit ground! Returning up slowly...");

        impulseSource.GenerateImpulse();

        StartCoroutine(DelayGoUp());
    }

    IEnumerator DelayGoUp()
    {
        yield return new WaitForSeconds(goUpDelay);
        spriteRenderer.sprite = normalSprite;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Hero")) return;
        if (!isSlammingDown) return;

        // check if door hits player from above
        ContactPoint2D contact = collision.GetContact(0);

        if (contact.normal.y > 0.5f)
        {
            HeroRespawn heroRespawn = collision.collider.GetComponent<HeroRespawn>();
            if (heroRespawn != null)
            {
                heroRespawn.HeroDeath();
            }

            Debug.Log("HERO CRUSHED BY DOOR!");
        }
        else
        {
            Debug.Log("Side collision (ignored).");
        }
    }


}
