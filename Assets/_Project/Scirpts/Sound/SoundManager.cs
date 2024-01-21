using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _coinSoundObjects;

    private List<AudioSource> _coinAudioSoruces;
    
    private const float STRIKE_SOUND_VOLUME = 0.5f;

    private AudioSource _coinSource;

    private List<AudioClip> _strikes = new List<AudioClip>();

    private AudioSource _audioSource;

    private void Awake()
    {
        _coinAudioSoruces = _coinSoundObjects.Select(x => x.GetComponent<AudioSource>()).ToList();
        
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
        _audioSource.volume = STRIKE_SOUND_VOLUME;
        _audioSource.Play();
        //_audioSource.PlayOneShot(_strikes[random]);
    }

    public void Coin()
    {
        //_coinSource.Play();

        var audioSource = _coinAudioSoruces[0];
        _coinAudioSoruces.RemoveAt(0);
        _coinAudioSoruces.Add(audioSource);
        audioSource.Play();
    }
}
