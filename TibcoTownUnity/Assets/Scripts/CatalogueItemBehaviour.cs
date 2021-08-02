using System;
using UnityEngine;
using UnityEngine.UI;

public class CatalogueItemBehaviour : MonoBehaviour
{
    [SerializeField]
    private Image itemSprite;
    [SerializeField]
    private Text itemName,itemPrice;
    [SerializeField]
    private GameObject purchaseSuccess,purchaseFail;

    [SerializeField]
    private BuildingItem buildingItem;

    private void Start()
    {
        SetValues(buildingItem);
    }

    public void SetValues(BuildingItem item)
    {
        buildingItem = item;
        itemSprite.sprite = item.sprite;
        itemName.text = item.name;
        itemPrice.text = $"{item.price} Buy";
    }

    public void OnBuy()
    {
        if (GameManagerBehaviour.Instance.IsEnoughMoney(buildingItem.price))
        {
            GameManagerBehaviour.Instance.RemoveMoney(buildingItem.price);
            GenericPopupBehaviour.Instance.SetValues("Congrats!",
                $"You have successfully purchased {buildingItem.name}.", "OKAY");
        }
        else
        {
            GenericPopupBehaviour.Instance.SetValues("Purchase Failed!",
                $"You have dont have sufficient coins to purchase {buildingItem.name}.", "OKAY");
        }
    }
}
