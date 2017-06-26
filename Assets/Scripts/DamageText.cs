using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour {

    public GameObject character;
    public Vector2 size;
    public Vector3 position;
    public int damage;
    private float damageShown;
    public Text text;
    private Color _origColor;
    private Color _newColor;
    private float _newColorAlpha;

    const float MIN_WIDTH = -0.5f;
    const float MAX_WIDTH = 0.5f;
    const float MIN_HEIGHT = 0.5f;
    const float MAX_HEIGHT = 1f;
    const float DISAPPEAR_DELAY = 1.5f;
    readonly Vector3 DISAPPEAR_DIST = new Vector3(0f, 0.1f, 0f);
    bool timeToDisappear = false;

    /* Get the size of the text box. */
    void Start()
    {
        RectTransform rectTransform = ((RectTransform)gameObject.transform);
        size = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
    }

    public void Initialize(GameObject character, int damage)
    {
        this.character = character;
        this.damage = damage;

        /* Set and generate the position of the text. */
        float relMinWidth = character.transform.position.x + MIN_WIDTH;
        float relMaxWidth = character.transform.position.x + MAX_WIDTH;
        float relMinHeight = character.transform.position.y + MIN_HEIGHT;
        float relMaxHeight = character.transform.position.y + MAX_HEIGHT;
        position = new Vector2(Random.Range(relMinWidth, relMaxWidth), Random.Range(relMinHeight, relMaxHeight));
        gameObject.transform.position = position;

        StartCoroutine(VisibilityTimer(DISAPPEAR_DELAY));
    }

    /* Causes the text to fade after a certain time and keep it at the same position despite camera movement. */
    void Update()
    {
        if (timeToDisappear)
        {
            _origColor = text.color;
            if (_origColor.a <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                /* Make the text start to fade. */
                _newColor = _origColor;
                _newColor.a -= 2.5f * Time.deltaTime;
                text.color = _newColor;

                /* Make the text move up. */
                position += DISAPPEAR_DIST * Time.deltaTime;
            }
        }
        else if (damageShown != damage)
        {
            /* Count up to the number. */
            damageShown += (damage / (DISAPPEAR_DELAY/10) * Time.deltaTime);
            text.text = "" + (int)damageShown;
            if (damageShown > damage)
                damageShown = damage;
        }
        gameObject.transform.position = position;
    }

    /* Allows the text to disappear once waitTime is up. */
    public IEnumerator VisibilityTimer(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        timeToDisappear = true;
    }
}
