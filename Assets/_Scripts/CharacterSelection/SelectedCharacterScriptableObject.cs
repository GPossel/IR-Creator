using UnityEngine;

[CreateAssetMenu(menuName = "Characters/SelectedCharacterScriptableObject")]
public class SelectedCharacterScriptableObject : ScriptableObject
{
    [SerializeField] public CharacterScriptableObject myCharacterSO;

    public delegate void UpdateSelectedCharacterScriptableObjectUpdated();
    public event UpdateSelectedCharacterScriptableObjectUpdated OnSelectedCharacterScriptableObjectUpdated;

    public void RaiseSelectedCharacterIsUpdated()
    {
        OnSelectedCharacterScriptableObjectUpdated?.Invoke();
    }


    public void Bind(CharacterScriptableObject characterScriptableObject)
    {
        this.myCharacterSO = characterScriptableObject;
    }
}
