using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Database SO", menuName = "Item Database SO", order = 2)]
public class ItemDatabaseSO : ScriptableObject {
    public List<ItemObject> itemList;
}
