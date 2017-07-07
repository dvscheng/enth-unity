using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Item Database", menuName = "Custom Objects", order = 1)]
public class ItemDatabase : ScriptableObject {

    public enum ItemType
    {
        error,
        equip,
        use,
        mats
    }

    public enum ItemID
    {
        none,
        mushroom,
        rock,
        leatherMail
    }

    public enum MobID
    {
        Slime,
        BlueSlime,
    }

    public enum ItemDropRate
    {
        common,
        uncommon,
        rare,
        legendary,
    }

    /* Percentages, used in Enemy.RollForDrops() */
    public int COMMON = 40;
    public int UNCOMMON = 40;
    public int RARE = 10;
    public int LEGENDARY = 1;

    // TODO: check scriptable object and stuff
    public int[,] dropTable;    // TODO: change to readonly somehow

    /* ItemID to its type. */
    public int[] itemIDToType;

    /* ItemID to its Sprite. */
    public Sprite[] itemToSprite;

    /* MobID to its Sprite. */
    public Sprite[] mobToSprite;
    
    public void OnEnable()
    {
        /* The drop table is a 2d array that is resized on runtime depending on how many mobs and drop types there are.
            Accessed via dropTable[mobID, itemDropRateType]. */
        int numMobs = Enum.GetValues(typeof(MobID)).Length;
        int numDropRateTypes = Enum.GetValues(typeof(ItemDropRate)).Length;
        dropTable = new int[numMobs, numDropRateTypes];
        dropTable[(int)MobID.Slime, (int)ItemDropRate.common] = (int)ItemID.mushroom;
        dropTable[(int)MobID.Slime, (int)ItemDropRate.uncommon] = (int)ItemID.mushroom;
        dropTable[(int)MobID.Slime, (int)ItemDropRate.rare] = (int)ItemID.mushroom;
        dropTable[(int)MobID.Slime, (int)ItemDropRate.legendary] = (int)ItemID.rock;
        dropTable[(int)MobID.BlueSlime, (int)ItemDropRate.common] = (int)ItemID.rock;
        dropTable[(int)MobID.BlueSlime, (int)ItemDropRate.uncommon] = (int)ItemID.rock;
        dropTable[(int)MobID.BlueSlime, (int)ItemDropRate.rare] = (int)ItemID.rock;
        dropTable[(int)MobID.BlueSlime, (int)ItemDropRate.legendary] = (int)ItemID.mushroom;

        /* Map ItemIDs to item types. */
        int numItems = Enum.GetValues(typeof(ItemID)).Length;
        itemIDToType = new int[numItems];
        itemIDToType[(int)ItemID.none] = (int)ItemType.mats;
        itemIDToType[(int)ItemID.mushroom] = (int)ItemType.mats;
        itemIDToType[(int)ItemID.rock] = (int)ItemType.mats;
        itemIDToType[(int)ItemID.leatherMail] = (int)ItemType.equip;

        /* Map ItemIDs to Sprites. */
        itemToSprite = new Sprite[numItems];
        itemToSprite[(int)ItemID.none] = Resources.Load<Sprite>("Sprites/spr_item_slot");
        itemToSprite[(int)ItemID.mushroom] = Resources.Load<Sprite>("Sprites/spr_mushroom");
        itemToSprite[(int)ItemID.rock] = Resources.Load<Sprite>("Sprites/spr_rock");
        itemToSprite[(int)ItemID.leatherMail] = Resources.Load<Sprite>("Sprites/spr_leather_mail");

        /* Map MobIDs to Sprites. */
        mobToSprite = new Sprite[numMobs];
        mobToSprite[(int)MobID.Slime] = Resources.Load<Sprite>("Sprites/spr_slime_strip2");
        mobToSprite[(int)MobID.BlueSlime] = Resources.Load<Sprite>("Sprites/spr_slime_blue_strip");
    }
}