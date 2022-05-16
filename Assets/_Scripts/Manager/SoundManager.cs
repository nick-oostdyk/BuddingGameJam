using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _i;
    public static SoundManager Instance => _i;

    [SerializeField] private AudioSource[] _soundEffects;
    [SerializeField] private AudioSource _music;

    private void Awake()
    {
        if (_i == null) _i = this;
        else Destroy(this);
    }

    public void PlaySound(AudioClip _a, int _i)
    {
        _soundEffects[_i].PlayOneShot(_a);
    }

    public void ChangeMasterVolume(float _v)
    {
        AudioListener.volume = _v;
    }

    public void ToggleEffects()
    {
        foreach (var e in _soundEffects)
             e.mute = !e.mute;
    }
    public void ToggleMusic()
    {
        _music.mute = !_music.mute;
    }
}
