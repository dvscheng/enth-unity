using System.Collections.Generic;

public static class NPCDatabase
{
    /// <summary>
    /// NPC IDs
    /// </summary>
    public enum ID
    {
        example,                                        // 0
        desert_yellowHood,                              // 1
        desert_master,                                  // 2
        desert_alchemist,                               // 3
        desert_historian,                               // 4
        desert_blacksmith,                              // 5
        desert_merchant,                                // 6
    }

    public enum Slot
    {
        name,
        characterSprite,
        dialogueSprite,
        neutralText
    }

    /* (NPC_ID, [["Name"], ["Sprite Directory"], ["Initial text"], ["Quest complete text"], ["Dialogue after quest complete."]]).
     * EVERY NPC MUST HAVE AT LEAST 4 STRING[]s. */
    public static readonly Dictionary<int, string[][]> valuesDictionary = new Dictionary<int, string[][]>();


    /* Name, Sprite Directory, Neutral text, Giving quest text, */
    public static readonly Dictionary<int, string[][]> idToInfo = new Dictionary<int, string[][]>();

    public static readonly Dictionary<int, NPC> npcObjects = new Dictionary<int, NPC>();

    public static string[] QuestNotCompleteString
    {
        get { return new string[] { "You don't seem to have finished the quest." }; }
    }

    static NPCDatabase()
    {
        //obsolete for now?
        valuesDictionary[(int)ID.desert_master] = new string[][]
        {
            new string[] { "Yellow Hood" },
            new string[] { "Sprites/spr_sign" },
            new string[] { "Hi there! How are you?", "I've been having trouble collecting some items.", "Are you up for a task?" },
            new string[] { "Thanks for your help, friend!" },
            new string[] { "Thanks again for the help." }
        };

        /* Example template. */
        idToInfo[(int)ID.example] = new string[][]
        {
            new string[] { "Name"},                                     // NPC name
            new string[] { "NPC sprite directory"},                     // Directory of character's sprite
            new string[] { "Dialogue sprite directory" },               // Directory of sprite that will show in dialogues
            new string[] { "Neutral text" },                            // Neutral/normal text
        };

        idToInfo[(int)ID.desert_yellowHood] = new string[][]
        {
            new string[] { "Yellow Hood"},
            new string[] { "Sprites/place holder npcs_0"},
            new string[] { "Sprites/Items/spr_mushroom" },
            new string[] { "Hey." }, 
        };

        idToInfo[(int)ID.desert_master] = new string[][]
        {
            new string[] { "Master"},
            new string[] { "Sprites/place holder npcs_1"},
            new string[] { "Sprites/Items/spr_mushroom" },
            new string[] { "Hello there, Enth." },
        };

        idToInfo[(int)ID.desert_alchemist] = new string[][]
        {
            new string[] { "Alchemist"},
            new string[] { "Sprites/place holder npcs_2"},
            new string[] { "Sprites/Items/spr_mushroom" },
            new string[] { "Want some potions, Enth!?" },
        };

        idToInfo[(int)ID.desert_historian] = new string[][]
        {
            new string[] { "Historian"},
            new string[] { "Sprites/place holder npcs_3"},
            new string[] { "Sprites/Items/spr_mushroom" },
            new string[] { "Greetings, young man." },
        };

        idToInfo[(int)ID.desert_blacksmith] = new string[][]
        {
            new string[] { "Blacksmith"},
            new string[] { "Sprites/place holder npcs_4"},
            new string[] { "Sprites/Items/spr_mushroom" },
            new string[] { "'hoy there my boy!" },
        };

        idToInfo[(int)ID.desert_merchant] = new string[][]
        {
            new string[] { "Merchant"},
            new string[] { "Sprites/place holder npcs_5"},
            new string[] { "Sprites/Items/spr_mushroom" },
            new string[] { "Want to browse my wares? Oh, just saying hi? Of course! Hi there, Enth!" },
        };
    }

    /* Used when an NPC is first loaded in a scene. */
    public static void InitializeNPC(int id, NPC npc)
    {
        npcObjects.Add(id, npc);
    }
}