using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _coinSoundObjects;
    
    [SerializeField] private List<GameObject> _strikeSoundObjects;
    
    [SerializeField] private List<GameObject> _jumpSoundObjects;

    [SerializeField] private List<GameObject> _gaykaSoundObjects;
    
    private List<AudioSource> _coinAudioSources;
    
    private List<AudioSource> _strikeAudioSources;
    
    private List<AudioSource> _jumpAudioSources;

    private List<AudioSource> _gaykaAudioSources;

    private void Awake()
    {
        _coinAudioSources = _coinSoundObjects.Select(x => x.GetComponent<AudioSource>()).ToList();
        _strikeAudioSources = _strikeSoundObjects.Select(x => x.GetComponent<AudioSource>()).ToList();
        _jumpAudioSources = _jumpSoundObjects.Select(x => x.GetComponent<AudioSource>()).ToList();
        _gaykaAudioSources = _gaykaSoundObjects.Select(x => x.GetComponent<AudioSource>()).ToList();
    }

    private void PlayFromList(List<AudioSource> list, int index)
    {
        var audioSource = list[index];
        list.RemoveAt(index);
        list.Add(audioSource);
        audioSource.Play();
    }
    
    public void Strike()
    {
        var randomIndex = new System.Random().Next(0, 6);
        PlayFromList(_strikeAudioSources, randomIndex);
    }

    public void Coin()
    {
        PlayFromList(_coinAudioSources, 0);
    }

    public void Jump()
    {
        PlayFromList(_jumpAudioSources, 0);
    }
    
    public void SpeedUp()
    {
        PlayFromList(_gaykaAudioSources, 0);
    }
}
