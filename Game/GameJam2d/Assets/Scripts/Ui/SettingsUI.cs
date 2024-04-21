using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Slider masterSlider, musicSlider, sfxSlider;
    [SerializeField] private Button resetValuesButton;
    [SerializeField] private Button returnButton;

    private void Awake() {
        masterSlider.onValueChanged.AddListener(SetMasterVol);
        musicSlider.onValueChanged.AddListener (SetMusicVol);
        sfxSlider.onValueChanged.AddListener(SetSFXVol);
        returnButton.onClick.AddListener(ReturnButtonOnClick);
        resetValuesButton.onClick.AddListener(ResetValuesButtonOnClick);
    }
    private void OnEnable() {
        masterSlider.value = AudioManager.Instance.GetVolume(AudioManager.AudioGroup.Master);
        musicSlider.value = AudioManager.Instance.GetVolume(AudioManager.AudioGroup.Music);
        sfxSlider.value = AudioManager.Instance.GetVolume(AudioManager.AudioGroup.SFX);
    }

    private void SetMasterVol(float value)
    {
        AudioManager.Instance.SetVolume(AudioManager.AudioGroup.Master, value);
    }
    private void SetMusicVol(float value)
    {
        AudioManager.Instance.SetVolume(AudioManager.AudioGroup.Music, value);
    }

    private void SetSFXVol(float value)
    {
        AudioManager.Instance.SetVolume(AudioManager.AudioGroup.SFX, value);
    }

    private void ReturnButtonOnClick() {
        UiManager.Instance.ExitSettings();
    }

    private void ResetValuesButtonOnClick() {
        AudioManager.Instance.SetVolume(AudioManager.AudioGroup.Master, 0.5f);
        AudioManager.Instance.SetVolume(AudioManager.AudioGroup.Music, 0.5f);
        AudioManager.Instance.SetVolume(AudioManager.AudioGroup.SFX, 0.5f);
        masterSlider.value = AudioManager.Instance.GetVolume(AudioManager.AudioGroup.Master);
        musicSlider.value = AudioManager.Instance.GetVolume(AudioManager.AudioGroup.Music);
        sfxSlider.value = AudioManager.Instance.GetVolume(AudioManager.AudioGroup.SFX);
    }
}
