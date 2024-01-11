using System.Collections.Generic;
using UnityEngine;

public abstract class PoissonPrefabObjectCollectionScriptableObject : ScriptableObject, IPoissonPrefabObjectCollection
{
    public abstract string[] Names { get; }
    public abstract Material Material { get; }
    public abstract VisualStyleTypes VisualStyleType { get; }
    public abstract List<ArrayPrefabObjectBasic> Elements { get; }

    public ArrayPrefabObjectBasic Element
    {
        get { return Elements[0]; }
    }
    public int GetRandomObjectIndex(int elementIndex)
    {
        return Elements[elementIndex].GetRandomObjectIndex();
    }

    public int GetCountWeighted(int elementIndex)
    {
        return Elements[elementIndex].GetWeightedElementsCount();
    }

    public GameObject GetObject(int elementIndex, int index)
    {
        return Elements[elementIndex].GetGameObject(index);
    }

    public int GetPrevious(int elementIndex, int index)
    {
        return Elements[elementIndex].GetPrevious(index);
    }

    public int GetNext(int elementIndex, int index)
    {
        return Elements[elementIndex].GetNext(index);
    }
}