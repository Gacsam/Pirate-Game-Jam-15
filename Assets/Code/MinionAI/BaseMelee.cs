using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMelee : BaseUnit
{
    // Start is called before the first frame update
    void Start()
    {
        // Melee unit so range is equal to melee
        attackRange = distanceForMeleeCombat;
        this.thisUnitType = UnitType.Melee;
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
                GameMan.GetClosestEnemy(thisUnitSide).TakeDamage(this.baseDamage);
            }
        }
    }

    // This shouldn't be possible so throw a system error (FaNtAsTiC suggestion VStudio)
    public override void FightRanged()
    {
        throw new System.NotImplementedException();
    }

}