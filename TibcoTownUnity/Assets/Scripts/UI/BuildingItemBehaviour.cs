using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class BuildingItemBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    [SerializeField]
    private BuildingItem item;
    [SerializeField]
    private GameObject priceGroup;
    [SerializeField]
    private Image itemSprite;
    [SerializeField]
    private Text itemPrice;
    [SerializeField]
    private RectTransform dragObject;
    [SerializeField]
    private RectTransform dragArea;
    [SerializeField]
    private Color disableColor;

    private Vector2 originalDragPointerPosition;
    private Vector3 originalDragPosition;
    private Vector3 originalPosition;
    private bool isDisable = false;

    private void Start()
    {
        originalPosition = dragObject.localPosition;
        itemSprite.sprite = item.sprite;
        itemPrice.text = item.price.ToString("000");

        GameManagerBehaviour.Instance.PlayerMoneyAction += OnMoneyChanged;
    }

    public void OnBeginDrag(PointerEventData data)
	{
        if(!isDisable){
			originalDragPosition = dragObject.localPosition;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(dragArea, data.position, data.pressEventCamera, out originalDragPointerPosition);
			priceGroup.SetActive(false); 
        }
    }

	public void OnDrag(PointerEventData data)
	{
        if (!isDisable)
        {
            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(dragArea, data.position, data.pressEventCamera, out localPointerPosition))
            {
                Vector3 offsetToOriginal = localPointerPosition - originalDragPointerPosition;
                dragObject.localPosition = originalDragPosition + offsetToOriginal;
            }
        }
	}

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDisable)
        {
            Vector3 worldPoiterPosition;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(dragArea, dragObject.position, Camera.main, out worldPoiterPosition);
            GameObject building = Instantiate(item.buildItemPrefab, worldPoiterPosition, Quaternion.identity);
            building.GetComponentInChildren<BuildItem>().SetValues(item.sprite, item.price, item.contructionTime, item.profitValue, item.profitTime);
            dragObject.localPosition = originalPosition;
            priceGroup.SetActive(true);
        }
    }

	public void OnMoneyChanged (int money)
    {
        if(item.price > money)
        {
            itemSprite.color = disableColor;
            isDisable = true;
        } else 
        {
            itemSprite.color = Color.white;
            isDisable = false;
        }
    }
}
