using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemOnGround))]
public class ItemOnGroundEditor : Editor {

    override
	public void OnInspectorGUI ()
    {
        ItemOnGround myItemOnGround = (ItemOnGround)target;
        ItemDatabase itemDB = ScriptableObject.CreateInstance<ItemDatabase>();

        /* Set the default item: 1 mushroom. */
        if (myItemOnGround.item == null)
        {
            myItemOnGround.item = new MatItems(1, 1);
        }

        myItemOnGround.itemBcTrigger = EditorGUILayout.ObjectField("Pickup Trigger Range", myItemOnGround.itemBcTrigger, typeof(BoxCollider2D), false) as BoxCollider2D;
        myItemOnGround.itemBcHitbox = EditorGUILayout.ObjectField("Item Hitbox", myItemOnGround.itemBcHitbox, typeof(BoxCollider2D), false) as BoxCollider2D;

        EditorGUILayout.Space();

        // TODO: bug, doesnt initialize the item until you play the game THEN edit the id.
        /* Show/set the item ID, changes the item as you set it. */
        myItemOnGround.item.ItemID = EditorGUILayout.IntField("Item ID", myItemOnGround.item.ItemID);
        int itemType = itemDB.itemIDToType[myItemOnGround.item.ItemID];
        switch (itemType) {
            case ((int)ItemDatabase.ItemType.equip):
                myItemOnGround.GetComponent<ItemOnGround>().SetItem(new EquipItems(myItemOnGround.item.ItemID, myItemOnGround.item.Amount));
                break;

            case ((int)ItemDatabase.ItemType.use):
                myItemOnGround.GetComponent<ItemOnGround>().SetItem(new UseItems(myItemOnGround.item.ItemID, myItemOnGround.item.Amount));
                break;

            case ((int)ItemDatabase.ItemType.mats):
                myItemOnGround.GetComponent<ItemOnGround>().SetItem(new MatItems(myItemOnGround.item.ItemID, myItemOnGround.item.Amount));
                break;
        }

        /* Show/get the item amount. */
        myItemOnGround.item.Amount = EditorGUILayout.IntField("Amount", myItemOnGround.item.Amount);


        /* Show the item type. */
        EditorGUILayout.LabelField("Item Type", "" + myItemOnGround.item.GetType());
    }
}
