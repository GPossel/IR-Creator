using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class TotalInventoryViewCharacter : MonoBehaviour
{
    [SerializeField] public GameManager2 myGameManager;
    [SerializeField] public PlayerInventoryTotalSO playerInventoryTotal;
    [SerializeField] public GameObject InventoryItemViewPrefab;
    [SerializeField] public List<GameObject> allMyInventoryItemView;

    void Start()
    {
        if (myGameManager == null)
            myGameManager = FindObjectOfType<GameManager2>();

        Assert.IsNotNull(myGameManager);
        Assert.IsNotNull(playerInventoryTotal);
        Assert.IsNotNull(InventoryItemViewPrefab);

        var myItemReferences = myGameManager.GetInventoryItemsFromCollection();

        // get the ref information -> making sure you use the same collection
        var allItemRefs = myGameManager.GetAllItemsReferencesFromSpawnCollection();

        foreach (var item in myItemReferences)
        {
            var countOfItems = playerInventoryTotal.CountOf(item.itemType);
            if (countOfItems > 0)
            {
                var itemCountView = Instantiate(InventoryItemViewPrefab);
                itemCountView.transform.SetParent(this.transform, false);

                if (itemCountView.transform.GetChild(0).GetComponent<Text>() != null)
                    itemCountView.transform.GetChild(0).GetComponent<Text>().text =  $"{countOfItems}";
                
                if (itemCountView.transform.GetChild(1).GetComponent<Image>() != null)
                    itemCountView.transform.GetChild(1).GetComponent<Image>().sprite = allItemRefs.Where(x => x.itemType == item.itemType)
                                                                                                  .FirstOrDefault().SpriteSource;

                allMyInventoryItemView.Add(itemCountView);
            }
        }
    }
}