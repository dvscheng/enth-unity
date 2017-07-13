using UnityEngine;

public class UIEventSystem : MonoBehaviour {
    #region Singleton Behaviour
    private static UIEventSystem _instance;   // singleton behaviour
    public static UIEventSystem Instance
    {
        get { return _instance; }
    }
    #endregion


    // make persistent
    void Awake() {
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
        #endregion
        DontDestroyOnLoad(gameObject);
	}
}
