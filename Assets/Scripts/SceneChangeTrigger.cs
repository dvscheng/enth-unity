using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : MonoBehaviour {

    public string sceneName;
    public Vector2 startingPositionInNextScene;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Player"))
        {
            SceneManager.LoadSceneAsync(sceneName);
            collision.gameObject.transform.position = startingPositionInNextScene;
            //Camera.main.transform.position = startingPositionInNextScene;
        }
    }
}
