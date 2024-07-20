using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUnit : BaseObject
{
<<<<<<< Updated upstream
    public static float distanceForMeleeCombat = 0.5f;
    public static float distanceForRangeCombat = 2f;
    public float attackRange = 1;
    public int baseDamage = 2;
=======
    public static float distanceForMeleeCombat = 1f;
>>>>>>> Stashed changes
    protected bool isPathBlocked = false;
    public float walkSpeed = 1;
    public float currencyReward = 1;
    public float unitCost = 10;

    public override void HandleCombat()
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
<<<<<<< Updated upstream

    protected bool CheckPath()
    {
        // Small if/else statement to check for player direction
        Vector3 direction = (this.thisUnitSide == unitSide.player ? Vector3.right : Vector3.left);
        // Get base collider's extents from centre, half a sprite
        float colliderSize = this.GetComponent<Collider2D>().bounds.extents.x;
        // Multiply direction by colliderSize to create a ray right at edge of character
        Vector3 offset = direction * colliderSize;
        // Create a raycast of X steps where you can't move further
        RaycastHit2D hit = Physics2D.Raycast(transform.position + offset, direction, distanceForMeleeCombat,~LayerMask.GetMask("Ignore Raycast"));
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
=======
>>>>>>> Stashed changes
    public abstract void FightMelee();
    public abstract void FightRanged();


    public override void MoveTowardsOppositeTower()
    {
        // Check if ally unit isn't blocking the path
        if (IsPathClear())
        {
            this.gameObject.transform.position += GetDirection() * walkSpeed * Time.deltaTime;
        }
    }

    protected bool IsPathClear()
    {
        // Multiply direction by horizontal offset to create a ray right at edge of character
        Vector3 offset = GetDirection() * this.GetComponent<Collider2D>().bounds.extents.x;
        // Create a raycast of X steps where you can't move further
        RaycastHit2D hit = Physics2D.Raycast(transform.position + offset, GetDirection(), distanceForMeleeCombat);
        // If there's nothing in front of us for X distance, say path blocked or nah
        return hit.collider == null;
    }
}