using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle muteToggle;

    [Header("Slider value")]
    [SerializeField] TextMeshProUGUI musicSliderValueTMP;
    [SerializeField] TextMeshProUGUI sfxSliderValueTMP;

    private void Start()
    {
        LoadSettings();
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        muteToggle.onValueChanged.AddListener(SetMute);
    }

    private void SetMusicVolume(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
        FormatValueSliderTMP(musicSliderValueTMP, volume);
    }

    private void SetSFXVolume(float volume)
    {
        AudioManager.Instance.SetSFXVolume(volume);
        FormatValueSliderTMP(sfxSliderValueTMP, volume);
    }

    // Convierte de escala lineal (0.0 a 1.0) a dB (entre -80 y 0 aprox.)
    private float LinearToDecibel(float linear)
    {
        return Mathf.Log10(Mathf.Max(linear, 0.0001f)) * 20f;
    }

    private void SetMute(bool isMuted)
    {
        AudioManager.Instance.MuteAudio(isMuted);
    }

    private void LoadSettings()
    {
        // Cargar valores almacenados previamente o valores por defecto
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        bool isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;

        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
        muteToggle.isOn = !isMuted;

        AudioManager.Instance.SetMusicVolume(musicVolume);
        AudioManager.Instance.SetSFXVolume(sfxVolume);
        AudioManager.Instance.MuteAudio(!isMuted);

        FormatValueSliderTMP(musicSliderValueTMP, musicVolume);
        FormatValueSliderTMP(sfxSliderValueTMP, sfxVolume);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.SetInt("Muted", muteToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void FormatValueSliderTMP(TextMeshProUGUI tmp, float volume)
    {
        tmp.text = $"{Mathf.RoundToInt(volume * 100)}%";
    }
}
