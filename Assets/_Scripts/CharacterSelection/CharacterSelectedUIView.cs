using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class CharacterSelectedUIView : MonoBehaviour
{
    [SerializeField] public SelectedCharacterScriptableObject mySelectedCharacter;
    [SerializeField] public Image selectedCharacterImg = null;
    [SerializeField] public Text selectedCharacterName = null;


    void Start()
    {
        Assert.IsNotNull(mySelectedCharacter);
        Assert.IsNotNull(selectedCharacterImg);
        Assert.IsNotNull(selectedCharacterName);

        selectedCharacterImg.sprite = mySelectedCharacter.myCharacterSO.CharacterSprite;
        selectedCharacterName.text = mySelectedCharacter.myCharacterSO.Name;
    }

    void Update()
    {
        UpdateSelectionView();
    }

    public void Bind(CharacterScriptableObject selectedCharacter)
    {
        this.mySelectedCharacter.myCharacterSO = selectedCharacter;
    }

    public void UpdateSelectionView()
    {
        selectedCharacterImg.sprite = mySelectedCharacter.myCharacterSO.CharacterSprite;
        selectedCharacterName.text = mySelectedCharacter.myCharacterSO.Name;
    }
}
