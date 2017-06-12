using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class InventoryTabButton : MonoBehaviour {
    public int tabNumber;   // 0 = equips, 1 = use, 2 = mats

    /* Notify the inventory UI which grid to display. */
    public void ButtonOnClick()
    {
        PlayerInventory.Instance.SetTabInFocus(tabNumber);
    }
}
