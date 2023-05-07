using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _clips;

    public static AudioManager Instance {get; private set;}

    private void Awake() => Instance = this;

    public void PlayAudio(int index) {
        _audioSource.PlayOneShot(_clips[index]);
    }
}