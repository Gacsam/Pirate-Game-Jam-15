using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

public class TestManager : MonoBehaviour
{
    [SerializeField]
    private List<BaseObject> newAlchemyUnits, newShadowUnits;

    private void Start()
    {
        // Find the "spawnedUnits" field in GameMan.Alchemy
        var alchemyUnitsField = GameMan.Alchemy.GetType().GetField("spawnedUnits");
        Debug.Log("Added alchemy field");
        // Get the field value as a List<BaseObject>
        var gamemanAlchemyUnits = (List<BaseObject>)alchemyUnitsField.GetValue(GameMan.Alchemy);
        Debug.Log("Got alchemy units");
        // add our test units to the list
        gamemanAlchemyUnits.AddRange(newAlchemyUnits);
        Debug.Log("Added alchemy units");
        // Repeat for shadow stuff
        var shadowUnitsField = GameMan.Shadow.GetType().GetField("spawnedUnits");
        var gamemanShadowUnits = (List<BaseObject>)shadowUnitsField.GetValue(GameMan.Shadow);
        gamemanShadowUnits.AddRange(newShadowUnits);
    }
}
