using UnityEngine;
using UnityEngine.InputSystem.HID;

public enum SlapState
{
    LowRight,
    HighRight,
    LowLeft,
    HighLeft,
    Middle,
    None
}

public class HeroSlapDetect : MonoBehaviour
{
    [Header("Script Reference")]
    public HeroMovement heroMovement;

    [Header("Enum related")]
    public SlapState whereSlap;
    public bool MidAirSlap;

    private Vector2 locationDifference;

    private void Start()
    {
        whereSlap = SlapState.None;
    }

    public void CalculateSlapLocation(Vector2 slapVector2)
    {
        if (MidAirSlap) return;
        Vector2 heroLocation = this.transform.position;

        locationDifference = heroLocation - slapVector2;

        //when jumping
        if (!heroMovement.isGrounded)
        {
            if (locationDifference.x < -0.3 && locationDifference.y > 0)
            {
                whereSlap = SlapState.LowRight;
            }
            else if (locationDifference.x > 0.3 && locationDifference.y > 0)
            {
                whereSlap = SlapState.LowLeft;
            }
            else if (locationDifference.x < 0.3 && locationDifference.x > -0.3 && locationDifference.y > 0)
            {
                whereSlap = SlapState.Middle;
            }
        }

        //When no jumping
        if (heroMovement.isGrounded)
        {
            if (locationDifference.x < 0)
            {
                whereSlap = SlapState.HighRight;
            }
            else if (locationDifference.x > 0)
            {
                whereSlap = SlapState.HighLeft;
            }
        }

        CheckWhatSlap();
    }

    public void CheckWhatSlap()
    {
        //if backshot Stop 1 sec
        //if di face slap turn around

        //Bitch slap
        if(heroMovement.CurrentState == MovementState.MovingRight && whereSlap == SlapState.HighRight)
        {
            heroMovement.CurrentState = MovementState.MovingLeft;
            return;
        }
        else if (heroMovement.CurrentState == MovementState.MovingLeft && whereSlap == SlapState.HighLeft)
        {
            heroMovement.CurrentState = MovementState.MovingRight;
            return;
        }

        //BackShot
        if (heroMovement.CurrentState == MovementState.MovingLeft && whereSlap == SlapState.HighRight)
        {
            StartCoroutine(heroMovement.HeroGotStunned());
            return;
        }
        else if(heroMovement.CurrentState == MovementState.MovingRight && whereSlap == SlapState.HighLeft)
        {
            StartCoroutine(heroMovement.HeroGotStunned());
            return;
        }

        //Low Left, Low Right
        if (heroMovement.CurrentState == MovementState.MovingRight && whereSlap == SlapState.LowRight && !heroMovement.isGrounded)
        {
            heroMovement.CurrentState = MovementState.MovingLeft;
            MidAirSlap = true;
            heroMovement.ReverseJump();
            return;
        }
        else if (heroMovement.CurrentState == MovementState.MovingLeft && whereSlap == SlapState.LowLeft && !heroMovement.isGrounded)
        {
            heroMovement.CurrentState = MovementState.MovingRight;
            MidAirSlap = true;
            heroMovement.ReverseJump();
            return;
        }
        else if (heroMovement.CurrentState == MovementState.MovingRight && whereSlap == SlapState.LowLeft && !heroMovement.isGrounded)
        {
            MidAirSlap = true;
            heroMovement.FastJump();
        }
        else if (heroMovement.CurrentState == MovementState.MovingLeft && whereSlap == SlapState.LowRight && !heroMovement.isGrounded)
        {
            MidAirSlap = true;
            heroMovement.FastJump();
        }

        //Double Jump
        if (whereSlap == SlapState.Middle && !heroMovement.isGrounded)
        {
            MidAirSlap = true;
            heroMovement.Jump();
            return;
        }
    }
}
