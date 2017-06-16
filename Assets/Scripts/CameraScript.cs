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

    /* Camera not moving outside of map. */
    private Camera cameraComponent;
    private float camHalfHeight;
    private float camHalfWidth;
    public BoxCollider2D mapBounds;
    private Vector3 minBounds;
    private Vector3 maxBounds;

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

        cameraComponent = GetComponent<Camera>();
        camHalfHeight = cameraComponent.orthographicSize;
        camHalfWidth = camHalfHeight * Screen.width / Screen.height;
        minBounds = mapBounds.bounds.min;
        maxBounds = mapBounds.bounds.max;
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
        if (mapBounds == null)
        {
            mapBounds = FindObjectOfType<MapBounds>().GetComponent<BoxCollider2D>();
            if (mapBounds == null)
            {
                Debug.Log("No bounds set for this map.");
            }
            else
            {
                minBounds = mapBounds.bounds.min;
                maxBounds = mapBounds.bounds.max;
            }
        }

        float clampedX = Mathf.Clamp(transform.position.x, minBounds.x + camHalfWidth, maxBounds.x - camHalfWidth);
        float clampedY = Mathf.Clamp(transform.position.y, minBounds.y + camHalfHeight, maxBounds.y - camHalfHeight);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    public float RoundToNearestPixel(float unityUnits)
    {
        float valueInPixels = unityUnits * pixelToUnits;
        valueInPixels = Mathf.Round(valueInPixels);
        float roundedUnityUnits = valueInPixels * (1 / pixelToUnits);
        return roundedUnityUnits;
    }
}
