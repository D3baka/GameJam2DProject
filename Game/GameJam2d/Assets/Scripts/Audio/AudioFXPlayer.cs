using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioFXPlayer : MonoBehaviour
{
    public static AudioFXPlayer Instance { get; private set; }

    

    [SerializeField] private AudioClip randomMoveSound;
    [SerializeField] private AudioClip getCoinSound;
    [SerializeField] private AudioClip shipHitSound;
    [SerializeField] private AudioClip projectileHitSound;
    [SerializeField] private AudioClip projectileFireSound;
    [SerializeField] private AudioClip getShieldSound;

    [SerializeField] private AudioClip gameOverSound;


    private void Awake()
    {
        if (AudioFXPlayer.Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Too many AudioFXPlayer Instances: " + AudioFXPlayer.Instance);
        }
    }

    public void PlaySound(SoundEffect sound)
    {
        AudioClip clipToPlay = null;

        switch (sound)
        {
            case SoundEffect.randomMoveSound:
                clipToPlay = randomMoveSound;
                break;
            case SoundEffect.getCoinSound:
                clipToPlay = getCoinSound;
                break;
            case SoundEffect.shipHitSound:
                clipToPlay = shipHitSound;
                break;
            case SoundEffect.projectileHitSound:
                clipToPlay = projectileHitSound;
                break;
            case SoundEffect.projectileFireSound:
                clipToPlay = projectileFireSound;
                break;
            case SoundEffect.gameOverSound:
                clipToPlay = gameOverSound;
                break;
            case SoundEffect.getShieldSound:
                clipToPlay = getShieldSound;
                break;

        }

        if (clipToPlay != null)
        {
            AudioSource.PlayClipAtPoint(clipToPlay,Camera.main.transform.position, AudioManager.Instance.GetVolume(AudioManager.AudioGroup.SFX));
        }
        else
        {
            Debug.LogWarning("Sound clip not found for: " + sound);
        }        
     }

    public enum SoundEffect
    {
        randomMoveSound,
        getCoinSound,
        shipHitSound,
        projectileHitSound,
        projectileFireSound,
        gameOverSound,
        getShieldSound,

        // Add more sound effects here
    }
}
