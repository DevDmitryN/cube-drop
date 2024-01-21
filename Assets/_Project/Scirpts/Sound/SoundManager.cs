using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _coinSoundObjects;
    
    [SerializeField] private List<GameObject> _strikeSoundObjects;
    
    private List<AudioSource> _coinAudioSources;
    
    private List<AudioSource> _strikeAudioSources;

    private void Awake()
    {
        _coinAudioSources = _coinSoundObjects.Select(x => x.GetComponent<AudioSource>()).ToList();
        _strikeAudioSources = _strikeSoundObjects.Select(x => x.GetComponent<AudioSource>()).ToList();
    }

    public void Strike()
    {
        var randomIndex = new System.Random().Next(0, 6);
        var audioSource = _strikeAudioSources[randomIndex];
        _strikeAudioSources.RemoveAt(randomIndex);
        _strikeAudioSources.Add(audioSource);
        audioSource.Play();
    }

    public void Coin()
    {
        //_coinSource.Play();

        var audioSource = _coinAudioSources[0];
        _coinAudioSources.RemoveAt(0);
        _coinAudioSources.Add(audioSource);
        audioSource.Play();
    }
}
