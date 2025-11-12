using UnityEngine;

public class HeroAnim : MonoBehaviour
{
    public SpriteRenderer Hero;
    public HeroMovement heroMovement;
    public Animator heroAnimator;
    
    void Update()
    {
        if(heroMovement.CurrentState == MovementState.MovingLeft)
        {
            Hero.flipX = false;
        }
        else if(heroMovement.CurrentState == MovementState.MovingRight)
        {
            Hero.flipX = true;
        }
    }

    public void DeadTrigger()
    {
        heroAnimator.SetBool("Walking", false);
        heroAnimator.SetBool("Running", false);
        heroAnimator.SetTrigger("Dead");
    }

    public void SlappedTrigger()
    {
        heroAnimator.SetTrigger("Slapped");
    }

    public void RunTrigger()
    {
        heroAnimator.SetBool("Running", true);
        heroAnimator.SetBool("Walking", false);
        heroAnimator.SetTrigger("Run");
    }

    public void WalkTrigger()
    {
        heroAnimator.SetBool("Walking", true);
        heroAnimator.SetBool("Running", false);
        heroAnimator.SetTrigger("Walk");
    }

    public void JumpTrigger()
    {
        heroAnimator.SetBool("Walking", false);
        heroAnimator.SetBool("Running", false);
        heroAnimator.SetTrigger("Jump");
    }

    public void IdleTrigger()
    {
        heroAnimator.SetBool("Walking", false);
        heroAnimator.SetBool("Running", false);
    }
}
