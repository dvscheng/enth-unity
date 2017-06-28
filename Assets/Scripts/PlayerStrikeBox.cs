using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStrikeBox : MonoBehaviour {
    [HideInInspector] public GameObject player;
    PlayerController playerController;
    public BoxCollider2D bc;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();

        /* Ignore collision between the player and its own strikebox. */
        Physics2D.IgnoreCollision(bc, player.GetComponent<BoxCollider2D>());
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /* If the collision object has an 'Enemy' script (aka if it's an enemy). */
        Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();
        if (enemyScript)
        {
            /* If we've collided with the enemy's sightRange or hitbox, then ignore collision and do NOT apply damage. */
            if (collision.Equals(enemyScript.sightRange) || collision.Equals(enemyScript.boxTrigger))
                Physics2D.IgnoreCollision(bc, collision);
            else
            {
                /* Formula: dmg = att * att / (att + (def - (def * defPen)))
                 *   from https://gamedev.stackexchange.com/questions/129319/rpg-formula-attack-and-defense 
                 */
                int damageDealt = (playerController.BaseAtt * playerController.BaseAtt) / (playerController.BaseAtt + (enemyScript.Defence - (int)(enemyScript.Defence * playerController.DefencePen)));
                damageDealt = Random.Range((int) (damageDealt * playerController.Mastery), damageDealt);
                enemyScript.TakeDamage(damageDealt);

                /* Play the appropriate enemy damage sound. */
                switch (enemyScript.mobID)
                {
                    case (int)ItemDatabase.MobID.slime:
                        AudioManager.Instance.Play("Slime Hit");
                        break;

                    case (int)ItemDatabase.MobID.blueSlime:
                        AudioManager.Instance.Play("Slime Hit");
                        break;

                    default:
                        AudioManager.Instance.Play("Default Hit");
                        break;
                }
            }
        }
    }
}
