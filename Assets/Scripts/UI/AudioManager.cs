using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public AudioSource[] music;
    public AudioSource[] sfx;

    public int levelMusicToPlay;

    public AudioMixerGroup MasterMixer ,musicMixer, sfxMixer;



    public int CurrentTrack;

    private void Awake()
    {
        instance = this;
    }


    /*
    public void PlayMusic(int MusicToPlay)
    {
        for(int i = 0; i < music.Length; i++)
        {
            music[i].Stop();
        }

        //music[MusicToPlay].Play();
    }

    public void PlaySFX(int SFXtoPlay)
    {
        sfx[SFXtoPlay].Play();
    }


    /* example for playing audio on a pickup
     * AudioManager.instance.PlaySFX(soundtoplay);
     * Remember to add a public int on the seperate script like:
     * public int soundtoplay;
     */

    public void SetMasterLevel()
    {
        musicMixer.audioMixer.SetFloat("MasterVolControl", UIManager.instance.MasterSlide.value);
    }

    public void SetMusicLevel()
    {
        musicMixer.audioMixer.SetFloat("MusicVolControl", UIManager.instance.MusicSlide.value);
    }

    public void SetSFXLevel()
    {
        sfxMixer.audioMixer.SetFloat("SFXComVolControl", UIManager.instance.SFXSlide.value);
    }

    public void PlaySFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Play();
    }

}
