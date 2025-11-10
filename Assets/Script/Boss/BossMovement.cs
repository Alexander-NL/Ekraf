using System.Collections;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public float deathAnimDelay = 5f;
    public bool isDead;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    public Transform Left;
    public Transform Right;

    private bool movingToRight = true;

    void Update()
    {
        if (Left == null || Right == null || isDead) return;

        Vector3 targetPosition = movingToRight ? Right.position : Left.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            movingToRight = !movingToRight;
        }
    }

    public void BossDead()
    {
        StartCoroutine(bossDeadAnimation());
    }

    IEnumerator bossDeadAnimation()
    {
        isDead = true;
        yield return new WaitForSeconds(deathAnimDelay);
        Destroy(gameObject);
    }
}
