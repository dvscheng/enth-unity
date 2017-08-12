using UnityEngine;

public class ItemObject : ScriptableObject{
    public int id;                                      // The unique ID of every item.
    public int type;                                    // The item type (of Equip, Use, or Mats).
    public string itemName;                             // The item's name.
    public Sprite sprite;                               // The item's Sprite image.
    public bool isStackable;
    public bool isQuestItem;                            // Is this item a special quest item?
    public string flavorText;                           // The description of the item.
}
