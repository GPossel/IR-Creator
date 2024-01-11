using UnityEngine;

[CreateAssetMenu(menuName = "Characters/CharacterSriptableObject")]
public class CharacterScriptableObject : ScriptableObject
{
    [SerializeField] public string Name;
    [SerializeField] public Material Material;
    [SerializeField] public Color Color;
    [SerializeField] public Sprite CharacterSprite;
    [SerializeField] public ItemTypeEnum Currency;
    [SerializeField] public int Price;
    [SerializeField] public bool isUnlocked;
    [SerializeField] public GameObject PrefabOfCharacter;
}