using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//[CreateAssetMenu]
public class ItemDatabase : ScriptableObject {

    public enum ItemType
    {
        equip,
        use,
        mats,
        error
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

    // common, uncommon, rare, and legendary.
    public enum ItemDropRate
    {
        common,
        uncommon,
        rare,
        legendary,
    }

    public int COMMON = 40;
    public int UNCOMMON = 40;
    public int RARE = 10;
    public int LEGENDARY = 1;

    // TODO: check scriptable object and stuff
    public int[,] dropTable;    // TODO: change to readonly somehow

    public int[] itemIDToType;

    public readonly Dictionary<int, Sprite> itemDictionary = new Dictionary<int, Sprite>();

    public readonly Dictionary<int, Sprite> mobToSprite = new Dictionary<int, Sprite>();
    
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


        int numItems = Enum.GetValues(typeof(ItemID)).Length;
        itemIDToType = new int[numItems];
        // one for none?
        itemIDToType[(int)ItemID.mushroom] = (int)ItemType.mats;
        itemIDToType[(int)ItemID.rock] = (int)ItemType.mats;
        itemIDToType[(int)ItemID.leatherMail] = (int)ItemType.equip;

        itemDictionary[(int)ItemID.none] = Resources.Load<Sprite>("Sprites/spr_item_slot");
        itemDictionary[(int)ItemID.mushroom] = Resources.Load<Sprite>("Sprites/spr_mushroom");
        itemDictionary[(int)ItemID.rock] = Resources.Load<Sprite>("Sprites/spr_rock");
        itemDictionary[(int)ItemID.leatherMail] = Resources.Load<Sprite>("Sprites/spr_leather_mail");

        mobToSprite[(int)MobID.Slime] = Resources.Load<Sprite>("Sprites/spr_slime_strip2");
        mobToSprite[(int)MobID.BlueSlime] = Resources.Load<Sprite>("Sprites/spr_slime_blue_strip");
    }
}