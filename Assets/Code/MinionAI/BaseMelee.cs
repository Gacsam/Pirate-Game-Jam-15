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
                if (closestTarget == null)
                {
                    this.closestTarget = currentObject.gameObject;
                }
                closestTargetDistance = Vector3.Distance(this.gameObject.transform.position, this.closestTarget.transform.position);
                if (currentObject.gameObject != closestTarget)
                {
                    float distanceToObject = Vector3.Distance(this.gameObject.transform.position, currentObject.gameObject.transform.position);

                    if (distanceToObject < closestTargetDistance)
                    {
                        this.closestTarget = currentObject.gameObject;
                    }
                }
            }
        }
        if (this.closestTargetDistance > range)
        {
            moveTowardsOppositeTower();
        }
    }
}
