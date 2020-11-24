using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PolyColliderSnapper))]
public class PolySnapperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PolyColliderSnapper Psnapper = (PolyColliderSnapper)target;
        if (GUILayout.Button("Snap Points"))
        {
            Psnapper.Snap();
        }
    }
}
