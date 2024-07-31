using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMinion: BaseMovingUnit, IMelee
{
    [SerializeField]
    private float pushForce = 10;
    // protected override void HandleDestruction()
    // {
    //     if (thisUnitSide == UnitSide.Shadow)
    //     {
    //         var shard = Instantiate(Resources.Load<GameObject>("Prefabs/Items/Alchemy/Fire shard"), this.transform.position, Quaternion.identity);
    //         shard.transform.SetParent(null, false);
    //     }
    //     Destroy(this.gameObject);
    // }

    [SerializeField]
    int knockbackEveryHitX = 3;
    int hitCounter = 0;
    void IMelee.Attack()
    {
        Debug.Log("fire attack");
        // Timer represents animations n all        
        this.attackTimer += Time.deltaTime;
        // When the attack animation is done
        if (this.attackTimer > this.attackCooldown)
        {
            // Double check the enemy unit still exist
            if (GameMan.GetClosestEnemy(thisUnitSide) != null)
            {
                // Reset the timer and damage closest enemy unit
                this.attackTimer = 0;
                if (DamageClosestEnemy())
                {
                    if (GameMan.GetClosestEnemy(thisUnitSide) is BaseMovingUnit enemyUnit)
                    {
                        if(hitCounter == 0)
                            enemyUnit.KnockbackEffect(GetEnemyTowerDirection() * pushForce / 2);
                        hitCounter++;
                    }
                }
                else
                {
                    if (GameMan.GetClosestEnemy(thisUnitSide) is BaseMovingUnit enemyUnit)
                    {
                        if (hitCounter == 0)
                            enemyUnit.KnockbackEffect(GetEnemyTowerDirection() * pushForce / 2);
                        hitCounter++;
                    }
                }

                if(hitCounter >= knockbackEveryHitX)
                {
                    hitCounter = 0;
                }
            }
        }
    }
}
