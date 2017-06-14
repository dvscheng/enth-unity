using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour {
    private static Inputs _instance; // singleton behaviour
    public static Inputs Instance
    {
        get { return _instance; }
    }

    public bool right_key;
    public bool left_key;
    public bool up_key;
    public bool down_key;
    public bool dash_key_down;
    public bool attack_key_down;
    public bool inventory_key_down;
    public bool equipment_key_down;
    public bool interaction_key_down;
    public bool quest_tracker_collapse_down;

    void Awake()
    {
        /* Singleton behaviour. */
        if (_instance == null)
        {
            _instance = this;
        } else if (_instance != this)
        {
            Destroy(this);
        }

        BindKeys();
    }

    void Update()
    {
        BindKeys();
    }

    /* Always check for inputs.*/
    private void BindKeys()
    {
        right_key = Input.GetKey(KeyCode.D);
        left_key = Input.GetKey(KeyCode.A);
        up_key = Input.GetKey(KeyCode.W);
        down_key = Input.GetKey(KeyCode.S);
        dash_key_down = Input.GetKeyDown(KeyCode.V);
        attack_key_down = Input.GetKeyDown(KeyCode.C);
        inventory_key_down = Input.GetKeyDown(KeyCode.B);
        equipment_key_down = Input.GetKeyDown(KeyCode.E);
        interaction_key_down = Input.GetKeyDown(KeyCode.Space);
        quest_tracker_collapse_down = Input.GetKeyDown(KeyCode.L);
    }
}
