using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public List<AudioClip> audioClips = new List<AudioClip>();

    public AudioSource audioSource;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    /// <summary>
    /// Find clip with name and PlayOneShot
    /// </summary>
    /// <param name="name"></param>
    public void PlayClip(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("SoundManager.PlayClip(string name) name is null or empty | Error");
            return;
        }
            

        var clip = audioClips.Find(a => a.name.ToLower() == name.ToLower());
        if (clip == null)
        {
            Debug.LogError("SoundManager.PlayClip(string name) " + name + " named clip not found | Error");
            return;
        }

        audioSource.PlayOneShot(clip);
    }

    public void PlayClip(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("SoundManager.PlayClip(AudioClip clip) Clip not found | Error");
            return;
        }
            

        audioSource.PlayOneShot(clip);
    }
}
