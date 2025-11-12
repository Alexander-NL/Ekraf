using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnim : MonoBehaviour
{
    public Animator birdAnim;
    public GameObject Hero;

    public PlayerAttackHero PAH;
    public SpriteRenderer birdRenderer;
    public SpriteRenderer weaponRenderer;
    private int xDifference;

    void Update()
    {
        xDifference = GetXDifference(PAH.mouseWorldPosition, Hero.transform);

        if (xDifference > 0)
        {
            birdRenderer.flipX = false;
            weaponRenderer.flipX = false;
        }
        else if(xDifference < 0)
        {
            birdRenderer.flipX = true;
            weaponRenderer.flipX = true;
        }
    }

    public int GetXDifference(Vector2 player, Transform target)
    {
        if (target == null) return 0;

        if(player.x > target.position.x)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    public void randomizeDead()
    {
        int curr = Random.Range(0, 3);

        switch (curr)
        {
            case 0:
                birdAnim.SetTrigger("Dead1");
                Debug.Log("1");
                break;
            case 1:
                birdAnim.SetTrigger("Dead2");
                Debug.Log("2");
                break;
            case 2:
                birdAnim.SetTrigger("Dead3");
                Debug.Log("3");
                break;
        }
    }

    public void confused()
    {
        birdAnim.SetTrigger("Confused");
    }
}
