using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    // Public Variables
    public AudioClip MainMenuMusic;
    public AudioClip GameplayMusic;

    // Private Variables
    [HideInInspector] private AudioSource _audioSource;

    public static SoundManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundEffect(AudioClip SFX, float volume = 1f, float pitch = 1f)
    {

    }

    public void PlayBackgroundMusic(float volume = 1f, bool loop = true)
    {
        //_audioSource.clip = BackgroundMusic;
        _audioSource.volume = volume;
        _audioSource.loop = loop;
        _audioSource.Play();
    }

    public void StopBackgroundMusic()
    {
        if (_audioSource != null)
            _audioSource.Stop();
    }

    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void SetBackgroundMusicVolume(float volume)
    {
        if (_audioSource != null)
            _audioSource.volume = volume;
    }

    public void SetSoundEffectVolume(float volume)
    {
        foreach (AudioSource source in GetComponentsInChildren<AudioSource>())
        {
            if (source != _audioSource)
                source.volume = volume;
        }
    }
}