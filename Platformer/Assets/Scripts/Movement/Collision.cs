using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    // This script will handle collisions with walls and grounds
    // Jumping, Wall Jumping, Sliding
    // It creates 3 circles. Bottom, Right, Left. Those are used to navigate and manage actions
 
    [Header("Layers")]
    public LayerMask groundLayer;
    [Space]

    // Used to handle mechanics
    [Header("Booleans")]
    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    [HideInInspector]
    public int wallSide;
    [Space]

    [Header("Collision")]
    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset;
    private Color debugCollisionColor = Color.red;

    private void Update()
    {
        // This part is used to create the circles on each side of the player except for the top
        // Then it checks if it overlaps with terrain. If so, booleans are true and we're colliding with a wall or ground.
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer)
            || Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);

        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        // Determine which side the player is in order to properly rotate the sprite
        wallSide = onRightWall ? -1 : 1;
    }

    // This is used to draw the gizmos to the screen
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        // Draws a sphere taking position + radius parameters
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }
}
