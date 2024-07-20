using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Shard : MonoBehaviour
{

    private bool released = false;
    public damageType thisDamageType;

    private void Update() {

        // shard follows mouse
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
        
        // wait for release
        if(Input.GetMouseButtonUp(0)){
            released = true;
            StartCoroutine(DestroyNextFrame());
        }
        
    }

    void OnTriggerStay2D(Collider2D other){
        if(!released){return;}

        Debug.Log("colliding");

        // if released on a normal minion then upgrade
        if (other.gameObject.tag == "player normal minion"){
            other.gameObject.GetComponent<UpgradeMinion>().Upgrade(thisDamageType);
        }

    }

    // we want to wait 2 frame
    // onTrigger runs before Update, so after button is released we need to wait for next frame
    // Destroy runs before onTrigger so once again, we have to wait till next frame
    IEnumerator DestroyNextFrame(){
        yield return new WaitForNextFrameUnit();
        yield return new WaitForNextFrameUnit();
        Destroy(gameObject);
    }

}
