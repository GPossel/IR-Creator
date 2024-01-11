using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerInventoryTotalSO))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {

        PlayerInventoryTotalSO myScript = (PlayerInventoryTotalSO)target;
        if (GUILayout.Button("Empty collection"))
        {
            myScript.EmptyTotalCollection();
        }
        DrawDefaultInspector();
    }
}