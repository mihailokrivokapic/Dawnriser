using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    // Combat logic enabled
    [Header("Combat Booleans")]
    [SerializeField]
    private bool combatEnabled;
    [SerializeField]
    private float inputTimer;

    // Attack parameters
    [SerializeField]
    private float attack1Radius;
    [SerializeField]
    float attack1Damage = 10f;
    private float[] attackDetails = new float[2];

    // Transforms
    [SerializeField]
    private Transform attack1HitBoxPos;
    [SerializeField]
    private LayerMask attackLayerMask;
    [SerializeField]
    private Vector3 offSet = new Vector3();

    // Keeping track of attacks and input
    private bool gotInput;
    private bool isAttacking;
    private bool isFirstAttack;

    private float lastInputTime = Mathf.NegativeInfinity;

    // References
    private Animator anim;
    [SerializeField]
    private Movement move;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnabled);
        move = GetComponent<Movement>();
    }

    private void Update()
    {
        CheckCombatInput();
        CheckAttacks();


        // We need to flip the Circle depending on players rotation
        if(move.side == 1)
        {
            offSet.x = 0.5f;
        }
        else
        {
            offSet.x = -0.5f;
        }
    }

    private void CheckCombatInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(combatEnabled)
            {
                gotInput = true;
                // Start measuring time as soon as we get a click
                lastInputTime = Time.time;
            }
        }
    }

    private void CheckAttacks()
    {
        if(gotInput)
        {
            if(!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                isFirstAttack = !isFirstAttack;
                anim.SetBool("attack1", true);
                anim.SetBool("firstAttack", isFirstAttack);
                anim.SetBool("attack1", isAttacking);
            }
        }

        if (Time.time >= lastInputTime + inputTimer)
        {
            gotInput = false;
        }
    }

    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(move.transform.position + offSet, attack1Radius, attackLayerMask);

        attackDetails[0] = attack1Damage;
        attackDetails[1] = transform.position.x;

        foreach (Collider2D collider in detectedObjects)
        {
            // Send message is used to call a specific function on an object without know what 
            // script it is
            collider.transform.SendMessage("Damage", attackDetails);

            // TODO: Particles
        }
    }

    private void FinishAttack1()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);
    }

    private void OnDrawGizmos()
    {
        // Use the offSet Vector to adjust the position of the attackRange
        Gizmos.DrawWireSphere(move.transform.position + offSet, attack1Radius);
    }

}
