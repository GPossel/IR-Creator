using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int Lives = 3;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (Lives == 0)
        {
            GameManager2.Instance.RaiseGameStateChanged(GameManager2.GameState.EndRun);
        }
    }

    public void TakeLive() => Lives--;
}