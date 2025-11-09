using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{
    [SerializeField] Transform arrowSpawnPoint;
    [SerializeField] GameObject arrowPrefab;

    [SerializeField] Transform hero;
    public ArrowBehaviour arrowScript;
    public GameObject arrowSpawn;

    public float detectRange = 10f;
    public float shootCooldown = 1.5f;
    public float aimSpeed = 8f;
    public int turretHP = 1;
    public float cooldown;


    void Start()
    {
        //var heroBody = GameObject.FindWithTag("Hero")?.transform;

    }

    void Update()
    {
        if (hero == null) return;

        cooldown -= Time.deltaTime;

        float dist = Vector2.Distance(transform.position, hero.position);
        if (dist <= detectRange)
        {
            AimAt(hero.position);
            if (cooldown <= 0f)
            {
                Shoot();
                cooldown = shootCooldown;
            }
        }
    }

    void AimAt(Vector3 targetPos)
    {
        Vector2 dir = (targetPos - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * aimSpeed);
    }

    void Shoot()
    {
        if (arrowPrefab == null || arrowSpawnPoint == null) return;

        arrowSpawn = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        arrowScript = arrowSpawn.GetComponent<ArrowBehaviour>();
        if (arrowScript != null)
        {
            arrowScript.Initialize(transform, hero.transform);
        }
    }

    public void ReceiveHit(int dmg)
    {
        Debug.Log("Turret get hit");
        turretHP -= dmg;

        if (turretHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
