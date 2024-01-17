using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] private GameObject _coinSoundObject;

    [SerializeField] private float _strikeSoundVolume = 0.7f;

    private AudioSource _coinSource;

    private List<AudioClip> _strikes = new List<AudioClip>();

    private AudioSource _audioSource;

    private void Awake()
    {
        _coinSource = _coinSoundObject.GetComponent<AudioSource>();

        _audioSource = GetComponent<AudioSource>();

        for (int i = 1; i <= 10; i++)
        {
            _strikes.Add(Resources.Load<AudioClip>($"sounds/strikes/strike {i}"));
        }

    }

    public void Strike()
    {
        var random = new System.Random().Next(0, 10);
        _audioSource.clip = _strikes[random];
        _audioSource.volume = _strikeSoundVolume;
        _audioSource.Play();
        //_audioSource.PlayOneShot(_strikes[random]);
    }

    public void Coin()
    {
        _coinSource.Play();
    }
}
