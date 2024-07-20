using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRanged : BaseUnit
{
    // Start is called before the first frame update
    void Start()
    {
        // Ranged unit so range is equal to range
        attackRange = distanceForRangeCombat;
        this.thisUnitType = unitType.ranged;
    }

    public override void FightMelee()
    {
        // Add animations etc
        this.attackTimer += Time.deltaTime;
        if (this.attackTimer > this.attackEveryX)
        {
            if (closestTarget != null)
            {
                this.attackTimer = 0;
                // Deal less damage as the ranged unit is stuck in melee, reduce by -X? Halve it?
                this.closestTarget.TakeDamage(this.baseDamage / 2);
            }
        }
    }

    // This shouldn't be possible so throw a system error (fAnTaStIc suggestion VStudio)
    public override void FightRanged()
    {
        // Add animations etc
        this.attackTimer += Time.deltaTime;
        if (this.attackTimer > this.attackEveryX)
        {
            if (closestTarget != null)
            {
                this.attackTimer = 0;
                this.closestTarget.TakeDamage(this.baseDamage);
            }
        }
    }
}