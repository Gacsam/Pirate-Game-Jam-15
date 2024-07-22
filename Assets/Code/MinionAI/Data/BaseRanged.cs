using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BaseRanged : BaseMovingUnit, IRanged
{
    private float attackRange = 3;
    public float AttackRange { get => attackRange; set => attackRange = value; }

    void IRanged.Attack()
    {
        MockAttack();
    }
    protected override void HandleDestruction()
    {
        // play sounds
        // play animations
        // instantiate vfx
        // destroy this unit at the very end
        Object.Destroy(this.gameObject);
    }
}
[CustomEditor(typeof(BaseRanged))]
public class RangedEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BaseRanged myComponent = (BaseRanged)target;

        // Display specific variables
        myComponent.AttackRange = EditorGUILayout.FloatField("Attack Range", myComponent.AttackRange);

        // Draw the default inspector for other properties
        DrawDefaultInspector();
    }
}