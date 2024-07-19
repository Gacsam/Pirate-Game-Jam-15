using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Spawner : MonoBehaviour
{
    // minion prefabs
    public GameObject melePrefab;

    // cooldowns
    public float CD = 2f;
    private bool canSpawnMele = true;

    // used on button
    public void SpawnMeleeMinion(){
        
        if(!canSpawnMele){return;}
        Instantiate(melePrefab, transform.position, Quaternion.identity);
        StartCoroutine(MeleCountdown());
    
    }

    // countdown
    IEnumerator MeleCountdown(){
        canSpawnMele = false;
        yield return new WaitForSeconds(CD);
        canSpawnMele = true;
    }

}
