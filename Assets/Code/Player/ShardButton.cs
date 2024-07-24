using UnityEngine;
using UnityEngine.EventSystems;

public class ShardButton : MonoBehaviour, IPointerDownHandler
{
    // Drag and drop shards to upgrade minions
    public ElementType thisDamageType;
    public GameObject shardPrefab;
    private GameObject shard;

    private bool shardEquiped = false;

    public void OnPointerDown(PointerEventData eventData){
        
        // highlight all minion that can be upgraded
        foreach(var minion in GameObject.FindGameObjectsWithTag("player normal minion")){
            minion.GetComponent<UpgradeMinion>().HighlightForUpgrade();
        }

        // raycast method not working for some reason so spawn in a prefab (shard) and use on trigger enter on that object to detect collision with minions
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        shard = Instantiate(shardPrefab,(Vector3)mousePosition,Quaternion.identity);
        shard.GetComponent<Shard>().thisDamageType = thisDamageType;

        shardEquiped = true;

    }

    private void Update() {
        if(!shardEquiped){return;}

        if(Input.GetMouseButtonUp(0)){

            // minion highlight reset
            foreach(var minion in GameObject.FindGameObjectsWithTag("player normal minion")){
                minion.GetComponent<UpgradeMinion>().DisableHighlight();
            }

            shardEquiped = false;

        }

    }



    // scraped ... raycast 2d not working for some reason. Tried everything
    // private void LateUpdate() {
    //     if(!shardEquiped){return;}

    //     // mouse button released when equipting a shard = upgrade minion pointed
    //     if(Input.GetMouseButtonUp(0)){

    //         // raycast to determine minion to upgrade
    //         Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //         RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

    //         // hit a minion (UPGRADE TIME !!!)
    //         if (hit.collider != null){
    //             Debug.Log("hit!");
    //             GameObject minion = hit.collider.gameObject;
    //             if(minion.tag == "player normal minion"){minion.GetComponent<UpgradeMinion>().Upgrade(thisType);}
    //         }
    //         else{
    //             Debug.Log("Nothing to upgrade");
    //         }

    //         // minion highlight reset
    //         foreach(var minion in GameObject.FindGameObjectsWithTag("player normal minion")){
    //             minion.GetComponent<UpgradeMinion>().DisableHighlight();
    //         }

    //         shardEquiped = false;
    //     }
    // }

}
