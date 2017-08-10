using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Database SO", menuName = "Item Database SO", order = 1)]
public class ItemDatabaseSO : ScriptableObject {
    /* 
       When adding items:
     
         1. Add to ItemID enum.
         2. Create and add to dictionary.
         3. Add to monster drop tables.
         
       When adding monsters:

         1. Add to MobID enum.
         2. Add to the 4 drop rate types to the monster drop table.
     */



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
        CreateAndAddMat((int)ItemID.mushroom, (int)ItemType.mats, "Mushroom", Resources.Load<Sprite>("Sprites/spr_mushroom"), true, false, "A shroomy.",
            255);
        CreateAndAddMat((int)ItemID.rock, (int)ItemType.mats, "Rock", Resources.Load<Sprite>("Sprites/spr_rock"), true, false, "A rock.",
            255);
        CreateAndAddEquip((int)ItemID.leatherMail, (int)ItemType.equip, "Leather Mail", Resources.Load<Sprite>("Sprites/spr_leather_mail"), false, false, "A mail of leather.",
            0, 5, 10, 0f, 0f);

        /* The drop table is a 2d array that is resized on runtime depending on how many mobs and drop types there are.
                Accessed via dropTable[mobID, itemDropRateType]. */
        int numMobs = Enum.GetValues(typeof(MobID)).Length;
        int numDropRateTypes = Enum.GetValues(typeof(DropRateType)).Length;
        dropTable = new int[numMobs, numDropRateTypes];

        dropTable[(int)MobID.Slime, (int)DropRateType.common] = (int)ItemID.leatherMail;
        dropTable[(int)MobID.Slime, (int)DropRateType.uncommon] = (int)ItemID.leatherMail;
        dropTable[(int)MobID.Slime, (int)DropRateType.rare] = (int)ItemID.mushroom;
        dropTable[(int)MobID.Slime, (int)DropRateType.legendary] = (int)ItemID.rock;

        dropTable[(int)MobID.BlueSlime, (int)DropRateType.common] = (int)ItemID.rock;
        dropTable[(int)MobID.BlueSlime, (int)DropRateType.uncommon] = (int)ItemID.rock;
        dropTable[(int)MobID.BlueSlime, (int)DropRateType.rare] = (int)ItemID.rock;
        dropTable[(int)MobID.BlueSlime, (int)DropRateType.legendary] = (int)ItemID.mushroom;
    }

    /* Creates a new EquipItems with the specified ItemType and adds it to the list.*/
    private void CreateAndAddEquip(int id, int type, string itemName, Sprite sprite, bool isStackable, bool isQuestItem, string flavorText,
                                int attack, int defence, int hp, float defencePen, float mastery)
    {
        ItemObject item = CreateInstance<EquipItems>();

        InitializeBaseItem(item, id, type, itemName, sprite, isStackable, isQuestItem, flavorText);
        (item as EquipItems).attack = attack;
        (item as EquipItems).defence = defence;
        (item as EquipItems).hp = hp;
        (item as EquipItems).defencePen = defencePen;
        (item as EquipItems).mastery = mastery;

        itemList.Add(item);
    }

    /* Creates a new UseItems with the specified ItemType and adds it to the list.*/
    private void CreateAndAddUse(int id, int type, string itemName, Sprite sprite, bool isStackable, bool isQuestItem, string flavorText,
                                int restoreHp, int maxAmount)
    {
        ItemObject item = CreateInstance<UseItems>();

        InitializeBaseItem(item, id, type, itemName, sprite, isStackable, isQuestItem, flavorText);
        (item as UseItems).restoreHp = restoreHp;
        (item as UseItems).maxAmount = maxAmount;

        itemList.Add(item);
    }

    /* Creates a new MatItems with the specified ItemType and adds it to the list.*/
    private void CreateAndAddMat(int id, int type, string itemName, Sprite sprite, bool isStackable, bool isQuestItem, string flavorText,
                                int maxAmount)
    {
        ItemObject item = CreateInstance<MatItems>();

        InitializeBaseItem(item, id, type, itemName, sprite, isStackable, isQuestItem, flavorText);
        (item as MatItems).maxAmount = maxAmount;

        itemList.Add(item);
    }

    /* Initializes the ItemObject parameters. */
    private void InitializeBaseItem(ItemObject item, int id, int type, string itemName, Sprite sprite, bool isStackable, bool isQuestItem, string flavorText)
    {
        item.id = id;
        item.type = type;
        item.isStackable = isStackable;
        item.isQuestItem = isQuestItem;
        item.itemName = itemName;
        item.sprite = sprite;
        item.flavorText = flavorText;
    }

    //        /* Map MobIDs to Sprites. */
    //        mobToSprite = new Sprite[numMobs];
    //        mobToSprite[(int)MobID.Slime] = Resources.Load<Sprite>("Sprites/spr_slime_strip2");
    //        mobToSprite[(int)MobID.BlueSlime] = Resources.Load<Sprite>("Sprites/spr_slime_blue_strip");
    //    }
    //}
}
