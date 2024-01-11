using System;
using UnityEngine;

[CreateAssetMenu(menuName = "World/SpawnWorldInformationScriptableObject")]
public class SpawnWorldInformationScriptableObject : ScriptableObject
{
    [SerializeField] private GameObject prefabOfWorld;

    public GameObject GetPrefabOfPlane() => prefabOfWorld;
}