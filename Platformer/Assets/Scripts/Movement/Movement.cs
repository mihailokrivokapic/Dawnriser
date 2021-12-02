using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Movement : MonoBehaviour
{

    // Movement variables
    [Header("Stats")]
    public float moveSpeed = 5f;
    private float horizontal;
    private float vertical;
    private float horizontalRaw;
    private float verticalRaw;
    public float jumpForce = 10f;
    public float dashSpeed = 20;
    public float slideSpeed = 5f;
    public float wallJumpLerp = 10f;

    [Space]
    [Header("Booleans")]
    public bool canMove = true;
    public bool wallGrab;
    public bool hasDashed;
    public bool wallJumped;
    public bool isDashing;
    public bool wallSlide;
    public bool groundTouch;
    public bool longFall;
    [Space]

    // Player Components
    [HideInInspector]
    public Rigidbody2D body;

    // Handle animation logic by referencing collsion
    public AnimationHandler anim;
    public ParticleSystem pSystem;
    
    // Handle movement logic by referencing collision
    public Collision col;
    
    // Used for sprite 
    public int side = 1;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<AnimationHandler>();
    }

    private void Update()
    {
        // Gets the input from the keyboard   
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        horizontalRaw = Input.GetAxisRaw("Horizontal");
        verticalRaw = Input.GetAxisRaw("Vertical");

        // Assign those values to the direction
        Vector2 direction = new Vector2(horizontal, vertical);

        Walk(direction);
        anim.SetHorizontalMovement(Mathf.Abs(horizontal), vertical, body.velocity.y);

        if (col.onWall && Input.GetButton("Fire3") && canMove)
        {
            // Handle the sprite rotation to be facing correctly
            if (side != col.wallSide)
                anim.Flip(side * -1);
            // Set these true because we're on the wall
            wallGrab = true;
            wallSlide = false;
        }

        // 
        if (Input.GetButtonUp("Fire3") || !col.onWall || !canMove)
        {
            wallGrab = false;
            wallSlide = false;
        }

        // If we're on the ground and we're not dashing enable the better jump component
        if (col.onGround && !isDashing)
        {
            wallJumped = false;
            GetComponent<Jump>().enabled = true;
        }

        // Wall sliding
        if (wallGrab && !isDashing)
        {
            body.gravityScale = 0;
            if (horizontal > .2f || horizontal < -.2f)
                body.velocity = new Vector2(body.velocity.x, 0);

            float speedModifier = vertical > 0 ? .5f : 1f;

            body.velocity = new Vector2(body.velocity.x, vertical * (moveSpeed * speedModifier));
        }
        else
        {
            body.gravityScale = 3;
        }

        if (col.onWall && !col.onGround)
        {
            if (horizontal != 0 && !wallGrab)
            {
                wallSlide = true;
                WallSlide();
            }
        }

        if (!col.onWall || col.onGround)
            wallSlide = false;

        // JUMPING
        if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("isJumping");
            UnityEngine.Debug.Log("Jumped");

            if (col.onGround)
                Jump(Vector2.up, false);
            // If we're on the wall and not on the ground we want to wall jump    
            if (col.onWall && !col.onGround)
                WallJump();
        }

        if(!col.onGround && body.velocity.y > 0.5f)
        {
            longFall = true;
        }

        if (Input.GetButtonDown("Fire1") && !hasDashed)
        {
            if (horizontalRaw != 0 || verticalRaw != 0)
                Dash(horizontalRaw, verticalRaw);
        }

        if (col.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if (!col.onGround && groundTouch)
        {
            groundTouch = false;
        }

        // WallParticles

        if (wallGrab || wallSlide || !canMove)
            return;

        if (horizontal > 0)
        {
            side = 1;
            anim.Flip(side);
        }
        if (horizontal < 0)
        {
            side = -1;
            anim.Flip(side);
        }
    }

    public void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;

        side = anim.sr.flipX ? -1 : 1;

        pSystem.Play();
        // UnityEngine.Debug.Log(pSystem.isPlaying);
    }

    public void Walk(Vector2 dir)
    {
        // We don't want to capture any movement if the player is stunned or rooted.
        if (!canMove)
            return;

        if (wallGrab)
            return;

        if (!wallJumped)
        {
            body.velocity = new Vector2(dir.x * moveSpeed, body.velocity.y);
        }
        else
        {
            body.velocity = Vector2.Lerp(body.velocity, (new Vector2(dir.x * moveSpeed, body.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
    }

    // TODO: Make smoother jumping
    private void Jump(Vector2 dir, bool wall)
    {
        // TODO: Particles

        body.velocity = new Vector2(body.velocity.x, 0);
        body.velocity += dir * jumpForce;

        // TODO: Enable particles
    }

    // Disable the movement to handle wall jumps and mechanics related to it
    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (col.onGround)
            hasDashed = false;
    }

    private void RigidBodyDrag(float x)
    {
        body.drag = x;
    }

    /// <summary>
    /// Handles player dashing.
    /// </summary>
    /// <param name="x">Direction on the x-axis</param>
    /// <param name="y">Direction on the y-axis</param>
    private void Dash(float x, float y)
    {
        // Camera.main.transform.DOComplete();
        Camera.main.transform.DOComplete();
        Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
        FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));

        hasDashed = true;

        // TODO: // Animation trigger

        body.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x, y);

        // Moves the player
        body.velocity += dir.normalized * dashSpeed;
        StartCoroutine(DashWait());
    }

    IEnumerator DashWait()
    {
        // TODO: // Animation
        FindObjectOfType<GhostTrail>().ShowGhost();
        StartCoroutine(GroundDash());
        DOVirtual.Float(14, 0, .8f, RigidBodyDrag);

        body.gravityScale = 0;
        GetComponent<Jump>().enabled = false;
        wallJumped = true;
        isDashing = true;

        yield return new WaitForSeconds(.3f);

        body.gravityScale = 3;
        GetComponent<Jump>().enabled = true;
        wallJumped = false;
        isDashing = false;
    }

    private void WallJump()
    {
        if ((side == 1 && col.onRightWall) || side == -1 && !col.onRightWall)
        {
            side *= -1;
            // anim.Flip(side);
        }

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = col.onRightWall ? Vector2.left : Vector2.right;

        Jump((Vector2.up / 1.5f + wallDir / 1.5f), true);

        wallJumped = true;
    }

    private void WallSlide()
    {
        if (col.wallSide != side)
            // anim.Flip(side * -1);

            if (!canMove)
                return;

        bool pushingWall = false;
        if ((body.velocity.x > 0 && col.onRightWall) || (body.velocity.x < 0 && col.onLeftWall))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : body.velocity.x;

        body.velocity = new Vector2(push, -slideSpeed);
    }

    public int GetSide()
    {
        return side;
    }

    /* private void WallParticle(float vertical)
    {   
        var main = sideParticle.main;

        if(wallSlide || (wallGrab && vertical < 0))
        {
            slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
            main.startColor = Color.White();
        }
        else
        {
            main.startColor = Color.clear;
        }
    }
    */

    private int ParticleSide()
    {
        int particleSide = col.onRightWall ? 1 : -1;
        return particleSide;
    }
}
