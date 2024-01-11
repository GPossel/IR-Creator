using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class CharacterSlotUIController : MonoBehaviour
{
    [SerializeField] public GameManager2 myGameManager;
    [SerializeField] public CharacterScriptableObject myCharacter;

    [SerializeField] public Image charImageSprite;
    [SerializeField] public Text charNameTxt;
    [SerializeField] public Button buyCharBtn;
    [SerializeField] public Button selectCharBtn;
    private bool isUnlockedUI = false;

    [SerializeField] public NotificationManager notificationManager;

    void Start()
    {
        if (myGameManager == null)
            myGameManager = FindObjectOfType<GameManager2>();

        Assert.IsNotNull(myGameManager);

        Assert.IsNotNull(charImageSprite);
        Assert.IsNotNull(charNameTxt);
        Assert.IsNotNull(buyCharBtn);
        Assert.IsNotNull(selectCharBtn);

        if (myCharacter != null)
            Bind(myCharacter);


        if (notificationManager == null)
            notificationManager = FindObjectOfType<NotificationManager>();

        var myItemSprite = myGameManager.GetInventoryIconFromCollection(myCharacter.Currency);
        if (myItemSprite != null)
        {
            if (buyCharBtn.transform.GetChild(1).GetComponent<Image>() != null)
                buyCharBtn.transform.GetChild(1).GetComponent<Image>().sprite = myItemSprite;
        }
    }


    void Update()
    {
        isUnlockedUI = myCharacter.isUnlocked;

        if (isUnlockedUI)
        {
            selectCharBtn.gameObject.SetActive(true);
            buyCharBtn.gameObject.SetActive(false);
        }
        else
        {
            selectCharBtn.gameObject.SetActive(false);
            buyCharBtn.gameObject.SetActive(true);
        }
    }

    public void Bind(CharacterScriptableObject myCharacter)
    {
        this.myCharacter = myCharacter;
        charImageSprite.sprite = myCharacter.CharacterSprite;
        charNameTxt.text = myCharacter.Name;        
        if (buyCharBtn.transform.GetChild(0).GetComponent<Text>() != null)
            buyCharBtn.transform.GetChild(0).GetComponent<Text>().text = $" {myCharacter.Price}";

        isUnlockedUI = myCharacter.isUnlocked;
    }

    public void BtnSelectUpdateCharacterToCharacterUI()
    {
        if (this.transform.parent.GetComponent<SimpleCharacterUIController>() != null)
        {
            this.transform.parent.GetComponent<SimpleCharacterUIController>().UpdateMySelectedCharacter(myCharacter);
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    public void BtnTryBuyToCharacterUI()
    {
        if (this.transform.parent.GetComponent<SimpleCharacterUIController>() != null)
        {
            bool succes = this.transform.parent.GetComponent<SimpleCharacterUIController>().TryToBuyTheCharacter(myCharacter);
            if (succes == false)
            {
                notificationManager.ShowNotification("Too expensive!", this.transform.position.x);
                Debug.Log("Too Expensive!");
            } 
            else
            {
                myCharacter.isUnlocked = true;
            }
        }
    }

}