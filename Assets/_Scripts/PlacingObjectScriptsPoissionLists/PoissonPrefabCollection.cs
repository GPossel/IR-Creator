using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PoissonResource/PoissonPrefab Collection")]
public class PoissonPrefabCollection : PoissonPrefabObjectCollectionScriptableObject
{
    [SerializeField] private VisualStyleTypes _stylingType = VisualStyleTypes.Western;

    [SerializeField] public Material _material = null;

    [SerializeField] private List<ArrayPrefabObjectBasic> _elements = new List<ArrayPrefabObjectBasic>() { new ArrayPrefabObjectBasic(false) };

    [SerializeField] private string[] _names = new string[] { "PoissonPrefab GameObjects" };
    public override VisualStyleTypes VisualStyleType => _stylingType;
    public override Material Material => _material;
    public override string[] Names => _names;
    public override List<ArrayPrefabObjectBasic> Elements => _elements;
}
