using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//[CreateAssetMenu]
public class NPCDatabase : ScriptableObject
{

    public enum ID
    {
        yellowHood,
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
        valuesDictionary[(int)ID.yellowHood] = new string[][]
        {
            new string[] { "Yellow Hood" },
            new string[] { "Sprites/mushroom (2)" },
            new string[] { "Hi there! How are you?", "I've been having trouble collecting some items.", "Are you up for a task?" },
            new string[] { "Thanks for your help, friend!" },
            new string[] { "Thanks again for the help." }
        };

        idToDialogues[0] = new string[][]
        {
            new string[] { "Name"},
            new string[] { "Sprite Directory" },
            new string[] { "Neutral text" },
            /* First quest */
            new string[] { "Giving quest text" },
            new string[] { "Quest completion text" }
        };
    }
}