using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip[] backgroundMusic;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if(backgroundMusic.Length > 0)
        {
            PlayMusic(backgroundMusic[0]);
        }
    }

    #region Music Functions
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if(bgmSource.isPlaying)
            bgmSource.Stop();

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void PlayMusic(int index, bool loop = true)
    {
        if(index > backgroundMusic.Length)
        {
            Debug.LogWarning($"Indice no valido {index}");
        }

        if(bgmSource.isPlaying)
            bgmSource.Stop();

        bgmSource.clip = backgroundMusic[index];
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void StopMusic()
    {
        bgmSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        bgmSource.volume = Mathf.Clamp01(volume);
    }
    #endregion

    #region SFX Functions
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp01(volume);
    }
    #endregion

    #region Utility Functions
    public void MuteAudio(bool mute)
    {
        bgmSource.mute = mute;
        sfxSource.mute = mute;
    }

    public bool IsMusicPlaying()
    {
        return bgmSource.isPlaying;
    }
    #endregion
}
