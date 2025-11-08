using System.Collections;
using UnityEngine;

public enum MovementState
{
    MovingLeft,
    MovingRight,
    Idle
}

public class HeroMovement : MonoBehaviour
{
    [Header("Reference")]
    public Rigidbody2D rb;
    public MovementState CurrentState;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    public bool isGrounded;
    public bool canJump;

    public bool Randomized;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 10f;

    [Header("Raycast Settings")]
    [SerializeField] private float raycastDistance = 5f;
    [SerializeField] private LayerMask raycastLayerMask = 1;
    [SerializeField] private Color capsuleRayColor = Color.red;
    [SerializeField] private Color aboveRayColor = Color.blue;

    private RaycastHit2D capsuleHit;
    private RaycastHit2D aboveHit;

    void Start()
    {
        SetRandomState();
    }

    void Update()
    {
        if (CurrentState == MovementState.Idle) return;

        if (isGrounded)
        {
            PerformRaycasts();
            float direction = CurrentState == MovementState.MovingRight ? 1f : -1f;
            rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        }
    }

    private void PerformRaycasts()
    {
        Vector2 capsuleRayOrigin = transform.position;
        Vector2 aboveRayOrigin = (Vector2)transform.position + Vector2.up * 4f;

        Vector2 rayDirection = CurrentState == MovementState.MovingRight ? Vector2.right : Vector2.left;

        if (CurrentState == MovementState.MovingRight)
        {
            capsuleHit = Physics2D.Raycast(capsuleRayOrigin, Vector2.right, raycastDistance, raycastLayerMask);
            aboveHit = Physics2D.Raycast(aboveRayOrigin, Vector2.right, raycastDistance, raycastLayerMask);
        }
        else if (CurrentState == MovementState.MovingLeft)
        {
            capsuleHit = Physics2D.Raycast(capsuleRayOrigin, Vector2.left, raycastDistance, raycastLayerMask);
            aboveHit = Physics2D.Raycast(aboveRayOrigin, Vector2.left, raycastDistance, raycastLayerMask);
        }

        Debug.DrawRay(capsuleRayOrigin, rayDirection * raycastDistance, capsuleHit.collider ? Color.red : capsuleRayColor);
        Debug.DrawRay(aboveRayOrigin, rayDirection * raycastDistance, aboveHit.collider ? Color.red : aboveRayColor);

        if (capsuleHit.collider != null && aboveHit.collider == null && capsuleHit.collider.gameObject.tag == "Wall")
        {
            canJump = true;
            Jump();
            Debug.Log("Can jump");
        }

        if (aboveHit.collider != null && aboveHit.collider.gameObject.tag == "Wall")
        {
            canJump = false;
            Debug.Log("Cannot jump");
        }
    }

    private void SetRandomState()
    {
        if (!Randomized) return;
        CurrentState = Random.Range(0, 2) == 0 ? MovementState.MovingLeft : MovementState.MovingRight;
        Debug.Log($"Now {CurrentState}");
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        Debug.Log("Jumped!");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
        if (collision.gameObject.tag == "Wall" && !canJump)
        {
            StartCoroutine(ChangeDirection());
        }
    }

    IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(1);
        if (CurrentState == MovementState.MovingLeft)
        {
            CurrentState = MovementState.MovingRight;
        }
        else
        {
            CurrentState = MovementState.MovingLeft;
        }

        canJump = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}