using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

[System.Serializable]
public class SFX
{
    public string name;
    public AudioClip audioClip;
}

public class SoundManager : MonoBehaviour {
    public static SoundManager instance;

    [Tooltip("This list is where you add the music clips for each mission.")]
    public List<AudioClip> musicClips;
    [Tooltip("This list is where you add the sound effects and an associated name with it. Use the same name for multiple sound effects for the same action.")]
    public List<SFX> soundEffects;
    
    [HideInInspector]
    public List<AudioSource> sfxObjects = new List<AudioSource>();


    [HideInInspector]
    public float masterVolume, musicVolume, sfxVolume;

    private GameObject gameMusic;
    private GameObject ambientMusic;
    private GameObject soundEffect;

    private void Awake()
    {
        MakeSingleton();
        gameMusic = transform.Find("Game Music").gameObject;
        ambientMusic = transform.Find("Ambient Music").gameObject;

    }

    private void MakeSingleton()
    {
        // Make a Singleton
        // If the static instance isnt null
        if (instance != null)
        {
            //We already have one Sound Manager Object In the Game, Destroy all others
            Destroy(gameObject);
        }
        else
        {
            //This is the first instance of Sound Manager, Store it and prevent it from dying. 
            instance = this;
            DontDestroyOnLoad(gameObject);
            // Only Initialize once
            Initialize();
        }
    }

    private void Initialize()
    {
        // Unregister as awake is called every time a scene is loaded. 
        SceneManager.sceneLoaded -= OnMissionLoad;
        // Then add to avoid calling the same method multiple times.. It causes problems. 
        SceneManager.sceneLoaded += OnMissionLoad;
    }

    private void OnMissionLoad(Scene mission, LoadSceneMode mode)
    {
        // The sound effects per mission may change so we want to reset them on mission load. 
        // Clear list of SFXs
        sfxObjects.Clear();
       
        // If in the Main Menu, no ambient music
        if (mission.buildIndex == 0)
        {
            if(ambientMusic.GetComponent<AudioSource>().isPlaying)
                ambientMusic.GetComponent<AudioSource>().Stop();
        }
        else
        {
            // Load list of SFXs
            GetAllSFX();

            ambientMusic.GetComponent<AudioSource>().Play();
            //Debug.Log(name + ": SFX found - " + sfxObjects.Count);

            // Set all the SFX volumes
            AdjustSFXVolume(sfxVolume);
        }

        if (musicClips.Count >= mission.buildIndex)
        {
            gameMusic.GetComponent<AudioSource>().Stop();
            gameMusic.GetComponent<AudioSource>().clip = musicClips[mission.buildIndex];
            gameMusic.GetComponent<AudioSource>().Play();
        }
    }

    private void GetAllSFX()
    {
        // Get all tagged objects in this scene
        GameObject[] search = GameObject.FindGameObjectsWithTag("SFX");
        foreach (GameObject sfx in search)
        { 
            AudioSource audio = sfx.GetComponent<AudioSource>();
            if (audio) sfxObjects.Add(audio);
        }
    }


    private void Start()
    { 
        //Debug.Log(name + " start");

        // Game Music error checking
        if (!gameMusic) gameMusic = GameObject.Find("Game Music");
        if (!gameMusic) Debug.LogError(name + " Cannot find Game Music Object");
        // Ambient Music error checking
        if (!ambientMusic) ambientMusic = GameObject.Find("Ambient Music");
        if (!ambientMusic) Debug.LogError(name + " Cannot find Ambient Music Object");

        // Get the saved volume settings. 
        masterVolume = GameManager.instance.gameSettings.masterVolume;
        musicVolume = GameManager.instance.gameSettings.musicVolume;
        sfxVolume = GameManager.instance.gameSettings.sfxVolume;

        // Set the volumes 
        gameMusic.GetComponent<AudioSource>().volume = masterVolume * musicVolume;
        ambientMusic.GetComponent<AudioSource>().volume = masterVolume * musicVolume;
        // Set all the SFX volumes
        AdjustSFXVolume(sfxVolume);

    }

    public void AdjustMasterVolume(float volume)
    {
        // Update the master volume and set the Music and Sound effect volumes accordingly
        masterVolume = volume;
        AdjustMusicVolume(musicVolume);
        AdjustSFXVolume(sfxVolume);
    }

    public void AdjustMusicVolume(float volume)
    {
        // Set and update the Music volue with the passed in value
        if(gameMusic) gameMusic.GetComponent<AudioSource>().volume = volume * masterVolume;
        if(ambientMusic) ambientMusic.GetComponent<AudioSource>().volume = masterVolume * musicVolume;

        musicVolume = volume;
    }

    public void AdjustSFXVolume(float volume)
    {
        // For all the sound effect objects found in this mission
        foreach (AudioSource sfx in sfxObjects)
        {
            // Set the volume to what was passed in
            sfx.volume = volume * masterVolume;
        }

        // Makee current volume to what was passed in
        sfxVolume = volume;
    }

    public AudioClip GetSoundEffect(string soundEffectname)
    {
        // Get all the Sound Effects with the name provided
        List<SFX> clips = soundEffects.Where(clip => clip.name == soundEffectname).ToList<SFX>();
        //Debug.Log(name + ": Clips count-" + clips.Count);
        // If  one or more was found
        if (clips.Count > 0)
        {
            // If only one sound effect was found return the first in the list
            if (clips.Count == 1) return clips[0].audioClip;
            // If more than one sound effect was found, return one at random
            else return clips[Random.Range(0, clips.Count)].audioClip;
        }
        // If no sound effect was found return null
        Debug.LogWarning(name + ": No sound effect named '" + soundEffectname + "' found");

        return null;
    }

}
