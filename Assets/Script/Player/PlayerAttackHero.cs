using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackHero : MonoBehaviour
{
    [Header("Input Action Reference")]
    public InputActionReference parryAction;

    [Header("Slap Related")]
    public float slapCooldown = 1.5f;
    public bool canSlap;

    public PlayerMiscScript playerMiscScript;
    public CinemachineImpulseSource impulseSource;

    private Vector2 mouseWorldPosition;

    private void OnEnable()
    {
        parryAction.action.Enable();
        parryAction.action.performed += GetMouseLocation;
    }

    private void OnDisable()
    {
        parryAction.action.performed -= GetMouseLocation;
        parryAction.action.Disable();
    }

    private void GetMouseLocation(InputAction.CallbackContext context)
    {
        if (playerMiscScript.paused) return;
        Debug.Log("Clicking");
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        Collider2D hit = Physics2D.OverlapPoint(mouseWorldPosition);


        //Check for Hero
        if (hit != null && hit is CapsuleCollider2D && !canSlap)
        {
            HeroSlapDetect HSD = hit.gameObject.GetComponent<HeroSlapDetect>();

            if (HSD.MidAirSlap) return;
            BGMmanager.Instance.PlayerSlap("Slap");
            BGMmanager.Instance.PlayerSfxSet("Slap");
            HSD.CalculateSlapLocation(mouseWorldPosition);

            impulseSource.GenerateImpulse();

            StartCoroutine(cooldown());
            StartCoroutine(ImpactFrames());
            return;
        }

        //Check for Arrow
        if (hit != null && hit.tag == "Arrow" && !canSlap)
        {
            ArrowBehaviour AB = hit.GetComponent<ArrowBehaviour>();

            impulseSource.GenerateImpulse();
            BGMmanager.Instance.PlayerSlap("Parry");

            AB.Parry();
            StartCoroutine(ImpactFrames());
            return;
        }
    }

    IEnumerator ImpactFrames()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.5f);

        if (!playerMiscScript.paused)
        {
            Time.timeScale = 1f;
        }
    }

    IEnumerator cooldown()
    {
        canSlap = true;
        yield return new WaitForSecondsRealtime(slapCooldown);
        canSlap = false;
    }
}
