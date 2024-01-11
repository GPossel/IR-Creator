using UnityEngine;

public interface IDamagable<T>
{
    void Damage(T amount, GameObject attackerObject);
}