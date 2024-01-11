using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ArrayPrefabCollectableItemObjectBasic
{
    [SerializeField]
    private List<PrefabCollectableItemBasic> _collectableObjects;

    [SerializeField]
    [HideInInspector]
    private int _totalWeight;

    [SerializeField]
    private bool _allowNull;

    public List<PrefabCollectableItemBasic> Objects
    {
        get { return _collectableObjects; }
        set { _collectableObjects = value; }
    }

    public int TotalWeight { get { return _totalWeight; } }

    public bool AllowNull { get { return _allowNull; } }

    public ArrayPrefabCollectableItemObjectBasic(bool allowNull)
    {
        _allowNull = allowNull;
        _totalWeight = 0;
    }

    public void RecalcTotalWeight()
    {
        Debug.Assert(Objects != null, "[ArrayPrefabCollectableItemObjectBasic: RecalcTotalWeight] -> Method called when array is null");
        _totalWeight = 0;
        foreach (PrefabCollectableItemBasic obj in Objects)
        {
            if (obj.Weight > 0 && (obj.Object != null || _allowNull))
            {
                _totalWeight += obj.Weight;
            }
        }
    }

    public bool HasElements()
    {
        return Objects != null && Objects.Count > 0;
    }

    public bool HasWeightedElements()
    {
        return Objects != null && TotalWeight > 0;
    }

    public bool HasWeightedElementsNonNull()
    {
        return Objects != null && TotalWeight > 0 && (!AllowNull || Objects.Any((o) => o.Object != null && o.Weight > 0));
    }

    public float GetChance(PrefabCollectableItemBasic obj)
    {
        return obj.Weight * (100.0f / TotalWeight);
    }
    public int GetWeightedElementsCount()
    {
        Debug.Assert(Objects != null, "[ArrayPrefabCollectableItemObjectBasic: GetWeightedElementsCount] -> Method called when array is null");

        int count = 0;
        foreach (PrefabCollectableItemBasic obj in Objects)
        {
            if (obj.Weight > 0 && (obj.Object != null || _allowNull))
            {
                ++count;
            }
        }
        return count;
    }

    public int GetWeightedElementsCountNonNull()
    {
        Debug.Assert(Objects != null, "[ArrayPrefabCollectableItemObjectBasic: GetWeightedElementsCount] -> Method called when array is null");

        int count = 0;
        foreach (PrefabCollectableItemBasic obj in Objects)
        {
            if (obj.Weight > 0 && obj.Object != null)
            {
                ++count;
            }
        }
        return count;
    }

    public int GetPrevious(int index)
    {
        Debug.Assert(Objects != null, "[ArrayPrefabCollectableItemObjectBasic: GetPrevious] -> Method called when array is null");
        int i = index;
        do
        {
            --i;
            if (i == -1)
            {
                i = Objects.Count - 1;
            }
        } while (index != i && !_allowNull && Objects[i].Object == null);
        return index;
    }

    public int GetNext(int index)
    {
        Debug.Assert(Objects != null, "[ArrayPrefabCollectableItemObjectBasic: GetNext] -> Method called when array is null");
        int i = index;
        do
        {
            i = (i + 1) % Objects.Count;
        } while (index != i && !_allowNull && Objects[index].Object == null);
        return index;
    }

    public int GetRandomObjectIndex()
    {
        Debug.Assert(Objects != null, "[ArrayPrefabCollectableItemObjectBasic: GetRandomObjectIndex] -> Method called when array is null");

        int num = UnityEngine.Random.Range(1, TotalWeight + 1);
        for (int i = 0; i < Objects.Count; ++i)
        {
            if (Objects[i].Object == null && !_allowNull)
            {
                continue;
            }
            num -= Objects[i].Weight;
            if (num <= 0)
            {
                return i;
            }
        }
        return -1;
    }

    public GameObject GetRandomObject()
    {
        Debug.Assert(Objects != null, "[ArrayPrefabCollectableItemObjectBasic: GetRandomObject] -> Method called when array is null");

        int index = GetRandomObjectIndex();
        if (index != -1)
        {
            return Objects[index].Object;
        }
        return null;
    }

    public GameObject GetGameObject(int index)
    {
        Debug.Assert(Objects != null, "[ArrayPrefabCollectableItemObjectBasic: GetGameObject] -> Method called when array is null");
        Debug.Assert(index >= 0 && index < Objects.Count, "[ArrayPrefabObjectBasic: GetGameObject] -> Method called with invalid index");

        return Objects[index].Object;
    }
}
