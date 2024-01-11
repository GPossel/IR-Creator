using UnityEngine;

public interface IStackObject<T>
{
    void StackObject(T amount, GameObject stackObject);
}
