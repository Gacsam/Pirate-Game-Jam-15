using Assets.Code.MinionAI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RangedMinion : BaseMovingUnit, IRanged
{
    // This will allow us to decide what projectile the ranged minion uses, perhaps shadow uses a bolt instead?
    [SerializeField]
    private GameObject projectileToSpawn;
    private float attackRange = 3;
    public float AttackRange { get => attackRange; set => attackRange = value; }

    void IRanged.Attack()
    {
        Shoot();
    }
    // protected override void HandleDestruction()
    // {
    //     // play sounds
    //     // play animations
    //     // instantiate vfx
    //     // destroy this unit at the very end
    //     if (thisUnitSide == UnitSide.Shadow)
    //     {
    //         var shard = Instantiate(Resources.Load<GameObject>("Prefabs/Items/Alchemy/Moon shard"), this.transform.position, Quaternion.identity);
    //         shard.transform.SetParent(null, false);
    //     }
    //     Object.Destroy(this.gameObject);
    // }

    protected void Shoot()
    {
        // Timer represents animations n all
        // When the attack animation is done
        if (this.attackTimer > this.attackCooldown)
        {
            // Reset the timer and damage closest enemy unit
            this.attackTimer = 0;
        }
        if (attackTimer == 0)
        {
            var offset = GetSpriteExtents();
            offset.x *= GetEnemyTowerDirection().x;
            var projectile = Instantiate(projectileToSpawn, transform.position + offset, Quaternion.identity);
            // create a baseprojectile component if it doesn't exist
            if (projectile.TryGetComponent<StraightProjectile>(out var projectileComponent))
                projectileComponent.Setup(GameMan.GetClosestEnemy(thisUnitSide).transform.position + offset, thisUnitSide);
            else
                projectile.AddComponent<StraightProjectile>().Setup(GameMan.GetClosestEnemy(thisUnitSide).transform.position + offset, thisUnitSide);
        }

        this.attackTimer += Time.deltaTime;
    }
}
// [CustomEditor(typeof(RangedMinion))]
// public class RangedEditor : Editor
// {
//     public override void OnInspectorGUI()
//     {
//         RangedMinion myComponent = (RangedMinion)target;

//         // Display specific variables
//         myComponent.AttackRange = EditorGUILayout.FloatField("Attack Range", myComponent.AttackRange);

//         // Draw the default inspector for other properties
//         DrawDefaultInspector();
//     }
// }