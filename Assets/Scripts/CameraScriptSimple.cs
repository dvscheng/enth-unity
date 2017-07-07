using System;
using UnityEngine;

public class CameraScriptSimple : MonoBehaviour {

    #region Singleton Behaviour
    private static CameraScriptSimple _instance;   // singleton behaviour
    public static CameraScriptSimple Instance
    {
        get { return _instance; }
    }
    #endregion

    public GameObject player;
    public float smoothTime = 6f;
    public Vector3 velocity = Vector3.zero;

    /* Camera not moving outside of map. */
    private Camera cameraComponent;
    private float camHalfHeight;
    private float camHalfWidth;
    public BoxCollider2D mapBounds;
    private Vector3 minBounds;
    private Vector3 maxBounds;

    // Use this for initialization
    void Awake()
    {
        #region Singleton Behaviour
        /* Singleton behaviour. */
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
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

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 newPos = new Vector3(0, 0, -10);
            newPos = newPos + player.transform.position;
            /* Enables to camera to snap into the player position so as to remove jittery sprites. */
            if (Math.Abs((gameObject.transform.position - newPos).x) < 0.01f && Math.Abs((gameObject.transform.position - newPos).y) < 0.01f)
                gameObject.transform.position = newPos;
            else
                gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, newPos, ref velocity, smoothTime * Time.deltaTime, 11f, Time.deltaTime);
                //gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, newPos, smoothTime * Time.deltaTime);
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
}
