using Assets.Code.MinionAI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MinionRanged : BaseMovingUnit, IRanged
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
    protected override void HandleDestruction()
    {
        // play sounds
        // play animations
        // instantiate vfx
        // destroy this unit at the very end
        Object.Destroy(this.gameObject);
    }

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
            var projectile = Instantiate(projectileToSpawn, transform.position + GetEnemyTowerDirection(), Quaternion.identity);
            var isPiercing = true; // we can do some checks here later
            // create a baseprojectile component if it doesn't exist
            if (projectile.GetComponent<BaseProjectile>() == null) projectile.AddComponent<BaseProjectile>().Setup(GetEnemyTowerDirection(), thisUnitSide, baseDamage, isPiercing);
            else projectile.GetComponent<BaseProjectile>().Setup(GetEnemyTowerDirection(), thisUnitSide);
        }

        this.attackTimer += Time.deltaTime;
    }
}
[CustomEditor(typeof(MinionRanged))]
public class RangedEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MinionRanged myComponent = (MinionRanged)target;

        // Display specific variables
        myComponent.AttackRange = EditorGUILayout.FloatField("Attack Range", myComponent.AttackRange);

        // Draw the default inspector for other properties
        DrawDefaultInspector();
    }
}