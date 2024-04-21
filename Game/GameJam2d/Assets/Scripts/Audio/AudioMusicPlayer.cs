using System.Collections.Generic;
using UnityEngine;

public class AudioMusicPlayer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> songlist;

    [SerializeField] private AudioSource audioSource;

    private int songIndex = 0;

    private void Update()
    {
        audioSource.volume = AudioManager.Instance.GetVolume(AudioManager.AudioGroup.Music);
        if(!audioSource.isPlaying)
            PlayNextSong();
    }
    private void PlayNextSong()
    {
        audioSource.clip = songlist[songIndex];
        audioSource.Play();
        songIndex++;
        if(songIndex >= songlist.Count)
        {
            songIndex = 0;
        }
    }
}
