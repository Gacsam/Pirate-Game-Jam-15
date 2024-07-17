using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : BaseObject
{
    public float speed = 1;
    public GameObject closestTarget = null;
    public float closestTargetDistance = 9999;
    public int range = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void moveTowardsOppositeTower() {
        if (thisUnitSide == unitSide.player)
        {
            this.gameObject.transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else
        {
            this.gameObject.transform.position += Vector3.left * speed * Time.deltaTime;
        }
    }
}