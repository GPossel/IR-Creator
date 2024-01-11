using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AssetReferenceManager : SingletonDontDestroyOnLoad<AssetReferenceManager>
{
    [Header("Create a list of player prefs, no scripts attached")]
    [SerializeField]
    public List<GameObject> MyPlayerPrefabs;

    [SerializeField]
    public TreasureHuntItemAlarmHelper TreasureHuntAlarmHelperPrefab;

    [Header("Reference to worlds")]
    [SerializeField]
    public List<GameObject> MyWorldsPrefabs;

    [Header("Reference to UI's for GamePlay")]
    [SerializeField]
    public List<GameObject> MyUIgamePlayCanvases;

    [Header("Reference to stack object for GamePlay")]
    [SerializeField]
    public GameObject MyStackObjectPrefabs;

    [Header("Reference to prefabs to use")]
    [SerializeField] public List<PoissonPrefabObjectCollectionScriptableObject> AllSceneObjects;
    [SerializeField] public List<PoissonPrefabCollectableItemsSingle> AllSpawnableItems;


    [SerializeField] public GameObject[] LaneWorldAreas;
    [SerializeField] public GameObject[] RoundWorldAreas;
    [SerializeField] public GameObject[] FillUpsOfLaneWorld;
    [SerializeField] public GameObject[] FillUpsOfRoundWorld;


    private void Start()
    {
        Assert.IsNotNull(MyPlayerPrefabs);
    }
}