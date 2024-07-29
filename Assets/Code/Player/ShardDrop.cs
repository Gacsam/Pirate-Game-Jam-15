using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardDrop : MonoBehaviour
{

    private float duration = 0.7f;

    void Start() {
        StartCoroutine(DestroyCountdown());    
    }

    void Update()
    {
        transform.position += Vector3.up*Time.deltaTime*3;
    }

    IEnumerator DestroyCountdown(){
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
