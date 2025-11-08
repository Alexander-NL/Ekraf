using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackHero : MonoBehaviour
{
    public InputActionReference parryAction;

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

    void Start()
    {
        
    }

    private void GetMouseLocation(InputAction.CallbackContext context)
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        Collider2D hit = Physics2D.OverlapPoint(mouseWorldPosition);

        if (hit != null && hit is CapsuleCollider2D)
        {
            Debug.Log($"Hit capsule: {hit.gameObject.name}");
        }
    }

    void Update()
    {
        
    }
}
