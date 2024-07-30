using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BaseMeleeRanged : BaseMovingUnit, IMelee, IRanged
{
    private float attackRange = 2;
    public float AttackRange { get => attackRange; set => attackRange = value; }

    void IRanged.Attack()
    {
        MockAttack();
    }

    void IMelee.Attack()
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

// [CustomEditor(typeof(BaseMeleeRanged))]
// public class MeleeRangedEditor : Editor
// {
//     public override void OnInspectorGUI()
//     {
//         BaseMeleeRanged myComponent = (BaseMeleeRanged)target;

//         // Display specific variables
//         myComponent.AttackRange = EditorGUILayout.FloatField("Attack Range", myComponent.AttackRange);

//         // Draw the default inspector for other properties
//         DrawDefaultInspector();
//     }
// }
