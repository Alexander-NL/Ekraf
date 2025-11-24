using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{
    [Header("Arrow Related")]
    [SerializeField] Transform arrowSpawnPoint;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] Transform hero;
    public ArrowBehaviour arrowScript;
    public GameObject arrowSpawn;

    [Header("Turret Stats")]
    public float detectRange = 10f;
    public float shootCooldown = 1.5f;
    public float aimSpeed = 8f;
    public int turretHP = 1;
    private float cooldown;
    public bool isDead;

    [Header("Vfx Related")]
    public GameObject vfxPrefab;
    public Transform vfxSpawnLocation;

    private GameObject _vfxPrefab;

    private void Start()
    {
        TurretManager.Instance.turretList.Add(this);
        GameObject temp = GameObject.FindWithTag("Hero");
        hero = temp.transform;
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

        BGMmanager.Instance.ArrowShoot();

        arrowSpawn = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        arrowScript = arrowSpawn.GetComponent<ArrowBehaviour>();
        arrowScript.turretBehaviour = this;

        TurretManager.Instance.arrowStash.Add(arrowSpawn);

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
            _vfxPrefab = Instantiate(vfxPrefab, vfxSpawnLocation);
            _vfxPrefab.transform.position = this.transform.position;

            isDead = true;
            BGMmanager.Instance.TurretDead();
            this.gameObject.SetActive(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
