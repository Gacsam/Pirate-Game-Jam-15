using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUnit : BaseObject
{
    public static float distanceForRangeCombat = 2f;
    public static float distanceForMeleeCombat = 1f;
    protected bool isPathBlocked = false;
    public float walkSpeed = 1;
    public float currencyReward = 1;
    public float unitCost = 10;

    public void Awake()
    {
        if(thisUnitSide == UnitSide.Alchemy)
        {
            GameMan.Alchemy.AddUnit(this);
        }
        else
        {
            GameMan.Shadow.AddUnit(this);
        }
    }

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

    // Draw ray (editor-exclusive) to show distance
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // 1-liner if-else
        float colliderSize = this.GetComponent<Collider2D>().bounds.extents.x;
        Vector3 offset = GetDirection() * colliderSize;
        Gizmos.DrawRay(transform.position + offset, GetDirection() * attackRange);
    }

    // Abstract class/variable basically means we want this method in every inherited script down the line but can call it from here, or something
    public abstract void FightMelee();
    public abstract void FightRanged();


    public override void MoveTowardsOppositeTower()
    {
        // Check if ally unit isn't blocking the path
        if (IsPathClear())
        {
            this.gameObject.transform.position += walkSpeed * Time.deltaTime * GetDirection();
        }
    }

    protected bool IsPathClear()
    {
        // Multiply direction by horizontal offset to create a ray right at edge of character
        Vector3 offset = GetDirection() * this.GetComponent<Collider2D>().bounds.extents.x;
        // Create a raycast of X steps where you can't move further
        RaycastHit2D hit = Physics2D.Raycast(transform.position + offset, GetDirection(), distanceForMeleeCombat, ~LayerMask.GetMask("Ignore Raycast"));
        // If there's nothing in front of us for X distance, say path blocked or nah
        return hit.collider == null;
    }
}