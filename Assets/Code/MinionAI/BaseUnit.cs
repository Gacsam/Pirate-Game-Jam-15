using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : BaseObject
{
    public float speed = 1;
    protected BaseObject closestTarget = null;
    protected float closestTargetDistance = 9999;
    public int range = 5;
    protected float attackTimer = 0;
    public float attackEveryX = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void moveTowardsOppositeTower() {
        this.gameObject.transform.position += (thisUnitSide == unitSide.player? Vector3.right : Vector3.left) * speed * Time.deltaTime;
    }
}