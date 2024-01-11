using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class SceneOrchestration : SingletonDontDestroyOnLoad<SceneOrchestration>
{
    [SerializeField] bool resetPlayerPrefAndMusic;
    [SerializeField] GameManager2 myGameManager;
    [SerializeField] SoundManager mySoundManager;
    private GameManagerSharedSetupInfo SharedGameManagerSetupInfo = new GameManagerSharedSetupInfo();

    public int CountOfRuns = 0;
    // ref : idea? https://myriadgamesstudio.com/how-to-use-the-unity-scenemanager/
    // Start is called before the first frame update

    protected override void Awake()
    {
        CountOfRuns = 0;
        base.Awake();

        if (resetPlayerPrefAndMusic)
        {
            PlayerSettings.SetisOnBackgroundMusic(true);
            PlayerSettings.SetMyBackgroundMusicVolume(0.3f);
            PlayerSettings.SetisOnSF(true);
            PlayerSettings.SetMySoundEffectsVolume(0.3f);
            PlayerSettings.SetNumbOfRuns(0);
        }
    }

    void Start()
    {
        Assert.IsNotNull(myGameManager);
        Assert.IsNotNull(mySoundManager);
    }

    void Update()
    {

    }

    public void FixedUpdate()
    {

    }

    // only script to set the new values, this happens in the very first scene
    public void InitializeGameManagerInfo()
    {
        myGameManager.SetSharedInfo(SharedGameManagerSetupInfo);
        myGameManager.gameObject.SetActive(true); // we need to activate it so it becomes part of the don't destory on load
        mySoundManager.PlayBackgroundSound();
        LoadUpcomingScene();
    }

    public bool isValidGenerationSet() =>
        (SharedGameManagerSetupInfo.MyWorldType != WorldType.None &&
        SharedGameManagerSetupInfo.MyGameMode != GameMode.None &&
        SharedGameManagerSetupInfo.MyCollectionType != VisualStyleTypes.None);

    public void UpdateSelectedWorld(WorldType worldType) => SharedGameManagerSetupInfo.MyWorldType = worldType;
    public void UpdateSelectedGameMode(GameMode gamemode) => SharedGameManagerSetupInfo.MyGameMode = gamemode;
    public void UpdateSelectedCollectionType(VisualStyleTypes collectiontype) => SharedGameManagerSetupInfo.MyCollectionType = collectiontype;

    public void Restart() => SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name, LoadSceneMode.Additive);
    public void LoadUpcomingScene()
    {
        var indexOfUpcomingLevel = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadSceneAsync(indexOfUpcomingLevel);
    }

    #region Loading Scenes with naming

    public void StartNewRun()
    {
        mySoundManager.StopBackgroundMusic();
        mySoundManager.PlayBackgroundSound();
        StartCoroutine(LoadYourAsyncScene("StartLevel", null));
        CountOfRuns++;
    }

    // https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.MoveGameObjectToScene.html
    public void LoadMenu()
    {
        mySoundManager.StopBackgroundMusic();
        mySoundManager.PlayBackgroundSoundPlayOptionsQuitUI();
        StartCoroutine(LoadYourAsyncScene("GameUIPlayOptionsQuit", null));
    }

    public void LoadCharacterSelectionView()
    {
        mySoundManager.StopBackgroundMusic();
        mySoundManager.PlayBackgroundSoundCharacterSelectionUI();
        StartCoroutine(LoadYourAsyncScene("CharacterSelection", null));
    }
    public void ExitGame() 
    { 
        Debug.Log("Game Closing"); 
        Application.Quit(); 
    }

    #endregion

    IEnumerator LoadYourAsyncScene(string sceneName, GameObject m_MyGameObject)
    {
        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        // more information on Additive loading: ref: https://www.youtube.com/watch?v=l_Y5SmWkOm4

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
        if (m_MyGameObject != null)
        {
            SceneManager.MoveGameObjectToScene(m_MyGameObject, SceneManager.GetSceneByName(sceneName));
            // Unload the previous Scene
        }

        SceneManager.UnloadSceneAsync(currentScene);
    }
}