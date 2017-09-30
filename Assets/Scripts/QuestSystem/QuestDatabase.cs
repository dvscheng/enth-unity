using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuestDatabase {

    private static Dictionary<int, Quest> questDictionary;                                          // (id, Quest) Quest IDs mapped to the respective quest
    public static Dictionary<int, Quest> QuestDictionary
    {
        get
        {
            return questDictionary;
        }
    }

    private static Dictionary<int, List<Quest>> npcIDToQuests;                                      // (npc_id, List<Quest>) An NPC's ID mapped to a List of all their quests
    public static Dictionary<int, List<Quest>> NPCIDToQuests
    {
        get
        {
            return npcIDToQuests;
        }
    }

    static QuestDatabase()
    {
        npcIDToQuests = new Dictionary<int, List<Quest>>();
        questDictionary = new Dictionary<int, Quest>
        {
            {
                0,
                /* Example quests. */
                new Quest(
                    0,                                                                                                                      // The quest's id
                    (int)Quest.State.qualified,                                                                                             // The quest's state; refer to Quest.cs for the enum
                    new int[] { },                                                                                                          // The quests (their ids) that are required for this quest to start
                    new int[] { },                                                                                                          // The quests that have this quest as a requirement.
                    (int)NPCDatabase.ID.example,                                                                                            // The NPC that gives the quest
                    (int)NPCDatabase.ID.example,                                                                                            // The NPC that completes the quest
                    "Quest Title",
                    new string[] { "Start dialogue" },                                                                                      // Start dialogue
                    new string[] { "End dialogue" },                                                                                        // End dialogue 
                    new QuestObjective[] { new ItemQuestObjective("Objective description", (int)ItemDatabaseSO.ItemID.mushroom, 2),         // (Objective description, sprite, item ID, amount to be collected)
                                           new ItemQuestObjective("Objective description", (int)ItemDatabaseSO.ItemID.rock, 2) })
            },
            {
                1,
                new Quest(
                    1,
                    (int)Quest.State.unqualified,
                    new int[] { },
                    new int[] { },
                    (int)NPCDatabase.ID.example,
                    (int)NPCDatabase.ID.example,
                    "Quest Title",
                    new string[] { "Start dialogue" },
                    new string[] { "End dialogue" },
                    new QuestObjective[] { new ItemQuestObjective("Objective description", (int)ItemDatabaseSO.ItemID.mushroom, 2),
                                            new ItemQuestObjective("Objective description", (int)ItemDatabaseSO.ItemID.rock, 2) })
            },

            /* desert_master quests. */
            {
                2,
                new Quest(
                    2,
                    (int)Quest.State.qualified,
                    new int[] { },
                    new int[] { 3 },
                    (int)NPCDatabase.ID.desert_master,
                    (int)NPCDatabase.ID.desert_master,
                    "New Beginnings",
                    new string[] { "Good morning, young one. You come at a splendid time. I have something to discuss with you.",
                                    "I'm sure you recall how strongly I've been against you venturing off out of Elios, but it's time I told you why.",
                                    "It's been a while since the last time we've talked about the world outside of Elios, and that's for good reason. The last thing our people need is our curious young ones running to their death.",
                                    "But as you've grown, I see that you've grown mature. More importantly, I see in your eyes the curiosity and adventurousness of your father, and through your actions I see your mother's ambitious and loving heart. I believe you are ready to discover the world your father and mother once faught to defend.",
                                    "As you may already know, our Elios is a safe haven in this mysterious and dangerous world. But you need to know this: Elios is not the only safe place. We are one of few safe havens scattered across this land, meaning there are other villages like ours.",
                                    "Your mother and father befriended all of the villages, and in turn they accumulated a power rivaled by none. But to their demise, your mother ... expand on story.",
                                    "Their goal, and now your goal, is it gain the favor of all the villages of this land. With the power of all the villages, you will face the great evil that has flooded our world.",
                                    "But enough intensity for now. You must take small steps. The first part of your journey will to gain the favor and power of our beloved village, Elios. Talk to everyone in the village, and show them this. (You gain an odd looking artifact)",
                                    "That shouldn't be too difficult of a task, granted you've lived here your whole life, can it?"},
                    new string[] { "You've returned. Good. Now you are ready for the tasks of our people. Go around and ask if there are favors you can fulfil." },
                    new QuestObjective[] { new NPCQuestObjective("Talk to the alchemist.", (int)NPCDatabase.ID.desert_alchemist, 1),
                                            new NPCQuestObjective("Talk to the historian.", (int)NPCDatabase.ID.desert_historian, 1),
                                            new NPCQuestObjective("Talk to the blacksmith.", (int)NPCDatabase.ID.desert_blacksmith, 1),
                                            new NPCQuestObjective("Talk to the merchant.", (int)NPCDatabase.ID.desert_merchant, 1) })
            },

            {
                3,
                new Quest(
                    3,
                    (int)Quest.State.unqualified,
                    new int[] { 2 },
                    new int[] { 4 },
                    (int)NPCDatabase.ID.desert_alchemist,
                    (int)NPCDatabase.ID.desert_alchemist,
                    "The Art of Alchemy",
                    new string[] { "That shouldn't be too difficult of a task, granted you've lived here your whole life, can it?", },
                    new string[] { "You've returned. Splendid! How did it go?" },
                    new QuestObjective[] { new ItemQuestObjective("Collect a cactus's fluid.", (int)ItemDatabaseSO.ItemID.cactusFluid, 2),
                                           new ItemQuestObjective("Collect a slime's orb.", (int)ItemDatabaseSO.ItemID.slimeOrb, 2),
                                           new ItemQuestObjective("Collect a scorpion's stinger.", (int)ItemDatabaseSO.ItemID.scorpioStinger, 1) })
            },

            {
                4,
                new Quest(
                    4,
                    (int)Quest.State.unqualified,
                    new int[] { 3 },
                    new int[] { 5 },
                    (int)NPCDatabase.ID.desert_historian,
                    (int)NPCDatabase.ID.desert_historian,
                    "Ancestral Knowledge",
                    new string[] { "That shouldn't be too difficult of a task, granted you've lived here your whole life, can it?", },
                    new string[] { "You've returned. Splendid! How did it go?" },
                    new QuestObjective[] { new ItemQuestObjective("Collect a skeletal wyvern's spine.", (int)ItemDatabaseSO.ItemID.skeletalSpine, 1),
                                           new ItemQuestObjective("Collect a slime's fang.", (int)ItemDatabaseSO.ItemID.slimeFang, 1) })
            },

            {
                5,
                new Quest(
                    5,
                    (int)Quest.State.unqualified,
                    new int[] { 4 },
                    new int[] { 6 },
                    (int)NPCDatabase.ID.desert_blacksmith,
                    (int)NPCDatabase.ID.desert_blacksmith,
                    "Forging Power",
                    new string[] { "That shouldn't be too difficult of a task, granted you've lived here your whole life, can it?", },
                    new string[] { "You've returned. Splendid! How did it go?" },
                    new QuestObjective[] { new ItemQuestObjective("Collect a skeletal wyvern's core.", (int)ItemDatabaseSO.ItemID.skeletalCore, 1),
                                           new ItemQuestObjective("Collect a scorpio's stinger.", (int)ItemDatabaseSO.ItemID.scorpioStinger, 2) })
            },

            {
                6,
                new Quest(
                    6,
                    (int)Quest.State.unqualified,
                    new int[] { 5 },
                    new int[] { },
                    (int)NPCDatabase.ID.desert_merchant,
                    (int)NPCDatabase.ID.desert_merchant,
                    "Charisma",
                    new string[] { "That shouldn't be too difficult of a task, granted you've lived here your whole life, can it?", },
                    new string[] { "You've returned. Splendid! How did it go?" },
                    new QuestObjective[] { new NPCQuestObjective("Talk to the alchemist.", (int)NPCDatabase.ID.desert_alchemist, 1),
                                            new NPCQuestObjective("Talk to the historian.", (int)NPCDatabase.ID.desert_historian, 1),
                                            new NPCQuestObjective("Talk to the blacksmith.", (int)NPCDatabase.ID.desert_blacksmith, 1),
                                            new NPCQuestObjective("Talk to the merchant.", (int)NPCDatabase.ID.desert_master, 1),
                                            new NPCQuestObjective("Talk to the merchant.", (int)NPCDatabase.ID.desert_yellowHood, 1) })
            },
        };
        CreateIDToQuestDictionary();
    }

    /* Used once to create an (id, quest) dictionary. */
    private static void CreateIDToQuestDictionary()
    {
        int[] keys = new int[questDictionary.Count];
        questDictionary.Keys.CopyTo(keys, 0);
        foreach (int key in keys)
        {
            Quest quest = questDictionary[key];
            if (npcIDToQuests.ContainsKey(quest.StartNPC))
                npcIDToQuests[quest.StartNPC].Add(quest);                                                       // Adds a quest to the existing List<Quest>
            else
                npcIDToQuests.Add(quest.StartNPC, new List<Quest>(new Quest[] { quest }));                      // Creates a new List<Quest>
        }
    }
}
