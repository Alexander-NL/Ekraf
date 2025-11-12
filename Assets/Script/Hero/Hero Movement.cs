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
    public PlayerAnim playerAnim;
    public HeroAnim heroAnim;
    public EventManager eventManager;

    [Header("Movement Settings")]
    public MovementState CurrentState;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float sprintSpeed = 5f;

    private float currSpeed;
    public bool isGrounded;
    public bool canJump;

    public bool huggingWall;

    public bool stunned;

    private float randomActionCooldown = 10f;
    public bool Randomized;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 10f;

    [Header("Raycast Settings")]
    [SerializeField] private float upperRay = 4f;
    [SerializeField] private float raycastDistance = 5f;
    [SerializeField] private float topRayOffset = 0.5f;
    [SerializeField] private float topRayDistance = 2f;

    [SerializeField] private LayerMask raycastLayerMask = 1;
    [SerializeField] private Color capsuleRayColor = Color.red;
    [SerializeField] private Color aboveRayColor = Color.blue;
    [SerializeField] private Color topRayColor = Color.cyan;

    private RaycastHit2D capsuleHit;
    private RaycastHit2D aboveHit;
    private RaycastHit2D topHitLeft;
    private RaycastHit2D topHitRight;

    [SerializeField] bool hitCeiling;

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
                StartCoroutine(Delay());

                randomActionCooldown = 10f;
            }

            float direction = CurrentState == MovementState.MovingRight ? 1f : -1f;
            rb.linearVelocity = new Vector2(direction * currSpeed, rb.linearVelocity.y);
        }
    }


    IEnumerator Delay()
    {
        BGMmanager.Instance.PlayerSfxSet("Alert");
        yield return new WaitForSeconds(1f);
        RandomizedAction();
    }

    public void RandomizedAction()
    {
        bool shouldJump = Random.Range(0, 2) == 0;

        playerAnim.confused();

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
        BGMmanager.Instance.PlayerSfxSet("Run");
        heroAnim.RunTrigger();

        yield return new WaitForSeconds(1f);

        BGMmanager.Instance.PlayerSfxSet("Walk");
        heroAnim.WalkTrigger();
        currSpeed = moveSpeed;
    }

    private void PerformRaycasts()
    {
        Vector2 capsuleRayOrigin = transform.position;
        Vector2 aboveRayOrigin = (Vector2)transform.position + Vector2.up * upperRay;

        Vector2 topRayOriginLeft = (Vector2)transform.position + new Vector2(-1f, topRayOffset);
        Vector2 topRayOriginRight = (Vector2)transform.position + new Vector2(1f, topRayOffset);

        Vector2 rayDirection = CurrentState == MovementState.MovingRight ? Vector2.right : Vector2.left;

        if (CurrentState == MovementState.MovingRight)
        {
            capsuleHit = Physics2D.Raycast(capsuleRayOrigin, Vector2.right, raycastDistance, raycastLayerMask);
            aboveHit = Physics2D.Raycast(aboveRayOrigin, Vector2.right, raycastDistance, raycastLayerMask);
            topHitLeft = Physics2D.Raycast(topRayOriginLeft, Vector2.up, topRayDistance, raycastLayerMask);
            topHitRight = Physics2D.Raycast(topRayOriginRight, Vector2.up, topRayDistance, raycastLayerMask);
        }
        else if (CurrentState == MovementState.MovingLeft)
        {
            capsuleHit = Physics2D.Raycast(capsuleRayOrigin, Vector2.left, raycastDistance, raycastLayerMask);
            aboveHit = Physics2D.Raycast(aboveRayOrigin, Vector2.left, raycastDistance, raycastLayerMask);
            topHitLeft = Physics2D.Raycast(topRayOriginLeft, Vector2.up, topRayDistance, raycastLayerMask);
            topHitRight = Physics2D.Raycast(topRayOriginRight, Vector2.up, topRayDistance, raycastLayerMask);
        }

        // Debug visualization
        Debug.DrawRay(capsuleRayOrigin, rayDirection * raycastDistance, capsuleHit.collider ? Color.red : capsuleRayColor);
        Debug.DrawRay(aboveRayOrigin, rayDirection * raycastDistance, aboveHit.collider ? Color.red : aboveRayColor);
        Debug.DrawRay(topRayOriginLeft, Vector2.up * topRayDistance, topHitLeft.collider ? Color.red : topRayColor);
        Debug.DrawRay(topRayOriginRight, Vector2.up * topRayDistance, topHitRight.collider ? Color.red : topRayColor);

        //Atap
        if ((topHitLeft.collider != null && 
            (topHitLeft.collider.gameObject.tag == "Ground" || topHitLeft.collider.gameObject.tag == "Wall" || topHitLeft.collider.gameObject.tag == "Spike")) 
            ||
            (topHitRight.collider != null && 
            (topHitRight.collider.gameObject.tag == "Ground" || topHitLeft.collider.gameObject.tag == "Wall" || topHitLeft.collider.gameObject.tag == "Spike")))
        {
            hitCeiling = true;
        }
        else
        {
            hitCeiling = false;
        }


        if (!hitCeiling && aboveHit.collider != null && aboveHit.collider.gameObject.tag == "Ground")
        {
            canJump = true;
            Jump();
        }

        if(!hitCeiling && capsuleHit.collider != null && (capsuleHit.collider.gameObject.tag == "Ground" || capsuleHit.collider.gameObject.tag == "Spike"))
        {
            canJump = true;
            Jump();
        }

        if (aboveHit.collider != null && aboveHit.collider.gameObject.tag == "Wall" && capsuleHit.collider != null && capsuleHit.collider.gameObject.tag == "Wall")
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
        if (!canJump)
        {
            canJump = true;
        }

        heroAnim.JumpTrigger();
        BGMmanager.Instance.PlayerSfxSet("Jump");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.75f, jumpForce);
    }

    public void FastJump()
    {
        heroAnim.JumpTrigger();
        BGMmanager.Instance.PlayerSfxSet("Jump");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x * 1.5f, jumpForce);
    }

    public void ReverseJump()
    {
        heroAnim.JumpTrigger();
        BGMmanager.Instance.PlayerSfxSet("Jump");
        rb.linearVelocity = new Vector2(-rb.linearVelocity.x * 1.5f, jumpForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Door")
        {
            heroAnim.IdleTrigger();
            eventManager.InvokeNextLevel();
            return;
        }
        if (collision.gameObject.tag == "Wall" && !canJump && isGrounded)
        {
            heroAnim.IdleTrigger();
            StartCoroutine(ChangeDirection());
        }

        if (collision.gameObject.tag == "Ground" && huggingWall)
        {
            isGrounded = true;
            canJump = false;
            heroAnim.IdleTrigger();
            StartCoroutine(ChangeDirection());
            return;
        }

        if (collision.gameObject.tag == "Ground")
        {
            BGMmanager.Instance.PlayerSfxSet("Walk");
            heroAnim.WalkTrigger();
            isGrounded = true;
            canJump = false;
            detect.MidAirSlap = false;
            return;
        }
        else if (collision.gameObject.tag == "Wall")
        {
            huggingWall = true;
            return;
        }
    }

    public IEnumerator HeroGotStunned()
    {
        stunned = true;
        heroAnim.IdleTrigger();
        yield return new WaitForSeconds(1);
        heroAnim.WalkTrigger();
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
        heroAnim.WalkTrigger();

        canJump = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
        if(collision.gameObject.tag == "Wall")
        {
            huggingWall = false;
        }
        else if (collision.gameObject.tag == "Wall" && isGrounded)
        {
            huggingWall = false;
            heroAnim.WalkTrigger();
        }
    }
}