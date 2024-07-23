using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    // Cloud movements
    // slow ----> fast
    private enum CloudDepth{none,far,medium,close}
    private enum CloudInitialDirection{left,right}

    [SerializeField]
    private CloudDepth cloudDepth;
    [SerializeField]
    private CloudInitialDirection cloudInitialDirection;

    private float startPosition;

    // Sin waves again !!! wohooo
    private float sin = 0f;
    private float wave;

    // left right swaying max displacement
    private float swayAmount = 5f;
    
    void Start() {
        startPosition = transform.position.x;
        if(cloudInitialDirection == CloudInitialDirection.left){sin=180f;}

        // further = smaller
        switch(GetEnumIndex(cloudDepth)){
            case 1:
                transform.localScale = new Vector3(0.5f,0.5f,1);
                break;
            case 3:
                transform.localScale = new Vector3(1.3f,1.3f,1);
                break;
            default:
                break;
        }
    }

    void Update() {
        if (cloudDepth == CloudDepth.none){return;}

        // calculate sin (/20 = x20 slower)
        sin = (sin + (GetEnumIndex(cloudDepth)*Time.deltaTime/20)) % 360;
        wave = Mathf.Sin(sin)*swayAmount;

        // "upload" to cloud :D
        transform.position = new Vector3(startPosition + wave,transform.position.y,10);

    }

    private int GetEnumIndex(CloudDepth depth)
    {
        return System.Array.IndexOf(System.Enum.GetValues(depth.GetType()), depth);
    }
}
