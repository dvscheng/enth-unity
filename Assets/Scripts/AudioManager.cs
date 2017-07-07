using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    #region Singleton Behaviour
    private static AudioManager _instance;   // singleton behaviour
    public static AudioManager Instance
    {
        get { return _instance; }
    }
    #endregion

    public Sound[] sounds;

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
            Destroy(gameObject);
            Destroy(this);
            return;
        }
        #endregion
        DontDestroyOnLoad(gameObject);

        /* For each Sound in the sounds array, create an AudioSource component with appropriate parameters. */
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
	}
	
	public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);    // looks in sounds for a sound such that sound.name == name.
        if (s == null)
        {
            Debug.Log("Sound " + name + " was not found.");
            return;
        }

        s.source.Play();
    }
}
