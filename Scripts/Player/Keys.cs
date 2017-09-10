using System.Collections.Generic;
using UnityEngine;

public class Keys : MonoBehaviour {

    [SerializeField]
    List<int> keys = new List<int>();

    private AudioSource audioSource;


    public void Start()
    {
        // Get or add the audio for the sound effect.
        audioSource = GetComponentInChildren<AudioSource>();
        if (!audioSource)
        {
            Debug.LogWarning(name + " Cannot find audio source to play sound effects. Adding it.");
            transform.Find("SFX").gameObject.AddComponent<AudioSource>();
            audioSource = GetComponentInChildren<AudioSource>();
        }
    }

    public void GainKey(int keyId)
    {
        AudioClip clip = SoundManager.instance.GetSoundEffect("Key");
        if (clip != null)
            audioSource.PlayOneShot(clip);

        // Dont add the key if we already have a key of the same ID
        if (!keys.Contains(keyId)) keys.Add(keyId);
        
    }

    public bool UseKey(int keyId)
    {
        if (keys.Count > 0 && keys.Contains(keyId))
        {
            keys.Remove(keyId);
            return true;
        }

        return false;

    }

    public bool HasKey(int keyId) { return keys.Contains(keyId); }
}
