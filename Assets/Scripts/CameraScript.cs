using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    #region Singleton Behaviour
    private static CameraScript _instance;   // singleton behaviour
    public static CameraScript Instance
    {
        get { return _instance; }
    }
    #endregion

    public GameObject player;
    public float pixelToUnits = 40f;

    // Use this for initialization
    void Awake() {
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
    }

    void Update()
    {
        if (player != null)
        {
            float player_x = player.transform.position.x;
            float player_y = player.transform.position.y;

            float rounded_x = RoundToNearestPixel(player_x);
            float rounded_y = RoundToNearestPixel(player_y);

            Vector3 new_pos = new Vector3(rounded_x, rounded_y, -10.0f); // this is 2d, so my camera is that far from the screen.
            gameObject.transform.position = new_pos;
        }
    }

    public float RoundToNearestPixel(float unityUnits)
    {
        float valueInPixels = unityUnits * pixelToUnits;
        valueInPixels = Mathf.Round(valueInPixels);
        float roundedUnityUnits = valueInPixels * (1 / pixelToUnits);
        return roundedUnityUnits;
    }
}
