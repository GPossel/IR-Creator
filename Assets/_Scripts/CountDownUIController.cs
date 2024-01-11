using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using static GameManager2;

public class CountDownUIController : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Image timeImage;
    [SerializeField] Text timeText;
    [SerializeField] Text timeAdditionText;
    [SerializeField] float durationInSeconds;
    [SerializeField] float currentTime;
    [SerializeField] GameManager2 myGameManager;

    private bool stopTimer = false;
    private int doOnce = 0;
    // TODO: extract the time from the gamemamanger => that is were we can configure the gamesettings like: timer and speed and maybe more more 

    private void Awake()
    {
        Assert.IsNotNull(timeImage);
        Assert.IsNotNull(timeText);
        Assert.IsNotNull(timeAdditionText);
        if (myGameManager == null)
            myGameManager = FindObjectOfType<GameManager2>();

        Assert.IsNotNull(myGameManager);
        this.gameObject.SetActive(false);
        myGameManager.OnGameStateChangedEvent += StartRun;
    }

    private void OnDestroy()
    {
        myGameManager.OnGameStateChangedEvent -= StartRun;
    }

    private void Start()
    {
        Assert.IsNotNull(panel);
        currentTime = durationInSeconds;
        timeText.text = currentTime.ToString();
    }
    
    private void StartRun(GameState gameState)
    {
        if (gameState == GameState.StartRun)
        {
            gameObject.SetActive(true);
        }
    }

    private void StopTimer(bool isWon)
    {
        stopTimer = true;
    }

    private void Update() // 60 frames, making it each second
    {
        if (Input.GetKey(KeyCode.T))
        {
            AddSecondsToTimer(.5f); // 5 seconds
        }

        if (!stopTimer)
        {
            currentTime -= Time.deltaTime;
            float minutes = Mathf.FloorToInt(currentTime / 60);
            float seconds = Mathf.FloorToInt(currentTime % 60);
            float milliseconds = currentTime % 1 * 1000;
            int hundredths = (int)Mathf.Round(milliseconds / 10);

            if (minutes == 0)
            {
                timeText.text = string.Format($"{seconds}:{hundredths}");
            }
            else
            {
                timeText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, hundredths);
            }

            timeImage.fillAmount = Mathf.InverseLerp(0, durationInSeconds, currentTime);
            if (minutes <= 0 && seconds <= 0 && milliseconds <= 0)
            {
                timeText.text = "00:00";
                if (doOnce == 0)
                {
                    myGameManager.ChangeState(GameManager2.GameState.EndRun);
                    doOnce++;
                }
            }
        }
        else
        {
            Debug.Log("Timer Stopped!");
        }
    }

    public void AddSecondsToTimer(float sec)
    {
        // TODO: make prefab? 
        var secondsToAdd = sec;
        timeAdditionText.text = string.Format("+{0}s", sec * 10);
        timeAdditionText.gameObject.SetActive(true);
        currentTime = currentTime + secondsToAdd; // 5 seconds
    }
}