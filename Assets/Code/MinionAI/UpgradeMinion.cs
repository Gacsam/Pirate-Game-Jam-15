using UnityEngine;

public class UpgradeMinion : MonoBehaviour
{
    public GameObject fireGuy;
    public GameObject arsenicGuy;
    public GameObject moonGuy;
    public GameObject boraxGuy;

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

    // pointer released when ready to upgrade (UPGADE TIME !!!)
    public void Upgrade(DamageType upgradeType){
        GameObject temp = GetComponent<GameObject>();
        if(upgradeType==DamageType.Fire){Instantiate(temp = fireGuy, transform.position,Quaternion.identity);}
        else if(upgradeType==DamageType.Borax){Instantiate(temp = boraxGuy, transform.position,Quaternion.identity);}
        else if(upgradeType==DamageType.Arsenic){Instantiate(temp = arsenicGuy, transform.position,Quaternion.identity);}
        else if(upgradeType==DamageType.Moon){Instantiate(temp = moonGuy,transform.position,Quaternion.identity);}
        if(GetComponent<BaseObject>().thisUnitSide == UnitSide.Alchemy)
        {
            GameMan.Alchemy.ClosestUnit = temp.GetComponent<BaseObject>();
        }
        else
        {
            GameMan.Alchemy.ClosestUnit = temp.GetComponent<BaseObject>();
        }
        Destroy(gameObject);
    }
}
