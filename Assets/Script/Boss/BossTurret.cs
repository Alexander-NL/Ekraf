using UnityEngine;

public class BossTurret : MonoBehaviour
{
    [Header("Arrow stuff")]
    [SerializeField] Transform arrowSpawnPoint;
    [SerializeField] Transform arrowEndPoint;
    [SerializeField] GameObject arrowPrefab;
    public ArrowBehaviour arrowScript;
    public GameObject arrowSpawn;

    [Header("Turret related")]
    public BossMovement bossMovement;
    public float detectRange = 10f;
    public float shootCooldown = 1.5f;
    public int maxHP = 5;
    public int turretHP;
    public float cooldown;

    [SerializeField] private float shootDistance = 10f;

    private void Start()
    {
        turretHP = maxHP;
    }

    void Update()
    {
        if(bossMovement.isDead) return;
        cooldown -= Time.deltaTime;
        if (cooldown <= 0f)
        {
            Shoot();
            cooldown = shootCooldown;
        }
    }

    void Shoot()
    {
        if (arrowPrefab == null || arrowSpawnPoint == null) return;

        Vector3 startPos = arrowSpawnPoint.position;
        Vector3 endPos = startPos + Vector3.down * shootDistance;

        Quaternion downwardRotation = Quaternion.Euler(0, 0, -90f);

        arrowSpawn = Instantiate(arrowPrefab, arrowSpawnPoint.position, downwardRotation);
        arrowScript = arrowSpawn.GetComponent<ArrowBehaviour>();
        if (arrowScript != null)
        {
            arrowScript.Initialize(arrowSpawnPoint, arrowEndPoint);
        }
    }

    public void ReceiveHit(int dmg)
    {
        Debug.Log("Turret get hit");
        turretHP -= dmg;

        if (turretHP <= 0)
        {
            bossMovement.BossDead();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(arrowSpawnPoint.position, Vector2.down * 2f);
    }
}
