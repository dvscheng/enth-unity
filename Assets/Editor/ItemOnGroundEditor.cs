using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemOnGround))]
public class ItemOnGroundEditor : Editor {

    override
	public void OnInspectorGUI ()
    {
        ItemOnGround myItemOnGround = (ItemOnGround)target;
        ItemDatabaseSO itemData = CreateInstance<ItemDatabaseSO>();

        /* Set the default item: 1 mushroom. */
        if (myItemOnGround.item == null)
        {
            myItemOnGround.item = CreateInstance<ItemObject>();
        }

        myItemOnGround.itemBcTrigger = EditorGUILayout.ObjectField("Pickup Trigger Range", myItemOnGround.itemBcTrigger, typeof(BoxCollider2D), false) as BoxCollider2D;
        myItemOnGround.itemBcHitbox = EditorGUILayout.ObjectField("Item Hitbox", myItemOnGround.itemBcHitbox, typeof(BoxCollider2D), false) as BoxCollider2D;

        EditorGUILayout.Space();

        // TODO: bug, doesnt initialize the item until you play the game THEN edit the id.
        /* Show/set the item ID, changes the item as you set it. */
        myItemOnGround.item.id = EditorGUILayout.IntSlider("Item ID", myItemOnGround.item.id, 0, 2);
        //myItemOnGround.item.ItemID = EditorGUILayout.IntField("Item ID", myItemOnGround.item.ItemID);
        int itemType = itemData.itemList[myItemOnGround.item.id].type;
        myItemOnGround.GetComponent<ItemOnGround>().Initialize(myItemOnGround.item.id, myItemOnGround.amount);

        /* Show/get the item amount. */
        myItemOnGround.amount = EditorGUILayout.IntField("Amount", myItemOnGround.amount);


        /* Show the item type. */
        EditorGUILayout.LabelField("Item Type", "" + myItemOnGround.item.GetType());
    }
}
