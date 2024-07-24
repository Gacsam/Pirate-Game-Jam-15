using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Shard : MonoBehaviour
{

    private bool released = false;
    public ElementType thisDamageType;

    private void Update() {

        if (Input.GetMouseButton(0))
        {
            // shard follows mouse
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;
        }
        
        // wait for release
        if(Input.GetMouseButtonUp(0)){
            released = true;
            StartCoroutine(DestroyNextFrame());
        }
        
    }

    void OnTriggerStay2D(Collider2D other){
        if(!released){return;}

        // if released on a normal minion then upgrade
        if (other.gameObject.tag == "player normal minion"){
            other.gameObject.GetComponent<TransformerMinion>().Upgrade(thisDamageType);
        }

    }

    IEnumerator DestroyNextFrame(){
        yield return new WaitForSeconds(0.1f);

        Destroy(gameObject);
    }

}
