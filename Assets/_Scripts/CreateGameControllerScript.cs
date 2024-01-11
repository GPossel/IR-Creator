using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class CreateGameControllerScript : MonoBehaviour
{
    [SerializeField] public SceneOrchestration SceneOrchestration;
    [SerializeField] public ToggleGroup select_Worlds;
    [SerializeField] public ToggleGroup select_GameModes;
    [SerializeField] public ToggleGroup select_Style;
    [SerializeField] public Button Generate_InfiniteRunner_btn;
    [SerializeField] public GameObject Warning_Not_AllOptions_Selected;

    private void Awake()
    {
        if (SceneOrchestration == null)
            SceneOrchestration = FindObjectOfType<SceneOrchestration>();

        Assert.IsNotNull(SceneOrchestration);
        Assert.IsNotNull(select_Worlds);
        Assert.IsNotNull(select_GameModes);
        Assert.IsNotNull(select_Style);
        Assert.IsNotNull(Generate_InfiniteRunner_btn);
        Assert.IsNotNull(Warning_Not_AllOptions_Selected);
    }

    private void Start()
    {


    }

    private void Update()
    {
        if (!select_Worlds.AnyTogglesOn())
            SceneOrchestration.Instance.UpdateSelectedWorld(WorldType.None);

        if (!select_GameModes.AnyTogglesOn())
            SceneOrchestration.Instance.UpdateSelectedGameMode(GameMode.None);

        if (!select_Style.AnyTogglesOn())
            SceneOrchestration.Instance.UpdateSelectedCollectionType(VisualStyleTypes.None);

        if (SceneOrchestration.isValidGenerationSet()) Warning_Not_AllOptions_Selected.SetActive(false);
    }

    public void UpdateWolrdTypeToRound() => SceneOrchestration.Instance.UpdateSelectedWorld(WorldType.Round);
    public void UpdateWolrdTypeToLane() => SceneOrchestration.Instance.UpdateSelectedWorld(WorldType.Lane);
    public void UpdateGameModesTypeToClassic() => SceneOrchestration.Instance.UpdateSelectedGameMode(GameMode.Classic);
    public void UpdateGameModesTypeToTreasure() => SceneOrchestration.Instance.UpdateSelectedGameMode(GameMode.Treasure);
    public void UpdateGameModesTypeToMission() => SceneOrchestration.Instance.UpdateSelectedGameMode(GameMode.Mission);
    public void UpdateSelectedCollectionTypeToUrbanType() => SceneOrchestration.Instance.UpdateSelectedCollectionType(VisualStyleTypes.Space);
    public void UpdateSelectedCollectionTypeToNatureType() => SceneOrchestration.Instance.UpdateSelectedCollectionType(VisualStyleTypes.Western);

    public void StartGenerationOfInfiniteRunnerGame()
    {
        if (!SceneOrchestration.isValidGenerationSet())
        {
            if (!select_Worlds.AnyTogglesOn())
            {
                List<Toggle> myToggles = select_Worlds.GetComponentsInChildren<Toggle>().ToList();
                myToggles.ForEach(delegate (Toggle tog)
                {
                    StartCoroutine(SmallWarningAnimation(tog));
                });
            }
            if (!select_GameModes.AnyTogglesOn())
            {
                List<Toggle> myToggles = select_GameModes.GetComponentsInChildren<Toggle>().ToList();
                myToggles.ForEach(delegate (Toggle tog)
                {
                    StartCoroutine(SmallWarningAnimation(tog));
                });
            }
            if (!select_Style.AnyTogglesOn())
            {
                List<Toggle> myToggles = select_Style.GetComponentsInChildren<Toggle>().ToList();
                myToggles.ForEach(delegate (Toggle tog)
                {
                    StartCoroutine(SmallWarningAnimation(tog));
                });
            }

            Warning_Not_AllOptions_Selected.SetActive(true);
            return;
        }

        SceneOrchestration.InitializeGameManagerInfo();
    }

    IEnumerator<object> SmallWarningAnimation(Toggle toggle)
    {
        var background = toggle.gameObject.transform.GetChild(0);
        var anim = background.GetComponent<Animation>();
        if (anim != null)
        {
            var animationLength = anim.clip.length;
            anim.Play();

            yield return new WaitForSeconds(animationLength);

            background.GetComponent<Animation>().Stop();
        }
    }

}