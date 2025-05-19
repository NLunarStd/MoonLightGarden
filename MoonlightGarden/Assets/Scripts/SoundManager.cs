using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public List<AudioClip> bgmClips;
    public AudioClip bgmSound;

    public AudioSource bgmSource;
    public AudioSource playerSource;
    public AudioSource monsterSource;

    public bool isMasterOn;
    public bool isBGMOn;
    public bool isSFXOn;

    public AudioMixer masterMixer;
    public AudioMixerSnapshot playSnapShot;
    public AudioMixerSnapshot pauseSnapshot;

    private void Start()
    {
        if(bgmClips != null && bgmClips.Count > 0)
        {
            bgmSound = bgmClips[0];
            bgmSource.clip = bgmSound;
            bgmSource.Play();
        }
    }
    public void SetMasterVolume(float soundLevel)
    {
        masterMixer.SetFloat("masterVolume", soundLevel);
    }
    public void SetSFXVolume(float soundLevel)
    {
        masterMixer.SetFloat("sfxVolume", soundLevel);
    }
    public void SetBGMVolume(float soundLevel)
    {
        masterMixer.SetFloat("bgmVolume", soundLevel);
    }

    public void ToggleMasterVolume(bool toggle)
    {
        isMasterOn = !isMasterOn;
        if(isMasterOn)
        masterMixer.SetFloat("masterVolume", 0);
        else
        masterMixer.SetFloat("masterVolume",-80);
    }
    public void ToggleSFXVolume(bool toggle)
    {
        isSFXOn = !isSFXOn;
        if (isSFXOn)
            masterMixer.SetFloat("sfxVolume", 0);
        else
            masterMixer.SetFloat("sfxVolume", -80);
    }
    public void ToggleBGMVolume(bool toggle)
    {
        isBGMOn = !isBGMOn;
        if (isBGMOn)
            masterMixer.SetFloat("bgmVolume", -10);
        else
            masterMixer.SetFloat("bgmVolume", -80);
    }

    public void PlayOneShotWithVaryPitch(AudioSource audioSource,AudioClip audioClip)
    {
        float randomPitch = Random.Range(1.0f, 3.0f);
        audioSource.pitch = randomPitch;
        audioSource.PlayOneShot(audioClip);
    }

    public AudioClip gameOverSound;
    public AudioClip calculatedSound;
    public AudioClip getShardSound;
}
