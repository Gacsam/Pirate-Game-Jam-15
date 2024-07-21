using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public abstract class BaseObject : MonoBehaviour
{
    public float thisUnitHealth = 10;
    public UnitSide thisUnitSide = 0;
    public UnitType thisUnitType = 0;
    protected BaseObject closestTarget = null;
    protected float closestTargetDistance = 9999;
    public float attackRange = 3;
    protected float attackTimer = 0;
    public float attackEveryX = 1;
    public int baseDamage = 2;

    // Add Box Collider if it doesn't have it whenever script is added to object
    public void OnValidate()
    {
        if (this.GetComponent<Collider2D>() == null)
        {
            this.AddComponent<BoxCollider2D>();
        }
    }
    public void Update()
    {
        // If we have a target which should always be true
        if (closestTarget != null)
        {
            // Is the enemy within attack range
            if (IsTargetWithinRange())
            {
                HandleCombat();
            }
            else
            {
                MoveTowardsOppositeTower();
            }
        }
    }

    // Every physics frame, refresh the closest enemy
    public void FixedUpdate()
    {
        if (GameMan.Instance != null)
        {
            closestTarget = GameMan.GetClosestEnemy(thisUnitSide);
        }
    }

    // Abstract class/variable basically means we want this method in every inherited script down the line, or something
    // Allows us to call the functions all the way from down here without having to run some checks twice etc
    public abstract void HandleCombat();
    // Towers will just do nothing in MoveTowardsOppositeTower, unless we get to some scary stuff I guess
    public abstract void MoveTowardsOppositeTower();

    // Check range to target
    public bool IsTargetWithinRange()
    {
        var spriteOffsets = closestTarget.GetSpriteExtents() + this.GetSpriteExtents(); // we use sprite offsets to get true distance between units
        closestTargetDistance = Mathf.Abs(closestTarget.transform.position.x - this.transform.position.x) - spriteOffsets;
        return closestTargetDistance <= attackRange;
    }

    public void TakeDamage(float damage, DamageType typeOfDamage = 0)
    {
        if (typeOfDamage == DamageType.standard)
        {
            thisUnitHealth -= damage;
        }
        else if (typeOfDamage == DamageType.fire) { }
        else return; // ignore other damage types temporarily

        if (this.thisUnitHealth <= 0)
        {
            HandleDeath();
        }
    }

    // virtual to allow override
    protected virtual void HandleDeath()
    {
        // play sounds
        // play animations
        // instantiate vfx

        // destroy this unit at the very end
        if (this.thisUnitType == 0)
        {
            // if it's the tower send a win/lose message
        }
        else {
            if(thisUnitSide == UnitSide.Alchemy)
            {
                GameMan.Alchemy.UnitDied();
            }
            else
            {
                GameMan.Shadow.UnitDied();
            }

        // if enemy && (melee || ranged)
        if (thisUnitSide == UnitSide.shadow && (thisUnitType == UnitType.Melee || thisUnitType == UnitType.Ranged))
            {
                ItemDrop.DropItem();
            }
        }
        Object.Destroy(this.gameObject);
    }

    public Vector3 GetDirection()
    {
        // Small if/else statement to check for player direction
        return thisUnitSide == UnitSide.Alchemy ? Vector3.right : Vector3.left;
    }   

    public float GetSpriteExtents()
    {
        return this.GetComponent<Collider2D>().bounds.extents.x;
    }
    // Draw ray in editor to show distance
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var offset = GetDirection() * this.GetSpriteExtents();
        Gizmos.DrawRay(transform.position + offset, GetDirection() * attackRange);
    }
}
public enum DamageType { standard, fire, arsenic, moon, borax }
public enum UnitSide { Alchemy, shadow }
public enum UnitType { tower, Melee, Ranged, siege }