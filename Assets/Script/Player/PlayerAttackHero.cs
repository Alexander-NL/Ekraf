using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.ParticleSystem;

public class PlayerAttackHero : MonoBehaviour
{
    [Header("Input Action Reference")]
    public InputActionReference parryAction;
    public InputActionReference slapAction;

    [Header("Spirit related")]
    public GameObject Bird;
    public GameObject Weapon;
    public Animator WeaponControl;

    [Header("Slap Related")]
    public float slapCooldown = 1.5f;
    public bool canSlap;

    public float parryCooldown = 1.5f;
    public bool canParry;

    public GameObject particle;

    public PlayerMiscScript playerMiscScript;
    public CinemachineImpulseSource impulseSource;
    [SerializeField] private LayerMask targetLayerMask;

    public Vector2 mouseWorldPosition;

    private HeroSlapDetect HSD;
    private ArrowBehaviour AB;

    private void OnEnable()
    {
        parryAction.action.Enable();
        slapAction.action.Enable();
        parryAction.action.performed += ParryGetMouseLocation;
        slapAction.action.performed += SlapGetMouseLocation;
    }

    private void OnDisable()
    {
        slapAction.action.performed -= SlapGetMouseLocation;
        parryAction.action.performed -= ParryGetMouseLocation;
        parryAction.action.Disable();
        slapAction.action.Disable();
    }

    private void Start()
    {
        Cursor.visible = false;
        Weapon.SetActive(false);
    }

    public void Update()
    {
        if (playerMiscScript.paused) return;
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        Weapon.transform.position = mouseWorldPosition;
        Bird.transform.position = mouseWorldPosition;
    }

    private void ParryGetMouseLocation(InputAction.CallbackContext context)
    {
        if (playerMiscScript.paused) return;
        Debug.Log("Clicking");

        RaycastHit2D[] hits = Physics2D.CircleCastAll(mouseWorldPosition, 0.5f, Vector2.zero, targetLayerMask);

        foreach (RaycastHit2D hit in hits)
        {
            //Check for Arrow
            if (hit.collider.tag == "Arrow" && !canSlap)
            {
                AB = hit.collider.GetComponent<ArrowBehaviour>();

                Weapon.SetActive(true); 
                AB.Parry();

                StartCoroutine(Particle());
                WeaponControl.SetTrigger("Parry");
                impulseSource.GenerateImpulse();
                BGMmanager.Instance.PlayerSlap("Parry");

                StartCoroutine(Parrycooldown());
                StartCoroutine(ParryImpactFrames());
                return;
            }
        }  
    }

    private void SlapGetMouseLocation(InputAction.CallbackContext context)
    {
        if (playerMiscScript.paused) return;
        Debug.Log("Clicking");

        RaycastHit2D[] hits = Physics2D.CircleCastAll(mouseWorldPosition, 0.5f, Vector2.zero, targetLayerMask);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider is CapsuleCollider2D && !canSlap && hit.collider.tag == "Hero")
            {
                HSD = hit.collider.gameObject.GetComponent<HeroSlapDetect>();

                if (HSD.MidAirSlap) return;
                BGMmanager.Instance.PlayerSlap("Slap");
                BGMmanager.Instance.PlayerSfxSet("Slap");

                Weapon.SetActive(true);
                WeaponControl.SetTrigger("Slap");

                impulseSource.GenerateImpulse();

                StartCoroutine(Slapcooldown());
                StartCoroutine(SlapImpactFrames());
                return;
            }
        }
    }

    IEnumerator Particle()
    {
        particle.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        particle.SetActive(false);
    }

    IEnumerator SlapImpactFrames()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        HSD.CalculateSlapLocation(mouseWorldPosition);

        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.5f);

        if (!playerMiscScript.paused)
        {
            Time.timeScale = 1f;
        }

        yield return new WaitForSecondsRealtime(0.5f);
        Weapon.SetActive(false);
    }

    IEnumerator ParryImpactFrames()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.5f);

        if (!playerMiscScript.paused)
        {
            Time.timeScale = 1f;
        }

        yield return new WaitForSecondsRealtime(0.5f);
        Weapon.SetActive(false);
    }

    IEnumerator Parrycooldown()
    {
        canParry = true;
        yield return new WaitForSecondsRealtime(parryCooldown);
        canParry = false;
    }

    IEnumerator Slapcooldown()
    {
        canSlap = true;
        yield return new WaitForSecondsRealtime(slapCooldown);
        canSlap = false;
    }
}
