using UnityEngine;
using System.Collections.Generic;

public class TestManager : MonoBehaviour
{
    [SerializeField]
    private BaseObject alchemyTower, shadowTower, alchemyUnit, shadowUnit;

    private void Start()
    {
        // Find the "spawnedUnits" field in GameMan.Alchemy
        var alchemyUnitsField = GameMan.Alchemy.GetType().GetField("spawnedUnits");
        Debug.Log("Added alchemy field");
        // Get the field value as a List<BaseObject>
        var alchemyUnits = (List<BaseObject>)alchemyUnitsField.GetValue(GameMan.Alchemy);
        Debug.Log("Got alchemy units");
        // add our test units to the list
        alchemyUnits.Add(alchemyTower);
        alchemyUnits.Insert(0, alchemyUnit);
        Debug.Log("Added alchemy units");
        // Repeat for shadow stuff
        var shadowyUnitsField = GameMan.Shadow.GetType().GetField("spawnedUnits");
        var shadowUnits = (List<BaseObject>)alchemyUnitsField.GetValue(GameMan.Shadow);
        shadowUnits.Add(shadowTower);
        shadowUnits.Insert(0, shadowUnit);
        Debug.Log("Added shadow unit");
    }


}
