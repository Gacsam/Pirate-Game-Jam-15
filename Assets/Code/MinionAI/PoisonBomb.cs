using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PoisonBomb : BaseProjectile
{
    private void Awake()
    {
        if(cloudEffect == null)
        {
            // Missing cloud gameobject
            throw new System.NotImplementedException();
        }            
    }
        

    [SerializeField]
    private float poisonRange = 5f;
    [SerializeField]
    GameObject cloudEffect;
    protected override void ProjectileImpact(GameObject thingThatWasHit)
    {
        var poisonSphere = Physics2D.OverlapCircleAll(transform.position + enemyTowerDirection, poisonRange);
        if (poisonSphere.Length > 0)
        {
            foreach (var hitUnit in poisonSphere)
            {
                if (hitUnit.gameObject.GetComponent<BaseUnit>() is BaseUnit theUnit)
                {
                    if (theUnit.thisUnitSide != allySide)
                    {
                        theUnit.ApplyPoison();
                    }
                }
            }
            StartCoroutine(PoisonCloud());
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }
    [SerializeField]
    float cloudTimer = 1;
    float currentTimer = 0;
    IEnumerator PoisonCloud() {
        var cloud = Instantiate(cloudEffect, this.transform);
        var alphaOverTimer = cloud.GetComponent<SpriteRenderer>().color.a / cloudTimer;
        // loop forever until ELSE
        while (true)
        {
            // make a bit more transparent
            cloud.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, alphaOverTimer * Time.deltaTime);
            foreach (SpriteRenderer miniCloud in cloud.GetComponentsInChildren<SpriteRenderer>()){
                miniCloud.color -= new Color(0, 0, 0, alphaOverTimer * Time.deltaTime); 
            }
            cloud.transform.position += Vector3.down * Time.deltaTime / 2;
            if (currentTimer <= cloudTimer)
            {
                currentTimer += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            else
            {
                break;
            }
        }
        Destroy(gameObject);
    }
}
