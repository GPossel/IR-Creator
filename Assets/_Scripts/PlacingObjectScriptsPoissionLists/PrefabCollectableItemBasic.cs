using System;
using UnityEngine;

[Serializable]
public class PrefabCollectableItemBasic
{
    public GameObject Object;
    public int Weight;

    internal object CleanMeUp()
    {
        throw new NotImplementedException();
    }
}