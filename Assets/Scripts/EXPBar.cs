using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EXPBar : MonoBehaviour {

    // used for scaling with resolution
    public Camera screen;

    public GameObject player;
    private PlayerController playerController;
    Slider expBar;
    Text level;

    public int currentExp;
    public int maxExp;
    public float currentPercentExp;

    // Use this for initialization
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        expBar = gameObject.GetComponent<Slider>();
        level = gameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();    //  EXP Bar > Level Background > Level (Text)
        SetExp();

        expBar.value = CalculateExp();
    }

    public void UpdateExp()
    {
        SetExp();
        expBar.value = CalculateExp();
        level.text = "" + PlayerController.Instance.Level;
    }

    void SetExp()
    {
        currentExp = playerController.Exp;
        maxExp = playerController.MaxExp;
    }

    float CalculateExp()
    {
        currentPercentExp = (float)currentExp / (float)maxExp;
        return currentPercentExp;
    }
}
