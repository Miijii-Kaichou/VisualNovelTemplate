using System;
using UnityEngine;

[Serializable]
public class Voice
{
    public string voiceName
    {
        get
        {
            return voice.name;
        }
    }
    public AudioClip voice;
}