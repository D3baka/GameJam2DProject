using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// The AudioManager class is responsible for managing the audio in the game. This includes setting volume of channels and saving these setting to the player preferences./// 
/// </summary>
public class AudioManager : MonoBehaviour
{
    
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer;

    //doing things with strings is dirty but sadly the only way in Unity. Like this we at least only have to worry about the names once.
    //If you change the names in the AudioMixer you have to change them here as well.
    private readonly string AUDIOMIXER_MASTERVOLUME = "AudioMixer_MasterVolume";
    private readonly string AUDIOMIXER_MUSICVOLUME = "AudioMixer_MusicVolume";
    private readonly string AUDIOMIXER_SFXVOLUME = "AudioMixer_SfxVolume";

      
    private void Awake() {
        if (AudioManager.Instance == null) {
            Instance = this;
        }
        else {
            Debug.LogError("Too many AudioManager Instances: " + AudioManager.Instance);
        }      
    }

    private void Start() {

        if (PlayerPrefs.HasKey(AUDIOMIXER_MASTERVOLUME)) {
            SetVolume(AudioGroup.Master, PlayerPrefs.GetFloat(AUDIOMIXER_MASTERVOLUME));
            //Debug.Log("Set Master volume to saved value: " + PlayerPrefs.GetFloat(AUDIOMIXER_MASTERVOLUME));
        }
        else {
            SetVolume(AudioGroup.Master, 0.5f);
        }

        if (PlayerPrefs.HasKey(AUDIOMIXER_MUSICVOLUME)) {
            SetVolume(AudioGroup.Music, PlayerPrefs.GetFloat(AUDIOMIXER_MUSICVOLUME));
        }
        else {
            SetVolume(AudioGroup.Music, 0.5f);
        }

        if (PlayerPrefs.HasKey(AUDIOMIXER_SFXVOLUME)) {
            SetVolume(AudioGroup.SFX, PlayerPrefs.GetFloat(AUDIOMIXER_SFXVOLUME));
        }
        else {
            SetVolume(AudioGroup.SFX, 0.5f);
        }
    }

    /// <summary>
    /// Set the volume of a specific audio group.
    /// </summary>
    /// <param name="audioGroup">The <c>AudioGroup</c> the volume is changed for</param>
    /// <param name="value">the volume that is to be set</param>
    public void SetVolume(AudioGroup audioGroup, float value) {
        switch (audioGroup) {
            case AudioGroup.Master: 
                {
                    audioMixer.SetFloat(AUDIOMIXER_MASTERVOLUME, value);
                    PlayerPrefs.SetFloat(AUDIOMIXER_MASTERVOLUME, value);
                    //Debug.Log("Setting Master Volume preference to: " + value);
                    break;
                }
            case AudioGroup.Music: 
                {
                    audioMixer.SetFloat(AUDIOMIXER_MUSICVOLUME, value);
                    PlayerPrefs.SetFloat (AUDIOMIXER_MUSICVOLUME, value);
                    //Debug.Log("Set MusicVol to " + value);
                    break;
                }
            case AudioGroup.SFX: {
                    audioMixer.SetFloat(AUDIOMIXER_SFXVOLUME, value);
                    PlayerPrefs.SetFloat(AUDIOMIXER_SFXVOLUME, value);
                    //Debug.Log("Set SFXVol to " + value);
                    break;
                } 
        }
    }
    /// <summary>
    /// Get the volume of a specific audio group.
    /// </summary>
    /// <param name="audioGroup"> the <c>AudioGroup</c> of which you want to get the Volume</param>
    /// <returns></returns>
    public float GetVolume(AudioGroup audioGroup) {
        float result = 0.5f;
        switch (audioGroup) {
            case AudioGroup.Master: {
                    audioMixer.GetFloat(AUDIOMIXER_MASTERVOLUME, out result);
                    //Debug.Log("Returning volume for master: " + result);
                    break;
                }
            case AudioGroup.Music: {
                    audioMixer.GetFloat(AUDIOMIXER_MUSICVOLUME, out result);
                    break;
                }
            case AudioGroup.SFX: {
                    audioMixer.GetFloat(AUDIOMIXER_SFXVOLUME, out result);
                    break;
                }
        }
            return result;
        }
        
        
    // if you want to add more sliders to the Audiomixer, you have to add them here as well
    public enum AudioGroup {
        Master,
        Music,
        SFX,
    }
}
