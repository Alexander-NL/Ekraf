using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackHero : MonoBehaviour
{
    [Header("Input Action Reference")]
    public InputActionReference parryAction;
    public InputActionReference slapAction;

    [Header("Spirit related")]
    public GameObject Bird;
    public GameObject Weapon;
    public SpriteRenderer birdSpriteRenderer;
    public Animator WeaponControl;

    [Header("Slap Related")]
    public float slapCooldown = 1.5f;
    public bool canSlap;

    public float parryCooldown = 0.5f;
    public bool canParry;

    public GameObject particlePrefab;
    public Transform bird;
    private GameObject _currentParticle;

    public PlayerMiscScript playerMiscScript;
    public CinemachineImpulseSource impulseSource;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] bool _isTutorial;

    public Vector2 mouseWorldPosition;

    private HeroSlapDetect HSD;
    private ArrowBehaviour AB;

    private Coroutine _currentIEParticle;

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
        if (!_isTutorial)
        {
            TurnOnPlayerHelp();
        }
        Weapon.SetActive(false);
    }

    public void TurnOnPlayerHelp()
    {
        canSlap = true;
        canParry = true;
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

        RaycastHit2D[] hits = Physics2D.CircleCastAll(mouseWorldPosition, 0.5f, Vector2.zero, targetLayerMask);

        foreach (RaycastHit2D hit in hits)
        {
            //Check for Arrow
            if (hit.collider.tag == "Arrow" && canParry)
            {
                AB = hit.collider.GetComponent<ArrowBehaviour>();

                Weapon.SetActive(true); 
                AB.Parry();

                if(_currentIEParticle != null)
                {
                    StopCoroutine(_currentIEParticle);
                }

                _currentIEParticle = StartCoroutine(Particle());
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

        RaycastHit2D[] hits = Physics2D.CircleCastAll(mouseWorldPosition, 0.5f, Vector2.zero, targetLayerMask);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider is BoxCollider2D && canSlap && hit.collider.tag == "Hero")
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
        if(_currentParticle != null)
        {
            Destroy(_currentParticle);
        }

        _currentParticle = Instantiate(particlePrefab, bird);

        _currentParticle.transform.localPosition = new Vector2(0, 0);
        yield return new WaitForSeconds(1.5f);
        Destroy(_currentParticle);
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
        canParry = false;
        yield return new WaitForSecondsRealtime(parryCooldown);
        canParry = true;
    }

    IEnumerator Slapcooldown()
    {
        canSlap = false;
        birdSpriteRenderer.material.SetFloat("_GrayscaleAmount", 1f);

        float time = 0;
        while (time < slapCooldown)
        {
            float ratio = time / slapCooldown;
            float grayscale = 1f - ratio;
            birdSpriteRenderer.material.SetFloat("_GrayscaleAmount", grayscale);

            time += Time.unscaledDeltaTime;
            yield return null;
        }

        birdSpriteRenderer.material.SetFloat("_GrayscaleAmount", 0f);
        canSlap = true;
    }
}
