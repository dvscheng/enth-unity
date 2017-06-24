using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEventSystem : MonoBehaviour{
    // make persistent
	void Awake() {
        DontDestroyOnLoad(gameObject);
	}
	
}
