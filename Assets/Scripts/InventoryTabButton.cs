using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class InventoryTabButton : MonoBehaviour {
    PlayerInventory playerInventory;
    public int tabNumber;   // 0 = equips, 1 = use, 2 = mats

    private void Start()
    {
        playerInventory = GameObject.Find("Player Inventory").GetComponent<PlayerInventory>();
    }

    /* Notify the inventory UI which grid to display. */
    public void ButtonOnClick()
    {
        playerInventory.SetTabInFocus(tabNumber);
    }

}
