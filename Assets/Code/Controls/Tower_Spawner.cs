using System.Collections;
using UnityEngine;

public class Tower_Spawner : MonoBehaviour
{
    // minion prefabs
    public GameObject melePrefab;
    public int minionCost = 30;

    // cooldowns
    public float CD = 2f;
    private bool canSpawnMele = true;

    // player inventory reference
    private Inventory playerInventory;

    void Start() {
        playerInventory = GetComponent<Inventory>();

    }

    // used on button
    public void SpawnMeleeMinion(){
        
        if(!canSpawnMele){return;}
        if(playerInventory.gold < minionCost){return;}

        Instantiate(melePrefab, transform.position, Quaternion.identity);

        playerInventory.gold -= minionCost;
        playerInventory.UpdateGold();

        StartCoroutine(MeleCountdown());
    
    }

    // countdown
    IEnumerator MeleCountdown(){
        canSpawnMele = false;
        yield return new WaitForSeconds(CD);
        canSpawnMele = true;
    }

}
