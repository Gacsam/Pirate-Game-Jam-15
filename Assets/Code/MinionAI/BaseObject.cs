using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class BaseObject : MonoBehaviour
{
    public float thisUnitHealth = 10;
    public unitSide thisUnitSide = 0;
    public unitType thisUnitType = 0;
    protected BaseObject closestTarget = null;
    protected float closestTargetDistance = 9999;


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

    protected virtual void FixedUpdate()
    {
        FindClosestTarget();
    }

    protected float GetSpriteHorizontalOffset()
    {
        Vector3 direction = (this.thisUnitSide == unitSide.player ? Vector3.right : Vector3.left);
        float colliderSize = this.GetComponent<Collider2D>().bounds.extents.x;
        return (direction * colliderSize).x;
    }

    public void FindClosestTarget()
    {
        // Get all objects of BaseObject class or inheriting BaseObject class
        BaseObject[] allObjects = FindObjectsOfType<BaseObject>();
        // Zoom through all these objects
        foreach (BaseObject currentObject in allObjects)
        {
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
                Debug.Log(this.closestTargetDistance);
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
        }
    }
}

public enum damageType { standard, fire, water, air, earth }
public enum unitSide { player, shadow }
public enum unitType { tower, melee, ranged, siege }