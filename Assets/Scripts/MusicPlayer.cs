using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    
    public void SetVolume(float volume) => _audioSource.volume = volume;
}
