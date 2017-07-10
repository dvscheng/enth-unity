using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ItemObject item;
    public int amount;
    
    GameObject grid;
    GameObject itemImage;
    GameObject count;
    GameObject tooltip;

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


    /* Acts as the constructor: displays an item on the slot. */
    public void SetItemAndSprite(ItemObject newItem, int amount)
    {
        /* Set this item slot's corresponding item to newItem, AND indicate that we have an item in the slot. */
        item = newItem;
        hasItem = true;
        this.amount = amount;

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
        this.amount += amount;
        UpdateTextCount();
    }

    public void RemoveFromExistingItem(int amount)
    {
        this.amount -= amount;
        UpdateTextCount();
        if (this.amount <= 0)
            RemoveItem();
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
        if (amount > 1)
        {
            textCount.text = "" + amount;
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
        // initialize tooltip prefab
        if (hasItem)
        {
            UIManager.Instance.itemTooltip.Initialize(item);
            // toggled on inside initialize.
            //UIManager.Instance.TurnOnOffUI(UIManager.UI_Type.itemTooltip, true);
        }
    }

    /* Un-highlights the slot. */
    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.TurnOnOffUI(UIManager.UI_Type.itemTooltip, false);
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (item != null)
        {
            // snap onto the mouse
        }
    }
}
