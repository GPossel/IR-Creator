using UnityEngine;

public class TreasureHuntInstigator : MonoBehaviour
{
    // ref: https://answers.unity.com/questions/1090051/most-efficient-way-to-find-the-closest-collider-to.html
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ITreasureHuntAlarm>() != null)
        {
            var closestCollider = CalculateClosestCollider(other);
            var treasureHuntItemAlarmHelper = other.GetComponent<TreasureHuntItemAlarmHelper>();
            treasureHuntItemAlarmHelper.SetCurrentCollider(closestCollider);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ITreasureHuntAlarm>() != null)
        {
            var closestCollider = CalculateClosestCollider(other);
            var treasureHuntItemAlarmHelper = other.GetComponent<TreasureHuntItemAlarmHelper>();
            treasureHuntItemAlarmHelper.RemoveCurrentCollider(closestCollider);
        }
    }

    private SphereCollider CalculateClosestCollider(Collider other)
    {
        var getSphereColliders = other.GetComponents<SphereCollider>();

        // get current position from player
        var playerPos = transform.position;
        // match that position with one of the colliders boundries

        var closestCollider = getSphereColliders[0];

        Vector3 closestPointB = closestCollider.ClosestPointOnBounds(playerPos);
        float distanceB = Vector3.Distance(closestPointB, playerPos);

        foreach (var collider in getSphereColliders)
        {
            Vector3 closestToPointA = collider.ClosestPointOnBounds(playerPos);
            float distanceA = Vector3.Distance(closestToPointA, playerPos);

            if (distanceA < distanceB)
            {
                closestCollider = collider;
                distanceB = distanceA;
            }
        }

        return closestCollider;
    }
}