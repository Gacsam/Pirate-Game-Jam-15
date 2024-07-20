using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public abstract class BaseObject : MonoBehaviour
{
    public float thisUnitHealth = 10;
    public unitSide thisUnitSide = 0;
    public unitType thisUnitType = 0;
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

    public void TakeDamage(float damage, damageType typeOfDamage = 0)
    {
        if (typeOfDamage == damageType.standard)
        {
            thisUnitHealth -= damage;
        }
        else if (typeOfDamage == damageType.fire) { }
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

        // if enemy && (melee || ranged)
        else if (thisUnitSide == unitSide.shadow  && (thisUnitType == unitType.melee || thisUnitType == unitType.ranged))
        {
            ItemDrop.DropItem();
        }
        Object.Destroy(this.gameObject);
    }

    // Every physics frame, refresh the closest enemy
    public void FixedUpdate()
    {
        if (GameMan.Instance != null)
        {
<<<<<<< Updated upstream
            // If the current-loop object exits, and it's not self, and it's not same side
            if (currentObject != null && currentObject != this && currentObject.thisUnitSide != this.thisUnitSide)
            {
                // If we don't have a target yet, set whatever is found first, then loop again
                if (this.closestTarget == null)
                {
                    this.closestTarget = currentObject;
                    continue;
                }
                // Update the distance to current closest target for comparison
                this.closestTargetDistance = Mathf.Abs((this.gameObject.transform.position.x + this.GetSpriteHorizontalOffset()) - (this.closestTarget.transform.position.x + this.closestTarget.GetSpriteHorizontalOffset()));
                // Debug.Log(this.closestTargetDistance);
                // If the closestTarget is the target of the loop, loop again
                if (currentObject == this.closestTarget) continue;
                // Get the distance to the target of the loop
                float distanceToObject = Mathf.Abs((this.gameObject.transform.position.x + this.GetSpriteHorizontalOffset()) - (currentObject.gameObject.transform.position.x + currentObject.GetSpriteHorizontalOffset()));
                // And compare it to distance of existing closest target, whichever's closest becomes the closestTarget
                if (distanceToObject < this.closestTargetDistance)
                {
                    this.closestTarget = currentObject;
                }
            }
=======
            closestTarget = GameMan.GetClosestEnemy(thisUnitSide);
>>>>>>> Stashed changes
        }
    }

    public Vector3 GetDirection()
    {
        // Small if/else statement to check for player direction
        return thisUnitSide == unitSide.alchemy ? Vector3.right : Vector3.left;
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

<<<<<<< Updated upstream
public enum damageType { standard, fire, arsenic, moon, borax }
public enum unitSide { player, shadow }
=======
public enum damageType { standard, fire, water, air, earth }
public enum unitSide { alchemy, shadow }
>>>>>>> Stashed changes
public enum unitType { tower, melee, ranged, siege }