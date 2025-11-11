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
    public HeroRespawn heroRespawn;
    public HeroSlapDetect detect;

    [Header("Movement Settings")]
    public MovementState CurrentState;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float sprintSpeed = 5f;

    private float currSpeed;
    public bool isGrounded;
    public bool canJump;

    public bool stunned;

    private float randomActionCooldown = 10f;
    public bool Randomized;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 10f;

    [Header("Raycast Settings")]
    [SerializeField] private float upperRay = 4f;
    [SerializeField] private float raycastDistance = 5f;
    [SerializeField] private LayerMask raycastLayerMask = 1;
    [SerializeField] private Color capsuleRayColor = Color.red;
    [SerializeField] private Color aboveRayColor = Color.blue;

    private RaycastHit2D capsuleHit;
    private RaycastHit2D aboveHit;

    void Start()
    {
        currSpeed = moveSpeed;
        SetRandomState();
    }

    void Update()
    {
        if (CurrentState == MovementState.Idle || stunned || heroRespawn.Dead) return;

        if (isGrounded)
        {
            PerformRaycasts();

            randomActionCooldown -= Time.deltaTime;
            if (randomActionCooldown <= 0f)
            {
                RandomizedAction();

                randomActionCooldown = 10f;
            }

            float direction = CurrentState == MovementState.MovingRight ? 1f : -1f;
            rb.linearVelocity = new Vector2(direction * currSpeed, rb.linearVelocity.y);
        }
    }

    public void RandomizedAction()
    {
        bool shouldJump = Random.Range(0, 2) == 0;

        if (shouldJump)
        {
            Jump();
        }
        else
        {
            StartCoroutine(OneSecondAction());
        }
    }

    private IEnumerator OneSecondAction()
    {
        currSpeed = sprintSpeed;

        yield return new WaitForSeconds(1f);

        currSpeed = moveSpeed;
    }

    private void PerformRaycasts()
    {
        Vector2 capsuleRayOrigin = transform.position;
        Vector2 aboveRayOrigin = (Vector2)transform.position + Vector2.up * upperRay;

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
        }

        if (aboveHit.collider != null && aboveHit.collider.gameObject.tag == "Wall")
        {
            canJump = false;
        }
    }

    private void SetRandomState()
    {
        if (!Randomized) return;
        CurrentState = Random.Range(0, 2) == 0 ? MovementState.MovingLeft : MovementState.MovingRight;
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.75f, jumpForce);
    }

    public void FastJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x * 1.5f, jumpForce);
    }

    public void ReverseJump()
    {
        rb.linearVelocity = new Vector2(-rb.linearVelocity.x * 1.5f, jumpForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            detect.MidAirSlap = false;
            return;
        }
        if (collision.gameObject.tag == "Wall" && !canJump && isGrounded)
        {
            StartCoroutine(ChangeDirection());
        }
    }

    public IEnumerator HeroGotStunned()
    {
        stunned = true;
        yield return new WaitForSeconds(1);
        stunned = false;
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