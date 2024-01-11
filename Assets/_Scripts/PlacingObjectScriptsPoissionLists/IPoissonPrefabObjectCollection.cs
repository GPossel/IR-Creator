using System.Collections.Generic;
using UnityEngine;

public interface IPoissonPrefabObjectCollection
{
    abstract string[] Names { get; }
    abstract List<ArrayPrefabObjectBasic> Elements { get; }

    public ArrayPrefabObjectBasic Element { get; }
    int GetRandomObjectIndex(int elementIndex);

    int GetCountWeighted(int elementIndex);

    GameObject GetObject(int elementIndex, int index);

    int GetPrevious(int elementIndex, int index);

    int GetNext(int elementIndex, int index);
}