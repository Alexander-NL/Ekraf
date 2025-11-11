using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class HeroRespawn : MonoBehaviour
{
    [Header("Dead related")]
    public int DeadCounter;
    public bool Dead;
    public float respawnTimer = 1f;
    public MovementState earlyMovementState;
    public CapsuleCollider2D box2D;

    [Header("Object & Script Reference")]
    public GameObject respawnLocation;
    public HeroMovement heroMovement;

    public void Start()
    {
        earlyMovementState = heroMovement.CurrentState;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Arrow") || collision.collider.CompareTag("Spike"))
        {
            Debug.Log("Dead");
            DeadCounter++;
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        Dead = true;
        box2D.enabled = false;

        heroMovement.ReverseJump();

        yield return new WaitForSeconds(respawnTimer);
        Dead = false;
        box2D.enabled = true;

        heroMovement.CurrentState = earlyMovementState;
        this.transform.position = respawnLocation.transform.position;
    }
}
