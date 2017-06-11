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

    /* (NPC_ID, ["Name", "Sprite Directory"]) */
    public readonly Dictionary<int, string[][]> valuesDictionary = new Dictionary<int, string[][]>();

    public void OnEnable()
    {
        valuesDictionary[(int)ID.yellowHood] = new string[][]
        {
            new string[] { "Yellow Hood" },
            new string[] { "Sprites/mushroom (2)" },
            new string[] { "Hi there! How are you?", "I've been having trouble collecting some items.", "Are you up for a task?" }
        };
    }
}