using System.Collections.Generic;
using UnityEngine;

public interface IPoissonPrefabSpawnableObjectCollection
{
    abstract string[] Names { get; }
    abstract List<ArrayPrefabCollectableItemObjectBasic> SpawnableSmallElements { get; }

    int GetRandomObjectIndex(int elementIndex);

    int GetCountWeighted(int elementIndex);

    GameObject GetObject(int elementIndex, int index);

    int GetPrevious(int elementIndex, int index);

    int GetNext(int elementIndex, int index);
}