using UnityEngine;
public interface IBoost<T>
{
    void Booster(T amount, GameObject boostObject);
}