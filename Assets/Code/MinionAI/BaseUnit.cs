using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUnit : BaseObject
{
    public static float distanceForMeleeCombat = 0.5f;
    public float attackRange = 1;
    public int baseDamage = 2;
    protected bool isPathBlocked = false;
    public float walkSpeed = 1;
    protected float attackTimer = 0;
    public float attackEveryX = 1;
    public float currencyReward = 1;

    protected override void FixedUpdate()
    {
        // Trigger base class first to get closest target
        base.FixedUpdate();
        if (closestTarget != null)
        {
            isPathBlocked = CheckPath();
        }
    }

    protected bool CheckPath()
    {
        // Small if/else statement to check for player direction
        Vector3 direction = (this.thisUnitSide == unitSide.player ? Vector3.right : Vector3.left);
        // Get base collider's extents from centre, half a sprite
        float colliderSize = this.GetComponent<Collider2D>().bounds.extents.x;
        // Multiply direction by colliderSize to create a ray right at edge of character
        Vector3 offset = direction * colliderSize;
        // Create a raycast of X steps where you can't move further
        RaycastHit2D hit = Physics2D.Raycast(transform.position + offset, direction, distanceForMeleeCombat);
        // If there's nothing in front of us for X distance, say path blocked or nah
        return hit.collider != null;
    }

    // Draw ray in editor to show distance
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // 1-liner if-else
        Vector3 direction = (this.thisUnitSide == unitSide.player ? Vector3.right : Vector3.left);
        float colliderSize = this.GetComponent<Collider2D>().bounds.extents.x;
        Vector3 offset = direction * colliderSize;
        Gizmos.DrawRay(transform.position + offset, direction * attackRange);
    }

    // Update is called once per frame
    void Update()
    {
        // If we have a target which should always be true
        if (closestTarget != null)
        {
            float spriteOffset = this.GetComponent<Collider2D>().bounds.extents.x + closestTarget.GetComponent<Collider2D>().bounds.extents.x;
            // Is the enemy within attack range
            if (closestTargetDistance <= attackRange)
            {
                // Is the unit forced into melee combat
                if (closestTargetDistance <= distanceForMeleeCombat)
                {
                    FightMelee();
                }
                else
                {
                    FightRanged();
                }
            }
            else
            {
                if (!isPathBlocked)
                {
                    moveTowardsOppositeTower();
                }
            }
        }
    }

    // Abstract class/variable basically means we want this method in every inherited script down the line but can call it from here, or something
    public abstract void FightMelee();
    public abstract void FightRanged();

    public void moveTowardsOppositeTower()
    {
        this.gameObject.transform.position += (thisUnitSide == unitSide.player ? Vector3.right : Vector3.left) * walkSpeed * Time.deltaTime;
    }
}