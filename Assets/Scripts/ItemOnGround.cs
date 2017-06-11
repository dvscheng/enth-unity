using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnGround : MonoBehaviour {
    // Custom inspector GUI, look at ItemOnGroundEditor.cs
    public BoxCollider2D itemBc;
    public Item item;

    public void SetItem(Item item)
    {
        this.item = item;
        gameObject.GetComponent<SpriteRenderer>().sprite = this.item.sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            /* If item is successfully added to inventory, then destroy its GameObject. */
            PlayerInventory pInventory = collision.gameObject.GetComponent<PlayerController>().inventory;
            
            if (pInventory.AddToInventory(item))
            {
                Destroy(gameObject);
            }
        }
    }

}
