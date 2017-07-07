using UnityEngine;

[System.Serializable]
public class ItemObject : ScriptableObject {
    [Range(0, 3)]                               //Enum.GetValues(typeof(ItemDatabase.ItemID)).Length
    public int id;

    public Sprite sprite;
    public bool isQuestItem;
    public bool isStackable;
    public string itemName;
    public string flavorText;
}
