using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _buttonClip;
    [SerializeField] private AudioClip _popupClip;
    [SerializeField] private AudioClip _actionClip;
    
    public void PlayButtonSound() => _audioSource.PlayOneShot(_buttonClip);
    public void PlayPopupSound() => _audioSource.PlayOneShot(_popupClip);
    public void PlayActionSound() => _audioSource.PlayOneShot(_actionClip);
    
    public void SetVolume(float volume) => _audioSource.volume = volume;
}
