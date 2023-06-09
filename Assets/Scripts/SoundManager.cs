using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager 
{
   public enum Sound
    {
        BirdJump,
        Lose,
        ScoreSound,
        ButtonOver,
        ButtonClick,
    }
    public static void PlaySound(Sound sound)
    {
        GameObject gameObject = new GameObject("sound", typeof(AudioSource));
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound));
    }
    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.GetInstance().soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found");
        return null;
    }
}
