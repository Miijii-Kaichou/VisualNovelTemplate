using UnityEngine;

public class VoiceManager : Singleton<VoiceManager>
{
    static Voice CurrentVoice;
    AudioSource source;
    
    /// <summary>
    /// Checks if the voice manager had been initialized
    /// </summary>
    public static bool IsInitialized { get; private set; }

    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// Initialize the VoiceManager
    /// </summary>
    void Init()
    {
        if (!IsInitialized)
        {
            source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            IsInitialized = true;
            return;
        }
    }

    /// <summary>
    /// Play a Character Voice
    /// </summary>
    /// <param name="VoiceName"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    public static void PlayVoice(Voice voice, float volume = 1f, float pitch = 1f)
    {
        if (!IsInitialized) return;

        CurrentVoice = voice;
        Debug.Log($"Playing Voice {CurrentVoice.voiceName}");

        Instance.source.clip = CurrentVoice.voice;
        Instance.source.volume = volume;
        Instance.source.pitch = pitch;

        //Play the voice, and set the volume and pitch
        Instance.source.Play();
    }
}