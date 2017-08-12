using System;
using UnityEngine;

/* Pip from stackexchange: https://gamedev.stackexchange.com/questions/96878/how-to-animate-objects-with-bobbing-up-and-down-motion-in-unity
 *  for the idea of using Math.Sin as a natural fluctuation.
 */
public class ItemOnGround : MonoBehaviour {
    /* Custom inspector GUI, look at ItemOnGroundEditor.cs */
    public BoxCollider2D itemBcTrigger;
    public BoxCollider2D itemBcHitbox;
    public ItemObject item;
    public int amount;

    ItemDatabaseSO itemData;

    float originalY;
    float fluctuation = 0.25f;

    /* ALWAYS call Initialize when creating the GameObject; fetches the appropriate item
     *  from itemDatabase, sets the amount, and applies the sprite to the GameObject.*/
    public void Initialize(int id, int amount)
    {
        itemData = ScriptableObject.CreateInstance<ItemDatabaseSO>();
        item = itemData.itemList[id];
        this.amount = amount;
        gameObject.GetComponent<SpriteRenderer>().sprite = item.sprite;
    }

    /* Initialize SO's and remove physics with the player and enemies. */
    void Awake()
    {

        originalY = gameObject.transform.position.y;

        // assumes player only has one boxcollider
        Physics2D.IgnoreCollision(itemBcHitbox, PlayerController.Instance.GetComponent<BoxCollider2D>());
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Item"), LayerMask.NameToLayer("Enemy"));
    }

    /* Apply 'bobbing' affect on the item. */
    void Update()
    {
        float newY = originalY + ((float)Math.Sin(Time.time) * 0.02f);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, newY, gameObject.transform.position.z);
    }

    /* Attempts to add the item to the player inventory upon player collision. */
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            PlayerInventory pInventory = collision.gameObject.GetComponent<PlayerController>().inventory;   // could just reference pinventory singleton
            /* If item is successfully added to inventory, then destroy its GameObject. */
            if (pInventory.AddToInventory(item, amount))
            {
                Destroy(gameObject);
            }
        }
    }
}
