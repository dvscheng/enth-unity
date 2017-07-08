using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Database SO", menuName = "Item Database SO", order = 1)]
public class ItemDatabaseSO : ScriptableObject {
    public enum ItemID
    {
        mushroom,
        rock,
        leatherMail
    }

    public enum ItemType
    {
        equip,
        use,
        mats
    }

    public enum DropRateType
    {
        common,
        uncommon,
        rare,
        legendary,
    }

    public enum MobID
    {
        Slime,
        BlueSlime,
    }

    /* Percentages, used in Enemy.RollForDrops() */
    public int COMMON_DROPRATE = 40;
    public int UNCOMMON_DROPRATE = 40;
    public int RARE_DROPRATE = 10;
    public int LEGENDARY_DROPRATE = 1;

    public int[,] dropTable;

    /* Indexed by id. */
    public List<ItemObject> itemList;
    //public List<MobObject> mobList;

    void OnEnable()
    {
        itemList = new List<ItemObject>();

        /* Create the item objects. */
        CreateAndAddItem((int)ItemID.mushroom, (int)ItemType.mats, ItemID.mushroom.ToString(),
                            Resources.Load<Sprite>("Sprites/spr_mushroom"), false, "");
        CreateAndAddItem((int)ItemID.rock, (int)ItemType.mats, ItemID.rock.ToString(),
                            Resources.Load<Sprite>("Sprites/spr_rock"), false, "");
        CreateAndAddItem((int)ItemID.leatherMail, (int)ItemType.equip, ItemID.leatherMail.ToString(),
                            Resources.Load<Sprite>("Sprites/spr_leather_mail"), false, "");

        /* The drop table is a 2d array that is resized on runtime depending on how many mobs and drop types there are.
                Accessed via dropTable[mobID, itemDropRateType]. */
        int numMobs = Enum.GetValues(typeof(MobID)).Length;
        int numDropRateTypes = Enum.GetValues(typeof(DropRateType)).Length;
        dropTable = new int[numMobs, numDropRateTypes];
        dropTable[(int)MobID.Slime, (int)DropRateType.common] = (int)ItemID.mushroom;
        dropTable[(int)MobID.Slime, (int)DropRateType.uncommon] = (int)ItemID.mushroom;
        dropTable[(int)MobID.Slime, (int)DropRateType.rare] = (int)ItemID.mushroom;
        dropTable[(int)MobID.Slime, (int)DropRateType.legendary] = (int)ItemID.rock;
        dropTable[(int)MobID.BlueSlime, (int)DropRateType.common] = (int)ItemID.rock;
        dropTable[(int)MobID.BlueSlime, (int)DropRateType.uncommon] = (int)ItemID.rock;
        dropTable[(int)MobID.BlueSlime, (int)DropRateType.rare] = (int)ItemID.rock;
        dropTable[(int)MobID.BlueSlime, (int)DropRateType.legendary] = (int)ItemID.mushroom;
    }

    /* Creates the item and adds it to the itemList. */
    void CreateAndAddItem(int id, int type, string itemName, Sprite sprite, bool isQuestItem, string flavorText)
    {
        ItemObject item = CreateInstance<ItemObject>();
        item.id = id;
        item.type = type;
        item.itemName = itemName;
        item.sprite = sprite;
        item.isQuestItem = isQuestItem;
        item.flavorText = flavorText;

        itemList.Add(item);
    }

    //        /* Map MobIDs to Sprites. */
    //        mobToSprite = new Sprite[numMobs];
    //        mobToSprite[(int)MobID.Slime] = Resources.Load<Sprite>("Sprites/spr_slime_strip2");
    //        mobToSprite[(int)MobID.BlueSlime] = Resources.Load<Sprite>("Sprites/spr_slime_blue_strip");
    //    }
    //}
}
