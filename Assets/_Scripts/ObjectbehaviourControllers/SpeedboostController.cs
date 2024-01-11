using UnityEngine;

public class SpeedboostController : MonoBehaviour
{
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<IBoost<int>>() != null)
        {
            col.gameObject.GetComponent<IBoost<int>>().Booster(2, col.gameObject);
        }
    }
}
