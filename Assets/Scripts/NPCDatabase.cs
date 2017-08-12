using System.Collections.Generic;

public static class NPCDatabase
{
    /// <summary>
    /// NPC IDs
    /// </summary>
    public enum ID
    {
        example,
        desert_yellowHood,
        desert_master,
        desert_alchemist,
        desert_historian,
        desert_blacksmith,
        desert_merchant,
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

    public static string[] QuestNotCompleteString
    {
        get { return new string[] { "You don't seem to have finished the quest." }; }
    }

    static NPCDatabase()
    {
        valuesDictionary[(int)ID.desert_master] = new string[][]
        {
            new string[] { "Yellow Hood" },
            new string[] { "Sprites/mushroom (2)" },
            new string[] { "Hi there! How are you?", "I've been having trouble collecting some items.", "Are you up for a task?" },
            new string[] { "Thanks for your help, friend!" },
            new string[] { "Thanks again for the help." }
        };

        /* Example template. */
        idToInfo[(int)ID.example] = new string[][]
        {
            new string[] { "Name"},                             // NPC name
            new string[] { "NPC sprite directory"},             // Directory of character's sprite
            new string[] { "Dialogue sprite directory" },       // Directory of sprite that will show in dialogues
            new string[] { "Neutral text" },                    // Neutral/normal text
        };

        idToInfo[(int)ID.desert_yellowHood] = new string[][]
        {
            new string[] { "Yellow Hood"},                              // NPC name
            new string[] { "Sprites/playerref editted"},                // Directory of character's sprite
            new string[] { "Sprites/mushroom (2)" },                    // Sprite directory
            new string[] { "Hi there!" },                               // Neutral/normal text
        };
    }
}