using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class AudioManager : Singleton<AudioManager>
{
    /// <summary>
    /// The current volume of the soundeffects played by this Audiomanager.
    /// </summary>
    public float SfxVolume = 1f;

    /// <summary>
    /// The current volume of the music played by this Audiomanager.
    /// </summary>
    public float MusicVolume = 1f;

    /// <summary>
    /// How long does a fade from one music to another take?
    /// </summary>
    public float CrossFadeDuration = 2.5f;

    /// <summary>
    /// Contains the cached audio files, so they don't have to be reloaded from resource every time.
    /// </summary>
    private Dictionary<string, AudioClip> audioCache = new Dictionary<string, AudioClip>();

    /// <summary>
    /// Clips that should be loaded in the cache. To be used in the inspector.
    /// </summary>
    public List<AudioClip> StartCache = new List<AudioClip>();

    private List<AudioSource> sfxSources = new List<AudioSource>();
    private AudioSource musicSource;

    
    void Awake()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.hideFlags = HideFlags.HideInInspector;

        foreach(AudioClip clip in StartCache)
        {
            audioCache[clip.name] = clip;
        }

        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        SfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Example code to cache Audio files at the start of the game
        //GetClip("Music/Combat");
        //GetClip("SFX/UI/mouse_over");
    }

    /// <summary>
    /// Converts the path returned by AssetDatabase.GetAssetPath(Object) to a Path that can be loaded by Resource.Load().
    /// </summary>
    /// <param name="assetPath">The path returned by AssetDatabase.GetAssetPath(Object)</param>
    /// <returns>A Path that can be loaded by Resource.Load().</returns>
    public static string GetResourcePathFromAssetPath(string assetPath)
    {
        //Assets/Resources/SFX/cloth1.ogg
        // -> SFX/cloth1
        if (assetPath.Contains("Resources/"))
        {
            assetPath = assetPath.Substring(assetPath.IndexOf("Resources/") + "Resources/".Length);
            assetPath = assetPath.Substring(0, assetPath.LastIndexOf("."));
            return assetPath;
        }
        return "";
    }

    /// <summary>
    /// Returns the Audioclip for a given resource path. (Example: "SFX/UI/mouse_over")   
    /// Checks if the given audio is already Cached and caches it if not.
    /// </summary>
    /// <param name="resource">The resource path of the audioclip.</param>
    /// <returns>The cached audioclip.</returns>
    private AudioClip GetClip(string resource)
    {
        //Check if clip is already cached.
        if (!audioCache.ContainsKey(resource))
        {
            //If it is not cached, load it from resource
            AudioClip loadedAudioclip = Resources.Load<AudioClip>(resource);
            if(loadedAudioclip != null)
            {
                //If loading from resource didn't fail, cache and return
                audioCache[resource] = loadedAudioclip;
                return loadedAudioclip;
            }
            else
            {
                return null;
            }
        }
        else
        {
            //Just return the clip if it is cached.
            return audioCache[resource];
        }
    }

    /// <summary>
    /// Plays the given effect by adding a new sfxSource and playing the sound on it. Checks if the resource is cached.
    /// </summary>
    /// <param name="resource">The effect to play. Will be cached if not already cached.</param>
    /// <param name="pitch">The pitch in which the sound should be played. Can be used to create variation in sound effects.</param>
    /// <param name="volume">Volume to play the soundeffect in. MAX: 1f</param>
    public void PlayEffect(string resource, float pitch = 1f, float volume = 1f)
    {
        if (volume > 1f)
            volume = 1f;

        AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.hideFlags = HideFlags.HideInInspector;
        sfxSource.clip = GetClip(resource);
        sfxSource.volume = volume * SfxVolume;
        sfxSource.pitch = pitch;
        sfxSource.Play();
        sfxSources.Add(sfxSource);

        float duration = sfxSource.clip.length;
        StartCoroutine(RemoveSfxSource(sfxSource, duration));
    }

    /// <summary>
    /// Play a random sound effect from a list.
    /// </summary>
    /// <param name="effects">The list of sound effects to pick from.</param>
    /// <param name="pitch">The pitch in which the sound should be played. Can be used to create variation in sound effects.</param>
    /// <param name="volume">Volume to play the soundeffect in. MAX: 1f</param>
    public void PlayRandomEffect(List<string> effects, float pitch = 1f, float volume = 1f)
    {
        PlayEffect(effects[Random.Range(0, effects.Count)], pitch, volume);
    }

    /// <summary>
    /// Play a random sound effect from a list.
    /// </summary>
    /// <param name="effects">The list of sound effects to pick from.</param>
    /// <param name="randomPitch">Whether or not to randomize the pitch of the sound effect.</param>
    /// <param name="min">The minimum pitch to randomize to.</param>
    /// <param name="max">The maximum pitch to randomize to.</param>
    /// <param name="volume">Volume to play the soundeffect in. MAX: 1f</param>
    public void PlayRandomEffect(List<string> effects, bool randomPitch, float min = 0.5f, float max = 1f, float volume = 1f)
    {
        PlayEffect(effects[Random.Range(0, effects.Count)], Random.Range(min, max), volume);
    }

    IEnumerator RemoveSfxSource(AudioSource sfxSource, float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(sfxSource);
    }


    /// <summary>
    /// Plays the given Music. Will fade out and in. Checks if the resource is cached.
    /// </summary>
    /// <param name="resource">The music to play. Will be cached if not already cached.</param>
    public void PlayMusic(string resource)
    {
        PlayMusic(GetClip(resource));
    }

    /// <summary>
    /// Private implementation of PlayMusic(string). 
    /// </summary>
    /// <param name="music"></param>
    private void PlayMusic(AudioClip music)
    {
        if (musicSource.isPlaying)
        {
            AudioSource oldSource = musicSource;
            FadeTo(oldSource, CrossFadeDuration, 0f);
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        musicSource.hideFlags = HideFlags.HideInInspector;
        musicSource.clip = music;
        musicSource.volume = 0f;
        musicSource.loop = true;
        musicSource.Play();
        FadeTo(musicSource, CrossFadeDuration, MusicVolume);
    }

    /// <summary>
    /// Set the soundeffect volume.
    /// </summary>
    /// <param name="volume">The new volume.</param>
    public void SetSFXVolume(float volume)
    {
        for (int i = 0; i <= sfxSources.Count - 1; i++)
        {
            AudioSource sfxSource = sfxSources[i];

            if (sfxSource != null)
            {
                sfxSource.volume = volume;
            }
        }

        SfxVolume = volume;

        Debug.Log("SFX" + volume);

        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Set the music volume.
    /// </summary>
    /// <param name="volume">The new volume.</param>
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        MusicVolume = volume;

        Debug.Log("Music" + volume);

        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Fade a audiosource to given values.
    /// </summary>
    /// <param name="audioSource">The audiosource to fade.</param>
    /// <param name="FadeTime">How long should the fade take?</param>
    /// <param name="targetVolume">To what volume should be faded?</param>
    private void FadeTo(AudioSource audioSource, float FadeTime, float targetVolume)
    {
        StartCoroutine(FadeToCoroutine(audioSource, FadeTime, targetVolume));
    }

    IEnumerator FadeToCoroutine(AudioSource audioSource, float FadeTime, float targetVolume)
    {
        float startVolume = audioSource.volume;
        float startTime = Time.time;

        while (Time.time <= startTime + FadeTime)
        {
            float fraction = (Time.time - startTime) / FadeTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, fraction);
            yield return null;
        }

        audioSource.volume = targetVolume;

        if (targetVolume == 0f)
        {
            Destroy(audioSource);
        }
    }
}
