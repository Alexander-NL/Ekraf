using System.Collections;
using TMPro;
using UnityEngine;

public class HeroRespawn : MonoBehaviour
{
    [Header("Dead related")]
    public int DeadCounter;
    public TextMeshProUGUI deadText;
    public bool Dead;
    public float respawnTimer = 1f;
    public MovementState earlyMovementState;
    public CapsuleCollider2D box2D;

    [Header("Object & Script Reference")]
    public GameObject respawnLocation;
    public HeroMovement heroMovement;

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

        BGMmanager.Instance.PlayerSfxSet("Dead");
        heroMovement.ReverseJump();

        yield return new WaitForSeconds(respawnTimer);
        Dead = false;
        box2D.enabled = true;

        heroMovement.CurrentState = earlyMovementState;
        this.transform.position = respawnLocation.transform.position;
    }
}
