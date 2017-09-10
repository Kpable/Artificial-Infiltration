using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundEffectTrigger : MonoBehaviour {

    public string soundEffectName;

    private AudioSource audioSource;

    private AudioClip clip = null;
    
    // Use this for initialization
	void Start () {
        // Get or add the audio for the sound effect.
        audioSource = GetComponentInChildren<AudioSource>();
        if (!audioSource)
        {
            Debug.LogWarning(name + " Cannot find audio source to play sound effects. Adding it.");
            transform.Find("SFX").gameObject.AddComponent<AudioSource>();
            audioSource = GetComponentInChildren<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (clip == null) clip = SoundManager.instance.GetSoundEffect(soundEffectName);
        if (clip == null) Debug.LogWarning(name + ": Unable to find sound effect: " + soundEffectName);

        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}
