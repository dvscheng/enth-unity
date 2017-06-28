using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour {

    public GameObject parentObj; // UNITY
    public GameObject fillObj;  // UNITY
    private Enemy parent;
    private SpriteRenderer background;
    private SpriteRenderer fill;

    public int currentHealth;
    public int maxHealth;
    public float currentPercentHealth;
    private float origWidth;



    // Use this for initialization
    void Start () {
        parent = parentObj.GetComponent<Enemy>();
        background = GetComponent<SpriteRenderer>();
        fill = fillObj.GetComponent<SpriteRenderer>();
        currentHealth = parent.Hp;
        maxHealth = parent.MaxHp;
        origWidth = fill.transform.localScale.x;
        CalculateHealthPercent();
    }
	
	public void UpdateBar()
    {
        currentHealth = parent.Hp;
        maxHealth = parent.MaxHp;
        CalculateHealthPercent();
        fill.transform.localScale = new Vector3(origWidth * currentPercentHealth, fill.transform.localScale.y, fill.transform.localScale.z);
    }

    void CalculateHealthPercent()
    {
        currentPercentHealth = (float)currentHealth / (float)maxHealth;
    }
}
