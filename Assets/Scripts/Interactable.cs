using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    public CircleCollider2D interactRange;
    [HideInInspector] Inputs inputs;

	// Use this for initialization
	void Awake () {
        inputs = gameObject.AddComponent<Inputs>();
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            if (inputs.interaction_key_down)
            {
                // open textbox
                // second "page" will have buttons (green check red x) to click on to accept or not
            }
        }
    }
}
