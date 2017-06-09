using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour {
    public bool right_key;
    public bool left_key;
    public bool up_key;
    public bool down_key;
    public bool dash_key_down;
    public bool attack_key_down;
    public bool inventory_key_down;
    public bool equipment_key_down;

    private void Awake()
    {
        BindKeys();
    }

    private void Update()
    {
        BindKeys();
    }

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
    }
}
