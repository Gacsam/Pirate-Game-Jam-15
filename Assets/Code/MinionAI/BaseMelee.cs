using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMelee : BaseUnit
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(this.thisUnitSide == unitSide.player? Vector3.right: Vector3.left) * range;
        Gizmos.DrawRay(transform.position, direction);
    }

    // Update is called once per frame
    private void Update()
    {
        foreach (BaseObject currentObject in FindObjectsOfType<BaseObject>()){
            if (currentObject != null && currentObject != this && currentObject.thisUnitSide != this.thisUnitSide)
            {
                if (this.closestTarget == null)
                {
                    this.closestTarget = currentObject;
                }
                this.closestTargetDistance = Vector3.Distance(this.gameObject.transform.position, this.closestTarget.transform.position);
                if (currentObject != this.closestTarget)
                {
                    float distanceToObject = Vector3.Distance(this.gameObject.transform.position, currentObject.gameObject.transform.position);

                    if (distanceToObject < this.closestTargetDistance)
                    {
                        this.closestTarget = currentObject;
                    }
                }
            }
        }
        if (this.closestTargetDistance > range)
        {
            moveTowardsOppositeTower();
        }
        else
        {
            this.attackTimer += Time.deltaTime;
            if (this.attackTimer > this.attackEveryX)
            {
                this.attackTimer = 0;
                this.closestTarget.TakeDamage(1);
            }
        }
    }
}
