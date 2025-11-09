using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackHero : MonoBehaviour
{
    [Header("Input Action Reference")]
    public InputActionReference parryAction;

    [Header("Slap Related")]
    public float slapCooldown = 1.5f;
    public bool canSlap;

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
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        Collider2D hit = Physics2D.OverlapPoint(mouseWorldPosition);

        if (hit != null && hit is CapsuleCollider2D && !canSlap)
        {
            HeroSlapDetect HSD = hit.gameObject.GetComponent<HeroSlapDetect>();
            HSD.CalculateSlapLocation(mouseWorldPosition);

            StartCoroutine(cooldown());

            StartCoroutine(ImpactFrames());
        }
    }

    IEnumerator ImpactFrames()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
    }

    IEnumerator cooldown()
    {
        canSlap = true;
        yield return new WaitForSecondsRealtime(slapCooldown);
        canSlap = false;
    }
}
