using UnityEngine;

public class UpgradeMinion : MonoBehaviour
{
    private bool readyForUpgrade = false;

    // sin waves for fading opacity etc ...
    private float wave = 0;
    private float waveSpeed = 0.01f;
    private float sin;

    // called when a shard is clicked
    public void HighlightForUpgrade(){
        readyForUpgrade=true;
    }

    public void DisableHighlight(){
        readyForUpgrade = false;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1,1);
    }

    private void Update() {
        if(!readyForUpgrade){return;}

        // color / opacity change to indicate that the unit is ready for an upgrade (using sin waves)

        // wave will be in the 0 - 360 range = 0 -> 1 -> -1 -> 0 (one full cycle)
        wave = (wave+waveSpeed)%360;
        // wave + 90 so sin will start from 1 -> -1 -> 1 (one full cycle)
        // devide 2 + 0.5 = 1 -> 0 -> 1 (one full cycle). reducing amplitude and applying y offset
        sin = (Mathf.Sin(wave+90)/2)+0.5f;

        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1,sin);
    }

    // moved to transformer minion
}
