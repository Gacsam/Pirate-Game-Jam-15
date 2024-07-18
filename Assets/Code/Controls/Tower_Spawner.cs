using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Spawner : MonoBehaviour
{
    // minion prefabs
    public GameObject melePrefab;
    public GameObject rangePrefab;

    // cooldowns
    public float CD = 2f;
    private bool canSpawnMele = true;
    private bool canSpawnRange = true;


    // used on button
    public void SpawnMeleeMinion(){
        
        if(!canSpawnMele){return;}
        Instantiate(melePrefab, transform.position, Quaternion.identity);
        StartCoroutine(MeleCountdown());
    
    }

    public void SpawnRangedMinion(){

        if(!canSpawnRange){return;}
        Instantiate(rangePrefab, transform.position, Quaternion.identity);
        StartCoroutine(RangedCountdown());
    
    }


    // countdown
    IEnumerator MeleCountdown(){
        canSpawnMele = false;
        yield return new WaitForSeconds(CD);
        canSpawnMele = true;
    }

    IEnumerator RangedCountdown(){
        canSpawnRange = false;
        yield return new WaitForSeconds(CD);
        canSpawnRange = true;
    }


}
