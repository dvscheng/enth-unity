using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Item item;
    public Item Item
    {
        get { return item; }
        set { item = value; }
    }
    
    GameObject grid;
    GameObject itemImage;
    GameObject count;

    PlayerInventory playerInventory;

    private bool hasItem;
    public bool HasItem
    {
        get { return hasItem; }
    }


    // Use this for initialization
    void Awake() {
        item = null;
        hasItem = false;

        /* Get the "Item Image" GameObject. Alpha value is set to 0 in prefab.
         * Get the "Text" GameObject. Default to be invisible. */
        itemImage = gameObject.transform.GetChild(0).gameObject;
        count = gameObject.transform.GetChild(1).gameObject;

        /* Get the grid (inventory tab grid) that this ItemSlot belongs to. */
        grid = gameObject.transform.parent.gameObject;

        /* Get a reference to the player's inventory. */
        playerInventory = PlayerInventory.Instance;
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (item != null)
        {
            // snap onto the mouse
        }
    }

    /* Acts as the constructor: displays an item on the slot. */
    public void SetItemAndSprite(Item newItem)
    {
        /* Set this item slot's corresponding item to newItem, AND indicate that we have an item in the slot. */
        this.item = newItem;
        hasItem = true;

        /* Set the "Item Image" GameObject's image to the new item's sprite;
            Change the color to white so that the item image isn't transparent.*/
        Image itemImageComponent = itemImage.GetComponent<Image>();
        Color c = itemImageComponent.color;
        c = Color.white;
        itemImageComponent.color = c;
        itemImageComponent.sprite = newItem.sprite;

        /* Check and show the amount of the item. */
        UpdateTextCount();
    }

    /* Adds an AMOUNT to an already existing item. */
    public void AddToExistingItem(int amount)
    {
        item.Amount += amount;
        UpdateTextCount();
    }

    public void RemoveFromExistingItem(int amount)
    {
        item.Amount -= amount;
        UpdateTextCount();
        if (item.Amount <= 0)
        {
            print("Removed item");
            RemoveItem();
        }
    }

    // TODO
    /* Remove the item from the slot, remove the sprite, and set the color to be transparent. */
    public void RemoveItem()
    {
        item = null;
        hasItem = false;

        /* Set the color to be transparent */
        Image itemImageComponent = itemImage.GetComponent<Image>();
        Color c = itemImageComponent.color;
        c.a = 0;
        itemImageComponent.color = c;
        //itemImageComponent.sprite = null;
    }

    /* Updates the count text on the bottom-left of the item slot. */
    private void UpdateTextCount()
    {
        Text textCount = count.GetComponent<Text>();
        if (item.Amount > 1)
        {
            textCount.text = "" + item.Amount;
            textCount.enabled = true;
        } else
        {
            textCount.enabled = false;
            textCount.text = "" + 2;
        }
    }

    public void SetPosition(Vector2 position)
    {
        gameObject.transform.position = position;
    }

    /* Highlights the slot. */
    public void OnPointerEnter(PointerEventData eventData)
    {
        //
    }

    /* Un-highlights the slot. */
    public void OnPointerExit(PointerEventData eventData)
    {
        //throw new NotImplementedException();
    }
}
