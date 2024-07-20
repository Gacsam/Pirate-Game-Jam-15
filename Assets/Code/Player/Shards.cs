using UnityEngine;
using UnityEngine.EventSystems;

public class Shards : MonoBehaviour, IPointerDownHandler
{
    // Drag and drop shards to upgrade minions
    public damageType thisType;
    private bool shardsEquiped = false;

    public void OnPointerDown(PointerEventData eventData){
        
        // highlight all minion that can be upgraded
        foreach(var minion in GameObject.FindGameObjectsWithTag("player normal minion")){
            minion.GetComponent<UpgradeMinion>().HighlightForUpgrade();
        }

        shardsEquiped = true;

    }

    private void Update() {
        if(!shardsEquiped){return;}

        // mouse button released when equipting a shard = upgrade minion pointed
        if(Input.GetMouseButtonUp(0)){

            // raycast to determine minion to upgrade
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            
            // hit a minion (UPGRADE TIME !!!)
            if (hit.collider != null){
                GameObject minion = hit.collider.gameObject;
                if(minion.tag == "player normal minion"){minion.GetComponent<UpgradeMinion>().Upgrade(thisType);}
            }

            // minion highlight reset
            foreach(var minion in GameObject.FindGameObjectsWithTag("player normal minion")){
                minion.GetComponent<UpgradeMinion>().DisableHighlight();
            }

        }
    }

}
