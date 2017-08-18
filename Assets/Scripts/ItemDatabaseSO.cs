using System;
using System.Collections.Generic;
using UnityEngine;

public static class ItemDatabaseSO
{
    /* 
       When adding items:
     
         1. Add to ItemID enum.
         2. Create and add to dictionary.
         3. Add to monster drop tables.
         
       When adding monsters:

         1. Add to MobID enum.
         2. Add to the 4 drop rate types to the monster drop table.
         IN UNITY EDITOR:
         3. Make prefab with same name as the enum.
         4. Change the prefab's Mob ID, move speed, sprite, box collider sizes, and hp bar location
         IN JOURNAL (ENEMY KC TRACKER):
         5. 
     */

    /* item id same as sprite name */
    public enum ItemID
    {
        mushroom,
        rock,
        leatherMail,
        slimeOrb,
        slimeFang,
        skeletalSpine,
        skeletalCore,
        cactusFlower,
        cactusFluid,
        scorpioStinger,
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

    /* Spawner requires this to have same name as Prefab. */
    public enum MobID
    {
        Slime,
        BlueSlime,
        SkeletalWyvern,
        Cactus,
        DesertScorpio
    }

    /* Percentages, used in Enemy.RollForDrops() */
    public static int COMMON_DROPRATE = 40;
    public static int UNCOMMON_DROPRATE = 40;
    public static int RARE_DROPRATE = 10;
    public static int LEGENDARY_DROPRATE = 1;

    public static int[,] dropTable;

    /* Indexed by id. */
    public static Dictionary<int, ItemObject> itemList;

    private static Sprite[] itemSprites;
    //public List<MobObject> mobList;

    static ItemDatabaseSO()
    {
        itemList = new Dictionary<int, ItemObject>();
        itemSprites = Resources.LoadAll<Sprite>("Sprites/Items/desert materials");      // item sprites path here
        for (int i = 0; i < Enum.GetNames(typeof(ItemID)).Length; i++)
        {
            Sprite temp = itemSprites[i];
            // get the Sprite that has the name of enum index i
        }
        

        /* Create the item objects. */
        Material((int)ItemID.mushroom, (int)ItemType.mats,
            "Mushroom", Resources.Load<Sprite>("Sprites/Items/spr_mushroom"), true, false,
            "A shroomy.",
            255);
        Material((int)ItemID.rock, (int)ItemType.mats,
            "Rock", Resources.Load<Sprite>("Sprites/Items/spr_rock"), true, false,
            "A rock.",
            255);
        Equip((int)ItemID.leatherMail, (int)ItemType.equip,
            "Leather Mail", Resources.Load<Sprite>("Sprites/Items/spr_leather_mail"), false, false,
            "A mail of leather.",
            0, 5, 10, 0f, 0f);
        Material((int)ItemID.slimeOrb, (int)ItemType.mats,
            "Slime Orb", Resources.Load<Sprite>("Sprites/Items/spr_mushroom"), true, false,
            "The condensed essence of a slime. I've heard people saying that this orb holds odd physical properties.",
            255);
        Material((int)ItemID.slimeFang, (int)ItemType.mats,
            "Fang", Resources.Load<Sprite>("Sprites/Items/spr_mushroom"), true, false,
            "An unusually pristine fang extracted from the carcase of a slime. It feels pointy and sharp to the touch.",
            255);
        Material((int)ItemID.skeletalSpine, (int)ItemType.mats,
            "Skeletal Spine", Resources.Load<Sprite>("Sprites/Items/spr_mushroom"), true, false,
            "The whole of that skeletal being's spine. It's surprisingly light and almost polished-looking, but it emits a strong, powerful aura.",
            255);
        Material((int)ItemID.skeletalCore, (int)ItemType.mats,
            "Skeletal Core", Resources.Load<Sprite>("Sprites/Items/spr_mushroom"), true, false,
            "So this is what powered that eerie beast. I'm surprised I was able to extract and handle such a strange object. Maybe I can put this to good use.",
            255);
        Material((int)ItemID.cactusFlower, (int)ItemType.mats,
            "Bristle Flower", Resources.Load<Sprite>("Sprites/Items/spr_mushroom"), true, false,
            "I couldn't resist taking this beauty with me. Its allure is irresistably captivating.. I found myself having a hard time looking away. This must have some uses.",
            255);
        Material((int)ItemID.cactusFluid, (int)ItemType.mats,
            "Cactus Fluid", Resources.Load<Sprite>("Sprites/Items/spr_mushroom"), true, false,
            "I'm assuming this fluid played a part in animating those odd plants. I'm sure this fluid has a use given the right expertise.",
            255);
        Material((int)ItemID.scorpioStinger, (int)ItemType.mats,
            "Scorpio Stinger", Resources.Load<Sprite>("Sprites/Items/spr_mushroom"), true, false,
            "A worn but dangerous weapon. With a deadly poison leaking from the tip, this is definitely a Scorpio's weapon of choice.",
            255);

        /* The drop table is a 2d array that is resized on runtime depending on how many mobs and drop types there are.
                Accessed via dropTable[mobID, itemDropRateType]. */
        int numMobs = Enum.GetValues(typeof(MobID)).Length;
        int numDropRateTypes = Enum.GetValues(typeof(DropRateType)).Length;
        dropTable = new int[numMobs, numDropRateTypes];

        dropTable[(int)MobID.Slime, (int)DropRateType.common] = (int)ItemID.slimeOrb;
        dropTable[(int)MobID.Slime, (int)DropRateType.uncommon] = (int)ItemID.slimeOrb;
        dropTable[(int)MobID.Slime, (int)DropRateType.rare] = (int)ItemID.slimeOrb;
        dropTable[(int)MobID.Slime, (int)DropRateType.legendary] = (int)ItemID.slimeOrb;

        dropTable[(int)MobID.BlueSlime, (int)DropRateType.common] = (int)ItemID.slimeFang;
        dropTable[(int)MobID.BlueSlime, (int)DropRateType.uncommon] = (int)ItemID.slimeFang;
        dropTable[(int)MobID.BlueSlime, (int)DropRateType.rare] = (int)ItemID.slimeFang;
        dropTable[(int)MobID.BlueSlime, (int)DropRateType.legendary] = (int)ItemID.slimeFang;

        dropTable[(int)MobID.SkeletalWyvern, (int)DropRateType.common] = (int)ItemID.skeletalSpine;
        dropTable[(int)MobID.SkeletalWyvern, (int)DropRateType.uncommon] = (int)ItemID.skeletalCore;
        dropTable[(int)MobID.SkeletalWyvern, (int)DropRateType.rare] = (int)ItemID.skeletalCore;
        dropTable[(int)MobID.SkeletalWyvern, (int)DropRateType.legendary] = (int)ItemID.skeletalCore;

        dropTable[(int)MobID.Cactus, (int)DropRateType.common] = (int)ItemID.cactusFlower;
        dropTable[(int)MobID.Cactus, (int)DropRateType.uncommon] = (int)ItemID.cactusFluid;
        dropTable[(int)MobID.Cactus, (int)DropRateType.rare] = (int)ItemID.cactusFluid;
        dropTable[(int)MobID.Cactus, (int)DropRateType.legendary] = (int)ItemID.cactusFluid;

        dropTable[(int)MobID.DesertScorpio, (int)DropRateType.common] = (int)ItemID.scorpioStinger;
        dropTable[(int)MobID.DesertScorpio, (int)DropRateType.uncommon] = (int)ItemID.scorpioStinger;
        dropTable[(int)MobID.DesertScorpio, (int)DropRateType.rare] = (int)ItemID.scorpioStinger;
        dropTable[(int)MobID.DesertScorpio, (int)DropRateType.legendary] = (int)ItemID.scorpioStinger;
    }

    /* Creates a new EquipItems with the specified ItemType and adds it to the list.*/
    private static void Equip(int id, int type, string itemName, Sprite sprite, bool isStackable, bool isQuestItem, string flavorText,
                                int attack, int defence, int hp, float defencePen, float mastery)
    {
        ItemObject item = new EquipItems();

        InitializeBaseItem(item, id, type, itemName, sprite, isStackable, isQuestItem, flavorText);
        (item as EquipItems).attack = attack;
        (item as EquipItems).defence = defence;
        (item as EquipItems).hp = hp;
        (item as EquipItems).defencePen = defencePen;
        (item as EquipItems).mastery = mastery;

        itemList.Add(id, item);
    }

    /* Creates a new UseItems with the specified ItemType and adds it to the list.*/
    private static void Use(int id, int type, string itemName, Sprite sprite, bool isStackable, bool isQuestItem, string flavorText,
                                int restoreHp, int maxAmount)
    {
        ItemObject item = new UseItems();

        InitializeBaseItem(item, id, type, itemName, sprite, isStackable, isQuestItem, flavorText);
        (item as UseItems).restoreHp = restoreHp;
        (item as UseItems).maxAmount = maxAmount;

        itemList.Add(id, item);
    }

    /* Creates a new MatItems with the specified ItemType and adds it to the list.*/
    private static void Material(int id, int type, string itemName, Sprite sprite, bool isStackable, bool isQuestItem, string flavorText,
                                int maxAmount)
    {
        ItemObject item = new MatItems();

        InitializeBaseItem(item, id, type, itemName, sprite, isStackable, isQuestItem, flavorText);
        (item as MatItems).maxAmount = maxAmount;

        itemList.Add(id, item);
    }

    /* Initializes the ItemObject parameters. */
    private static void InitializeBaseItem(ItemObject item, int id, int type, string itemName, Sprite sprite, bool isStackable, bool isQuestItem, string flavorText)
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
    //        mobToSprite[(int)MobID.Slime] = Resources.Load<Sprite>("Sprites/Items/spr_slime_strip2");
    //        mobToSprite[(int)MobID.BlueSlime] = Resources.Load<Sprite>("Sprites/Items/spr_slime_blue_strip");
    //    }
    //}
}
