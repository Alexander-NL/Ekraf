using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    public float speed = 14f;
    public float returnSpeed = 20f;
    public int damage = 1;
    public float maxLifeTime = 8f;

    private Rigidbody2D rb;
    private Transform turret;
    private bool isReturning = false;
    private float lifeTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Transform turretTarget, Transform hero)
    {
        turret = turretTarget;
        Vector2 dir = ((Vector2)hero.position - (Vector2)transform.position).normalized;

        // set arrow rotation to face the hero
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // set velocity toward the hero
        rb.linearVelocity = dir * speed;
    }

    void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= maxLifeTime) Destroy(gameObject);

        if (isReturning && turret != null)
        {
            Vector2 dir = ((Vector2)turret.position - (Vector2)transform.position).normalized;
            rb.linearVelocity = dir * returnSpeed;
        }

        // Rotate to match travel direction
        if (rb.linearVelocity.sqrMagnitude > 0.001f)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isReturning)
        {
            if (collision.collider.CompareTag("Hero"))
            {
                Debug.Log("Hero get hit");
                // Hero takes damage
                Destroy(gameObject);
            }
            else if (collision.collider.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (collision.collider.CompareTag("Turret"))
            {
                // Damage turret
                var turretBehaviour = collision.collider.GetComponent<TurretBehaviour>();
                if (turretBehaviour != null)
                    turretBehaviour.ReceiveHit(damage);

                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void Parry()
    {
        if (isReturning || turret == null) return;

        isReturning = true;
        lifeTimer = 0f;

        Vector2 dir = ((Vector2)turret.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = dir * returnSpeed;
    }
}
