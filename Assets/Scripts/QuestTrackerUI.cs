using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTrackerUI : MonoBehaviour {
    #region Singleton Behaviour
    private static QuestTrackerUI _instance;   // singleton behaviour
    public static QuestTrackerUI Instance
    {
        get { return _instance; }
    }
    #endregion

    public GameObject questObjectArea;  // UNITY


    readonly int MAX_NUM_QUESTS = 2;
    GameObject[] quests;

    // Use this for initialization
    void Awake()
    {
        #region Singleton Behaviour
        /* Singleton behaviour. */
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(gameObject);
        #endregion

        quests = new GameObject[MAX_NUM_QUESTS];
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
