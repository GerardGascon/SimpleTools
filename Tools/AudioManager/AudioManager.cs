using System.Collections;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour{

    public static AudioManager instance;

    [SerializeField] Sounds soundList = default;

    void Awake(){
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach(Sounds.List s in soundList.sounds){
            if(string.IsNullOrEmpty(s.name) || string.IsNullOrWhiteSpace(s.name)){
                Debug.LogWarning("The name one sound is empty");
                continue;
            }

            GameObject sound = new GameObject(s.name);
            sound.transform.parent = transform;
            s.source = sound.AddComponent<AudioSource>();

            if(soundList.mainMixer && soundList.sfxMixer){
                if (s.type == Sounds.List.Type.Music)
                    s.source.outputAudioMixerGroup = soundList.mainMixer;
                else
                    s.source.outputAudioMixerGroup = soundList.sfxMixer;
            }

            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    #region Play
    public void Play(string name){
        Sounds.List s = Array.Find(soundList.sounds, sound => sound.name == name);
        if(s == null){
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.pitch = s.RandomPitch;
        s.source.volume = s.RandomVolume;
        s.source.Play();
    }
    public void Play(string name, float delay){
        Sounds.List s = Array.Find(soundList.sounds, sound => sound.name == name);
        if (s == null){
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.pitch = s.RandomPitch;
        s.source.volume = s.RandomVolume;
        s.source.PlayDelayed(delay);
    }
    public void PlayOneShot(string name){
        Sounds.List s = Array.Find(soundList.sounds, sound => sound.name == name);
        if (s == null){
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.pitch = s.RandomPitch;
        s.source.volume = s.RandomVolume;
        s.source.PlayOneShot(s.clip);
    }
    #endregion
    #region Pause
    public void Pause(string name){
        Sounds.List s = Array.Find(soundList.sounds, sound => sound.name == name);
        if (s == null){
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.pitch = s.RandomPitch;
        s.source.volume = s.RandomVolume;
        s.source.Pause();
    }
    public void UnPause(string name){
        Sounds.List s = Array.Find(soundList.sounds, sound => sound.name == name);
        if (s == null){
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.pitch = s.RandomPitch;
        s.source.volume = s.RandomVolume;
        s.source.UnPause();
    }
    #endregion
    #region Stop
    public void Stop(string name){
        Sounds.List s = Array.Find(soundList.sounds, sound => sound.name == name);
        if (s == null){
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.pitch = s.RandomPitch;
        s.source.volume = s.RandomVolume;
        s.source.Stop();
    }
    public void StopAll(){
        foreach (Sounds.List s in soundList.sounds){
            if (s.source){
                s.source.Stop();
            }
        }
    }
    #endregion
    public AudioSource GetSource(string name){
        Sounds.List s = Array.Find(soundList.sounds, sound => sound.name == name);
        if (s == null){
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }
        return s.source;
    }
    #region Fades
    public void FadeIn(string name, float duration){
        StartCoroutine(FadeInCoroutine(name, duration));
    }
    IEnumerator FadeInCoroutine(string name, float fadeTime){
        AudioSource audioSource = GetSource(name);
        if (audioSource != null && !audioSource.isPlaying){
            float volume = audioSource.volume;
            audioSource.volume = 0;
            audioSource.Play();
            while (audioSource.volume < volume){
                audioSource.volume += Time.deltaTime / fadeTime;
                yield return null;
            }

            audioSource.volume = volume;
        }
    }
    public void FadeOut(string name, float duration){
        StartCoroutine(FadeOutCoroutine(name, duration));
    }
    IEnumerator FadeOutCoroutine(string name, float fadeTime){
        AudioSource audioSource = GetSource(name);

        if (audioSource && audioSource.isPlaying){
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0){
                audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
        }
    }

    public void PlayMuted(string name){
        Sounds.List s = Array.Find(soundList.sounds, sound => sound.name == name);
        if (s == null){
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.pitch = s.RandomPitch;
        s.source.volume = 0f;
        s.source.Play();
    }
    public void FadeMutedIn(string name, float duration){
        StartCoroutine(FadeMutedInCoroutine(name, duration));
    }
    IEnumerator FadeMutedInCoroutine(string name, float fadeTime){
        Sounds.List s = Array.Find(soundList.sounds, sound => sound.name == name);
        if (s == null){
            Debug.LogWarning("Sound: " + name + " not found!");
            yield break;
        }

        while (s.source.volume < s.volume){
            s.source.volume += Time.deltaTime / fadeTime;
            yield return null;
        }
        s.source.volume = s.volume;
    }
    public void FadeMutedOut(string name, float duration){
        StartCoroutine(FadeMutedOutCoroutine(name, duration));
    }
    IEnumerator FadeMutedOutCoroutine(string name, float fadeTime){
        Sounds.List s = Array.Find(soundList.sounds, sound => sound.name == name);
        if (s == null){
            Debug.LogWarning("Sound: " + name + " not found!");
            yield break;
        }

        while (s.source.volume > 0){
            s.source.volume -= Time.deltaTime / fadeTime;
            yield return null;
        }
        s.source.volume = 0;
    }
    #endregion
}
