using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;
    public static GameAssets GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }
    public Sprite pipeSprite;
    public Transform pfPipeBody;
    public Transform pfPipeHead;
    public Transform flower;
    public Transform ground;
    public Transform cloud;


    public SoundAudioClip[] soundAudioClipArray;

    [Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }
}
