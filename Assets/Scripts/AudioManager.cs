using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip _audioClip1;
    [SerializeField]
    private AudioClip _audioClip2;
    [SerializeField]
    private AudioClip _audioClip3;
    [SerializeField]
    private AudioClip _audioClip4;
    [SerializeField]
    private AudioClip _audioClip5;

    [SerializeField]
    private GameObject _backgroundMusic;
    private AudioSource _audio;
    private Scene _currentScene;

    

    // Start is called before the first frame update
    void Start()
    {
        _audio = _backgroundMusic.GetComponent<AudioSource>();
        if (_audio == null)
        {
            Debug.Log("background audio source is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Music1()
    {
        _audio.clip = _audioClip1;
        _audio.Play();
    }

    public void Music2()
    {
        _audio.clip = _audioClip2;
        _audio.Play();
    }

    public void Music3()
    {
        _audio.clip = _audioClip3;
        _audio.Play();
    }

    public void Music4()
    {
        _audio.clip = _audioClip4;
        _audio.Play();
    }

    public void Music5()
    {
        _audio.clip = _audioClip5;
        _audio.Play();
    }
}
