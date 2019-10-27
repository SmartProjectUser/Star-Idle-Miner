using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public AudioSource AudioStock;
    public AudioSource AudioMain;

    public List<AudioClip> MainClips;
    public Button NextTrackButton;
    public int NextTrack;
    public bool IsAnimating;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayMain();
        AudioStock.enabled = false;
        NextTrackButton.onClick.AddListener(NextTrackPlay);
    }

    private void NextTrackPlay()
    {
        if (IsAnimating)
            return;
        
        NextTrack++;
        if (NextTrack >= MainClips.Count)
            NextTrack = 0;

        IsAnimating = true;
        FadeMain(() =>
        {
            AudioMain.clip= MainClips[NextTrack];
            PlayMain();
            IsAnimating = false;
            //StartCoroutine(FadeIn(AudioMain, 1.0f, () => { IsAnimating = false; }));
        });
    }

    public void FadeMain(Action callback = null)
    {
        StartCoroutine(FadeOut(AudioMain, 1.0f, () =>
        {
            AudioMain.enabled = false;
            callback?.Invoke();
        }));
    }
    
    public void FadeStock()
    {
        StartCoroutine(FadeOut(AudioStock, 1.0f, () => { AudioStock.enabled = false; }));
    }

    public void PlayMain()
    {
        StopAllCoroutines();
        AudioMain.enabled = true;
        AudioMain.volume = 1.0f;
        AudioMain.Play();
        AudioStock.enabled = false;
        AudioStock.volume = 0.0f;
        AudioStock.Stop();
    }
    
    public void PlayStocks()
    {
        StopAllCoroutines();
        AudioStock.enabled = true;
        AudioStock.volume = 1.0f;
        AudioStock.Play();
        AudioMain.enabled = false;
        AudioMain.volume = 0.0f;
        AudioMain.Stop();
    }

    public static IEnumerator FadeOut (AudioSource audioSource, float FadeTime, Action callback=null) {
        float startVolume = audioSource.volume;
 
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
 
            yield return null;
        }
 
        audioSource.Stop ();
        audioSource.volume = startVolume;
        callback?.Invoke();
    }
}
