using UnityEngine;
using UnityEngine.UI;

public class InsigniaPanel : MonoBehaviour {
    /* Provided by Unity inspector. */
    public GameObject health;
    public GameObject attack;
    public GameObject defence;
    public GameObject defPen;
    public GameObject mastery;

    #region Text children
    private Text healthName;
    private Text healthBase;
    private Text healthBonus;
    private Text healthTotal;
    private Text attackName;
    private Text attackBase;
    private Text attackBonus;
    private Text attackTotal;
    private Text defenceName;
    private Text defenceBase;
    private Text defenceBonus;
    private Text defenceTotal;
    private Text defPenName;
    private Text defPenBase;
    private Text defPenBonus;
    private Text defPenTotal;
    private Text masteryName;
    private Text masteryBase;
    private Text masteryBonus;
    private Text masteryTotal;
    #endregion

    private readonly int NAME_POS = 0;
    private readonly int BASE_POS = 1;
    private readonly int BONUS_POS = 2;
    private readonly int TOTAL_POS = 3;

    // Use this for initialization
    void Start () {
        #region Text children initialization
        healthName = health.transform.GetChild(NAME_POS).GetComponent<Text>();
        healthBase = health.transform.GetChild(BASE_POS).GetComponent<Text>();
        healthBonus = health.transform.GetChild(BONUS_POS).GetComponent<Text>();
        healthTotal = health.transform.GetChild(TOTAL_POS).GetComponent<Text>();

        attackName = attack.transform.GetChild(NAME_POS).GetComponent<Text>();
        attackBase = attack.transform.GetChild(BASE_POS).GetComponent<Text>();
        attackBonus = attack.transform.GetChild(BONUS_POS).GetComponent<Text>();
        attackTotal = attack.transform.GetChild(TOTAL_POS).GetComponent<Text>();

        defenceName = defence.transform.GetChild(NAME_POS).GetComponent<Text>();
        defenceBase = defence.transform.GetChild(BASE_POS).GetComponent<Text>();
        defenceBonus = defence.transform.GetChild(BONUS_POS).GetComponent<Text>();
        defenceTotal = defence.transform.GetChild(TOTAL_POS).GetComponent<Text>();

        defPenName = defPen.transform.GetChild(NAME_POS).GetComponent<Text>();
        defPenBase = defPen.transform.GetChild(BASE_POS).GetComponent<Text>();
        defPenBonus = defPen.transform.GetChild(BONUS_POS).GetComponent<Text>();
        defPenTotal = defPen.transform.GetChild(TOTAL_POS).GetComponent<Text>();

        masteryName = mastery.transform.GetChild(NAME_POS).GetComponent<Text>();
        masteryBase = mastery.transform.GetChild(BASE_POS).GetComponent<Text>();
        masteryBonus = mastery.transform.GetChild(BONUS_POS).GetComponent<Text>();
        masteryTotal = mastery.transform.GetChild(TOTAL_POS).GetComponent<Text>();
        #endregion

        Refresh();
    }

    /* Load in the stats from the player. */
    public void Refresh()
    {
        PlayerController player = PlayerController.Instance;
        //healthName.text = "Health:";
        int maxHp = player.MaxHp;
        int bonusHp = player.BonusHp;
        healthBase.text = "" + maxHp;
        healthBonus.text = "+" + bonusHp;
        healthTotal.text = "" + (maxHp + bonusHp);

        //attackName.text = "Attack:";
        int baseAtt = player.BaseAtt;
        int bonusAtt = player.BonusAtt;
        attackBase.text = "" + baseAtt;
        attackBonus.text = "+" + bonusAtt;
        attackTotal.text = "" + (baseAtt + bonusAtt);

        //defenceName.text = "Defence:";
        int baseDef = player.Defence;
        int bonusDef = player.BonusDef;
        defenceBase.text = "" + baseDef;
        defenceBonus.text = "+" + bonusDef;
        defenceTotal.text = "" + (baseDef + bonusDef);

        //defPenName.text = "Def Pen:";
        float baseDefPen = player.DefencePen;
        float bonusDefPen = player.BonusDefPen;
        defPenBase.text = "" + baseDefPen + "%";
        defPenBonus.text = "+" + bonusDefPen + "%";
        defPenTotal.text = "" + (baseDefPen + bonusDefPen) + "%";

        //masteryName.text = "Mastery:";
        float baseMastery = player.Mastery;
        float bonusMastery = player.BonusMastery;
        masteryBase.text = "" + baseMastery + "%";
        masteryBonus.text = "+" + bonusMastery + "%";
        masteryTotal.text = "" + (baseMastery + bonusMastery) + "%";
    }
}
