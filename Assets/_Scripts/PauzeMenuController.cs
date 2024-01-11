using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class PauzeMenuController : MonoBehaviour
{
    [SerializeField] SceneOrchestration SceneOrchestration;
    [SerializeField] Button myPauzeBtn;
    [SerializeField] GameObject myPauzeMenuOverlay;
    [SerializeField] GameObject myPauzeMenu;

    public static bool isPauzed = false;

    void Start()
    {
        if (SceneOrchestration == null)
            SceneOrchestration = FindObjectOfType<SceneOrchestration>();

        Assert.IsNotNull(SceneOrchestration);
        Assert.IsNotNull(myPauzeBtn);
        Assert.IsNotNull(myPauzeMenuOverlay);
        Assert.IsNotNull(myPauzeMenu);
    }

    void Update()
    {
        if (isPauzed)
        {
            Pauze();
        }
        else
        {
            Resume();
        }
    }

    public void Pauze()
    {
        myPauzeMenu.gameObject.SetActive(true);
        PauzeMenuTimer.SetTimeToStop();
        isPauzed = true;
    }

    public void Resume()
    {
        myPauzeMenu.gameObject.SetActive(false);
        PauzeMenuTimer.SetTimeToNormal();
        isPauzed = false;
    }

    public void Menu()
    {
        Resume();
        SceneOrchestration.LoadMenu();
    }


    public void QuitGame()
    {
        SceneOrchestration.ExitGame();
    }

}