using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
	readonly int WIDTH = 20;	// readonly = runtime, const = compile time
	readonly int HEIGHT = 12;

	Sprite walls;
	Sprite bg;

    GameObject generatedBlocks;

	// Use this for initialization
	void Start () {
		walls = Resources.Load ("Sprites/tl_bg_tileset", typeof(Sprite)) as Sprite;
		bg = Resources.Load ("Sprites/bg_grass", typeof(Sprite)) as Sprite;
        /*
        generatedBlocks = new GameObject();
        generatedBlocks.name = "Generated Blocks";

		MakeArena ();
        */
        }
	
	// Update is called once per frame
	void Update () {
		
	}

	private void MakeArena () {
		for (int x = 0; x < WIDTH; x++)
        {
			for (int y = 0; y < HEIGHT; y++)
            {
                int BG_LAYER = LayerMask.NameToLayer("Background");
                int WALL_LAYER = LayerMask.NameToLayer("Walls");

                GameObject go = new GameObject();
                go.transform.position = new Vector3((-WIDTH/2) + x + 0.5f, (-HEIGHT/2) + y + 0.5f);

				SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
				if ((x == 0 || x == WIDTH-1) || (y == 0 || y == HEIGHT-1))
                {
                    sr.sprite = walls;
                    go.AddComponent<BoxCollider2D>();
                    go.name = "Wall (" + x + ", " + y + ")";
                    go.layer = WALL_LAYER;
                    sr.sortingLayerName = "Background";

                } else
                {
                    sr.sprite = bg;
                    go.name = "Background (" + x + ", " + y + ")";
                    go.layer = BG_LAYER;
                    sr.sortingLayerName = "Background";
                }
                go.transform.parent = generatedBlocks.transform;
			}
		}
	}
}
