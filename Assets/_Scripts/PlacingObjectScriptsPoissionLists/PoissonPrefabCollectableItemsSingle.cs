using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PoissonResource/PoissonPrefab SpawnableSmallElements")]
public class PoissonPrefabCollectableItemsSingle : PoissonPrefabSpawnableObjectCollection
{
    [SerializeField]
    private Material _material = null; /*new Material(Shader.Find("Standard"));*/

    [SerializeField]
    private VisualStyleTypes _visualStyle;

    [SerializeField]
    private List<ArrayPrefabCollectableItemObjectBasic> _spawnableSmallElements = new List<ArrayPrefabCollectableItemObjectBasic>();

    [SerializeField]
    private string[] _names = new string[] { "PoissonPrefab GameObjects" };

    public override Material Material => _material;
    public override string[] Names => _names;
    public override VisualStyleTypes VisualStyleType => _visualStyle;
    public override List<ArrayPrefabCollectableItemObjectBasic> SpawnableSmallElements => _spawnableSmallElements;

}