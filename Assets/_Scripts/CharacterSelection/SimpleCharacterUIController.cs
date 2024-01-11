using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SimpleCharacterUIController : MonoBehaviour
{
    // ref: probeer dit uit een folder te halen (!)
    [SerializeField] List<CharacterScriptableObject> allCharacterSO;
    [SerializeField] public GameObject SlotCharacterPrefab;
    [SerializeField] public SelectedCharacterScriptableObject mySelectedCharacter;
    [SerializeField] public List<GameObject> allMySlots;
    [SerializeField] public PlayerInventoryTotalSO playerTotalInventory;

    void Start()
    {
        Assert.IsNotNull(playerTotalInventory);
        Assert.IsNotNull(SlotCharacterPrefab);

        for (int i = allCharacterSO.Count - 1; i >= 0; i--)
        {
            var slotSetup = Instantiate(SlotCharacterPrefab);
            // make parent to access the horizontal view script
            slotSetup.transform.SetParent(this.transform);
            if (slotSetup.GetComponent<CharacterSlotUIController>() != null)
            {
                var characterSlotUIController = slotSetup.GetComponent<CharacterSlotUIController>();
                characterSlotUIController.Bind(allCharacterSO[i]);
                var matchingCharacterOfSlot = characterSlotUIController.myCharacter;
                // we also bind to all these buttons events
                allMySlots.Add(slotSetup);
            }
        }
    }

    void Update()
    {

    }

    // now we want to send out an update event on the new selected Character
    public void UpdateMySelectedCharacter(CharacterScriptableObject characterScriptableObject)
    {
        mySelectedCharacter.Bind(characterScriptableObject);
    }

    public bool TryToBuyTheCharacter(CharacterScriptableObject myCharacter)
    {
        return playerTotalInventory.RemoveItems(myCharacter.Currency, myCharacter.Price);
    }
}