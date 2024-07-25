using Assets.Code.MinionAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonMinion : BaseMovingUnit, IRanged, IMelee
{
    [SerializeField]
    private GameObject projectileToSpawn;
    protected override void HandleDestruction()
    {
        if (thisUnitSide == UnitSide.Shadow)
        {
            var shard = Instantiate(Resources.Load<GameObject>("Prefabs/Items/Alchemy/Arsenic shard"), this.transform.position, Quaternion.identity);
            shard.transform.SetParent(null, false);
        }
        Destroy(gameObject);
    }


    [SerializeField]
    private float throwRange = 3;
    float IRanged.AttackRange { get { return throwRange; } set { } }

    void IRanged.Attack()
    {
        if (this.attackTimer == 0)
        {
            this.attackTimer += Time.deltaTime;
            if (GameMan.GetClosestEnemy(thisUnitSide).thisUnitType == UnitType.Tower)
            {
                MoveTowardsOppositeTower();
            }else{
            var offset = GetSpriteExtents() * GetEnemyTowerDirection().x * 2;
            var projectile = Instantiate(projectileToSpawn, transform.position + offset, Quaternion.identity);
            if (projectile.GetComponent<PoisonBomb>() == null) projectile.AddComponent<PoisonBomb>().Setup(GetEnemyTowerDirection(), thisUnitSide);
            else projectile.GetComponent<PoisonBomb>().Setup(GetEnemyTowerDirection(), thisUnitSide);
            ThrowProjectile(ref projectile);
                this.attackTimer += Time.deltaTime;
            }
        }
        else if (this.attackTimer >= 2)
        {
            this.attackTimer = 0;
        }
        else
        {
            this.attackTimer += Time.deltaTime;
        }
    }

    [SerializeField]
    float arcThrowHeight = 2;
    void ThrowProjectile(ref GameObject projectile)
    {
        var rb = projectile.GetComponent<Rigidbody2D>();
        if(rb != null)
        {
            var distanceToEnemy = GameMan.GetClosestEnemy(thisUnitSide).transform.position - transform.position;
            float distance = distanceToEnemy.magnitude;

            // Normalize direction to get the horizontal direction
            Vector2 directionNormalized = distanceToEnemy.normalized;

            // Calculate the time to reach the target
            float gravity = Physics2D.gravity.y;
            float time = Mathf.Sqrt(-2 * arcThrowHeight / gravity) + Mathf.Sqrt(2 * (distanceToEnemy.y - arcThrowHeight) / gravity);

            // Calculate horizontal and vertical components of the velocity
            float Vy = Mathf.Sqrt(-2 * gravity * arcThrowHeight);
            float Vx = distance / time;

            // Combine the horizontal and vertical components
            Vector2 result = directionNormalized * Vx;
            result.y = Vy;

            rb.velocity = result;
        }

    }

    void IMelee.Attack()
    {
        MockAttack();
    }
}