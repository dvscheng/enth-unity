using System.Collections.Generic;
using UnityEngine;

public class NPCDatabase : ScriptableObject
{
    /// <summary>
    /// NPC IDs
    /// </summary>
    public enum ID
    {
        example,
        desert_master,
        desert_alchemist,
        desert_historian,
        desert_blacksmith,
        desert_merchant,
        desert_yellowHood,
    }

    /* (NPC_ID, [["Name"], ["Sprite Directory"], ["Initial text"], ["Quest complete text"], ["Dialogue after quest complete."]]).
     * EVERY NPC MUST HAVE AT LEAST 4 STRING[]s. */
    public readonly Dictionary<int, string[][]> valuesDictionary = new Dictionary<int, string[][]>();


    /* Name, Sprite Directory, Neutral text, Giving quest text, */
    public readonly Dictionary<int, string[][]> idToDialogues = new Dictionary<int, string[][]>();

    public string[] QuestNotCompleteString
    {
        get { return new string[] { "You don't seem to have finished the quest." }; }
    }

    public void OnEnable()
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
        idToDialogues[(int)ID.example] = new string[][]
        {
            new string[] { "Name"},                             // NPC name
            new string[] { "NPC sprite directory"},             // Directory of character's sprite
            new string[] { "Dialogue sprite directory" },       // Directory of sprite that will show in dialogues
            new string[] { "Neutral text" },                    // Neutral/normal text
        };

        idToDialogues[(int)ID.desert_master] = new string[][]
        {
            new string[] { "Name"},                             // NPC name
            new string[] { "NPC sprite directory"},             // Directory of character's sprite
            new string[] { "Sprite Directory" },                // Sprite directory
            new string[] { "Neutral text" },                    // Neutral/normal text
        };
    }
}