using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    // This script is used to handle animations

    // Script References
    [SerializeField]
    private Animator animator;
    private Movement movement;
    private Collision collision;
    [HideInInspector]
    public SpriteRenderer sr;

    private void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<Movement>();
        collision = GetComponent<Collision>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        animator.SetBool("onGround", collision.onGround);
    }

    // Exstension method
    public void SetTrigger(string trigger)
    {
        animator.SetTrigger(trigger);
    }

    public void SetHorizontalMovement(float x, float y, float yVel)
    {
        animator.SetFloat("HorizontalAxis", x);
        animator.SetFloat("VerticalAxis", y);
        animator.SetFloat("VerticalVelocity", yVel);
    }

    public void Flip(int side)
    {
        if(movement.wallGrab || movement.wallSlide)
        {
            if (side == -1 && sr.flipX)
                return;
            if(side == 1 && !sr.flipX)
                return;

        }

        bool state = (side == 1) ? false : true;
        sr.flipX = state;
    }
}
