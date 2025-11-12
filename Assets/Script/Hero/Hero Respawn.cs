using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class HeroRespawn : MonoBehaviour
{
    [Header("Dead related")]
    public int DeadCounter = 0;
    public TextMeshProUGUI deadText;
    public bool Dead;
    public float respawnTimer = 1f;
    public MovementState earlyMovementState;
    public CapsuleCollider2D box2D;

    [Header("Ui related")]
    public Animator Skull;
    public Animator Box;

    [Header("Object & Script Reference")]
    public GameObject respawnLocation;
    public HeroMovement heroMovement;
    public HeroAnim heroAnim;
    public PlayerAnim playerAnim;
    public TurretManager turretManager;
    public VFx_DamageFlash vfxdamage;
    public GameObject particle;

    public void Start()
    {
        deadText.text = DeadCounter.ToString();
        earlyMovementState = heroMovement.CurrentState;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Arrow") || collision.collider.CompareTag("Spike"))
        {
            Debug.Log("Dead");
            DeadCounter++;

            vfxdamage.CallDamageFlash();
            if (turretManager != null)
            {
                turretManager.RefreshTurret();
            }

            deadText.text = DeadCounter.ToString();
            StartCoroutine(Respawn());
        }
    }

    public void Retry()
    {
        DeadCounter = 0;
        deadText.text = DeadCounter.ToString();
        heroMovement.CurrentState = earlyMovementState;
        this.transform.position = respawnLocation.transform.position;
    }

    IEnumerator Respawn()
    {
        Dead = true;
        box2D.enabled = false;

        Skull.SetTrigger("Dead");
        Box.SetTrigger("Dead");

        playerAnim.randomizeDead();

        heroMovement.ReverseJump();
        BGMmanager.Instance.PlayerSfxSet("Dead");
        heroAnim.DeadTrigger();

        particle.SetActive(true);
        yield return new WaitForSeconds(respawnTimer);
        particle.SetActive(false);

        Dead = false;
        box2D.enabled = true;

        heroMovement.CurrentState = earlyMovementState;
        this.transform.position = respawnLocation.transform.position;
    }
}
