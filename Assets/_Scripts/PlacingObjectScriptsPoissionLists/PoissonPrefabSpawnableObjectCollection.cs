using System.Collections.Generic;
using UnityEngine;

public abstract class PoissonPrefabSpawnableObjectCollection : ScriptableObject, IPoissonPrefabSpawnableObjectCollection
{
    public abstract string[] Names { get; }
    public abstract Material Material { get; }
    public abstract VisualStyleTypes VisualStyleType { get; }
    public abstract List<ArrayPrefabCollectableItemObjectBasic> SpawnableSmallElements { get; }

    public int GetRandomObjectIndex(int elementIndex)
    {
        return SpawnableSmallElements[elementIndex].GetRandomObjectIndex();
    }

    public int GetCountWeighted(int elementIndex)
    {
        return SpawnableSmallElements[elementIndex].GetWeightedElementsCount();
    }

    public GameObject GetObject(int elementIndex, int index)
    {
        return SpawnableSmallElements[elementIndex].GetGameObject(index);
    }

    public int GetPrevious(int elementIndex, int index)
    {
        return SpawnableSmallElements[elementIndex].GetPrevious(index);
    }

    public int GetNext(int elementIndex, int index)
    {
        return SpawnableSmallElements[elementIndex].GetNext(index);
    }
}