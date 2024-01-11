using UnityEngine;
using UnityEngine.Assertions;

public class ScoreUI : MonoBehaviour
{
    public TextMesh Text_Score;
    public Transform player = null; // position of player

    private void Start()
    {
        Assert.IsNotNull(Text_Score);
    }

    void Update()
    {
        Text_Score.text = player?.position.z.ToString("0"); // display in who numbers
    }

    public void BindToPlayer(Transform playerTransform)
    {
        this.player = playerTransform;
    }
}