using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class AudioManager : MonoBehaviour , IController
{
    [SerializeField] private AudioClip bgMusic;
    [SerializeField] private AudioClip mainMusic;
    [SerializeField] private AudioSource destroyMusic;
    [SerializeField] private AudioSource buttonClickMusic;
    [SerializeField] private AudioSource notMatchMusic;
    [SerializeField]private AudioSource _audioSource;
    
    private bool _isPlayingBg = true;
    private float _interval = 0.5f;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartAlternatingPlay();
        
        this.RegisterEvent<PlayButtonClickSoundEvent>(e => { OnButtonClickMusicPlayOneShot(); })
            .UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<PlayDestroySoundEvent>(e => { OnDestroyMusicPlayOneShot(); })
            .UnRegisterWhenGameObjectDestroyed(gameObject);
        this.RegisterEvent<PlayNoMatchSoundEvent>(e => { OnNoMatchMusicPlayOneShot(); })
            .UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    private void StartAlternatingPlay()
    {
        StartCoroutine(AlternateMusicRoutine());
    }

    IEnumerator AlternateMusicRoutine()
    {
        while (true)
        {
            if (_isPlayingBg)
            {
                _audioSource.clip = bgMusic;
                _audioSource.Play();

                yield return new WaitForSeconds(bgMusic.length);
            }
            else
            {
                _audioSource.clip = mainMusic;
                _audioSource.Play();
                
                yield return new WaitForSeconds(mainMusic.length);
            }

            _isPlayingBg = !_isPlayingBg;

            if (_interval > 0)
            {
                yield return new WaitForSeconds(_interval);
            }
        }
    }

    public void OnDestroyMusicPlayOneShot()
    {
        destroyMusic.PlayOneShot(destroyMusic.clip);
    }
    
    public void OnButtonClickMusicPlayOneShot()
    {
        buttonClickMusic.PlayOneShot(buttonClickMusic.clip);
    }

    public void OnNoMatchMusicPlayOneShot()
    {
        notMatchMusic.PlayOneShot(notMatchMusic.clip);
    }

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
