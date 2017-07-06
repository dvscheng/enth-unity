using System;
using UnityEngine;

/* Pip from stackexchange: https://gamedev.stackexchange.com/questions/96878/how-to-animate-objects-with-bobbing-up-and-down-motion-in-unity
 *  for the idea of using Math.Sin as a natural fluctuation.
 */
public class ItemOnGround : MonoBehaviour {
    /* Custom inspector GUI, look at ItemOnGroundEditor.cs */
    public BoxCollider2D itemBcTrigger;
    public BoxCollider2D itemBcHitbox;
    public Item item;

    float originalY;
    float fluctuation = 0.25f;

    void Start()
    {
        originalY = gameObject.transform.position.y;

        // assumes player only has one boxcollider
        Physics2D.IgnoreCollision(itemBcHitbox, PlayerController.Instance.GetComponent<BoxCollider2D>());
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Item"), LayerMask.NameToLayer("Enemy"));

        item = new MatItems(1, 1);
        SetItem(new MatItems(item.ItemID, item.Amount));
    }

    void Update()
    {
        /* Apply 'bobbing' affect on the item. */
        float newY = originalY + ((float)Math.Sin(Time.time) * 0.02f);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, newY, gameObject.transform.position.z);
    }

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
            PlayerInventory pInventory = collision.gameObject.GetComponent<PlayerController>().inventory;   // could just reference pinventory singleton
            
            if (pInventory.AddToInventory(item))
            {
                Destroy(gameObject);
            }
        }
    }

}
