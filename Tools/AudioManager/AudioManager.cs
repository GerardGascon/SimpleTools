using System.Collections;
using UnityEngine;
using System;

namespace SimpleTools.AudioManager {
	public class AudioManager : MonoBehaviour {

		public static AudioManager instance;

		[SerializeField] Sounds soundList = default;

		void Awake() {
			if (instance == null) {
				instance = this;
			} else {
				Destroy(gameObject);
				return;
			}
			DontDestroyOnLoad(gameObject);

			foreach (Sounds.List s in soundList.sounds) {
				if (string.IsNullOrEmpty(s.name) || string.IsNullOrWhiteSpace(s.name)) {
					Debug.LogWarning("The name one sound is empty");
					continue;
				}

				GameObject sound = new GameObject(s.name);
				sound.transform.parent = transform;
				s.source = sound.AddComponent<AudioSource>();

				if (soundList.mainMixer && soundList.sfxMixer) {
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
		/// <summary>Use this to play a sound with a specific name
		/// <para>It has to be in the Sound asset referenced in the AudioManager instance</para>
		/// </summary>
		/// <param name="name" type="string">The name of the sound</param>
		public void Play(string name) {
			Sounds.List s = Array.Find(soundList.sounds, sound => sound.name == name);
			if (s == null) {
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}
			s.source.pitch = s.RandomPitch;
			s.source.volume = s.RandomVolume;
			s.source.Play();
		}
		/// <summary>Use this to play a sound with a specific name and with a certain delay
		/// <para>It has to be in the Sound asset referenced in the AudioManager instance</para>
		/// </summary>
		/// <param name="name" type="string">The name of the sound</param>
		/// <param name="delay" type="float">The delay in seconds</param>
		public void Play(string name, float delay) {
			Sounds.List s = Array.Find(soundList.sounds, sound => sound.name == name);
			if (s == null) {
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}
			s.source.pitch = s.RandomPitch;
			s.source.volume = s.RandomVolume;
			s.source.PlayDelayed(delay);
		}
		/// <summary>Use this to play one shot of a sound with a specific name
		/// <para>It has to be in the Sound asset referenced in the AudioManager instance</para>
		/// </summary>
		/// <param name="name" type="string">The name of the sound</param>
		public void PlayOneShot(string name) {
			Sounds.List s = Array.Find(soundList.sounds, sound => sound.name == name);
			if (s == null) {
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}
			s.source.pitch = s.RandomPitch;
			s.source.volume = s.RandomVolume;
			s.source.PlayOneShot(s.clip);
		}
		/// <summary>Use this to play an intro song and then start playing the song loop
		/// <para>It has to be in the Sound asset referenced in the AudioManager instance</para>
		/// </summary>
		/// <param name="intro" type="string">The name of the intro song</param>
		/// <param name="song" type="string">The name of the song loop</param>
		public void PlayWithIntro(string intro, string song) {
			Sounds.List s = Array.Find(soundList.sounds, sound => sound.name == intro);
			if (s == null) {
				Debug.LogWarning("Sound: " + intro + " not found!");
				return;
			}
			s.source.pitch = s.RandomPitch;
			s.source.volume = s.RandomVolume;
			s.source.Play();

			float introDuration = s.clip.length;
			Play(song, introDuration);
		}
		/// <summary>Use this to play one shot of a random sound within a list
		/// <para>They have to be in the Sound asset referenced in the AudioManager instance</para>
		/// </summary>
		/// <param name="names" type="string[]">The names of the sounds</param>
		public void PlayRandomSound(params string[] names) {
			int random = UnityEngine.Random.Range(0, names.Length);
			PlayOneShot(names[random]);
		}
		#endregion
		#region Pause
		/// <summary>Use this to pause a sound with a specific name
		/// <para>It has to be in the Sound asset referenced in the AudioManager instance</para>
		/// </summary>
		/// <param name="name" type="string">The name of the sound</param>
		public void Pause(string name) {
			Sounds.List s = Array.Find(soundList.sounds, sound => sound.name == name);
			if (s == null) {
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}
			s.source.pitch = s.RandomPitch;
			s.source.volume = s.RandomVolume;
			s.source.Pause();
		}
		/// <summary>Use this to unpause a sound with a specific name
		/// <para>It has to be in the Sound asset referenced in the AudioManager instance</para>
		/// </summary>
		/// <param name="name" type="string">The name of the sound</param>
		public void UnPause(string name) {
			Sounds.List s = Array.Find(soundList.sounds, sound => sound.name == name);
			if (s == null) {
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}
			s.source.pitch = s.RandomPitch;
			s.source.volume = s.RandomVolume;
			s.source.UnPause();
		}
		#endregion
		#region Stop
		/// <summary>Use this to stop a sound with a specific name
		/// <para>It has to be in the Sound asset referenced in the AudioManager instance</para>
		/// </summary>
		/// <param name="name" type="string">The name of the sound</param>
		public void Stop(string name) {
			Sounds.List s = Array.Find(soundList.sounds, sound => sound.name == name);
			if (s == null) {
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}
			s.source.pitch = s.RandomPitch;
			s.source.volume = s.RandomVolume;
			s.source.Stop();
		}
		/// <summary>Use this to stop all the sounds
		/// <para>It has to be in the Sound asset referenced in the AudioManager instance</para>
		/// </summary>
		public void StopAll() {
			foreach (Sounds.List s in soundList.sounds) {
				if (s.source) {
					s.source.Stop();
				}
			}
		}
		#endregion
		/// <summary>This function returns the AudioSource that contains a specific sound
		/// <para>It has to be in the Sound asset referenced in the AudioManager instance</para>
		/// </summary>
		/// <param name="name">The name of the sound</param>
		/// <returns>The AudioSource in the scene</returns>
		public AudioSource GetSource(string name) {
			Sounds.List s = Array.Find(soundList.sounds, sound => sound.name == name);
			if (s == null) {
				Debug.LogWarning("Sound: " + name + " not found!");
				return null;
			}
			return s.source;
		}
		#region Fades
		/// <summary>Use this to start playing a sound with a fade in
		/// <para>It has to be in the Sound asset referenced in the AudioManager instance</para>
		/// </summary>
		/// <param name="name" type="string">The name of the sound</param>
		/// <param name="duration" type="float">The duration of the fade in</param>
		public void FadeIn(string name, float duration) {
			StartCoroutine(FadeInCoroutine(name, duration));
		}
		IEnumerator FadeInCoroutine(string name, float fadeTime) {
			AudioSource audioSource = GetSource(name);
			if (audioSource != null && !audioSource.isPlaying) {
				float volume = audioSource.volume;
				audioSource.volume = 0;
				audioSource.Play();
				while (audioSource.volume < volume) {
					audioSource.volume += Time.deltaTime / fadeTime;
					yield return null;
				}

				audioSource.volume = volume;
			}
		}
		/// <summary>Use this to stop playing a sound with a fade out
		/// <para>It has to be in the Sound asset referenced in the AudioManager instance</para>
		/// </summary>
		/// <param name="name" type="string">The name of the sound</param>
		/// <param name="duration" type="float">The duration of the fade out</param>
		public void FadeOut(string name, float duration) {
			StartCoroutine(FadeOutCoroutine(name, duration));
		}
		IEnumerator FadeOutCoroutine(string name, float fadeTime) {
			AudioSource audioSource = GetSource(name);

			if (audioSource && audioSource.isPlaying) {
				float startVolume = audioSource.volume;

				while (audioSource.volume > 0) {
					audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
					yield return null;
				}

				audioSource.Stop();
				audioSource.volume = startVolume;
			}
		}

		/// <summary>Use this to start playing a sound muted
		/// <para>It has to be in the Sound asset referenced in the AudioManager instance</para>
		/// </summary>
		/// <param name="name" type="string">The name of the sound</param>
		public void PlayMuted(string name) {
			Sounds.List s = Array.Find(soundList.sounds, sound => sound.name == name);
			if (s == null) {
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}
			s.source.pitch = s.RandomPitch;
			s.source.volume = 0f;
			s.source.Play();
		}
		/// <summary>Use this to fade in a sound that is currently muted
		/// <para>It has to be in the Sound asset referenced in the AudioManager instance</para>
		/// <para>WARNING: If the PlayMuted hasn't been called before, this function won't work</para>
		/// </summary>
		/// <param name="name" type="string">The name of the sound</param>
		/// <param name="duration">The duration of the fade in</param>
		public void FadeMutedIn(string name, float duration) {
			StartCoroutine(FadeMutedInCoroutine(name, duration));
		}
		IEnumerator FadeMutedInCoroutine(string name, float fadeTime) {
			Sounds.List s = Array.Find(soundList.sounds, sound => sound.name == name);
			if (s == null) {
				Debug.LogWarning("Sound: " + name + " not found!");
				yield break;
			}

			while (s.source.volume < s.volume) {
				s.source.volume += Time.deltaTime / fadeTime;
				yield return null;
			}
			s.source.volume = s.volume;
		}
		/// <summary>Use this to fade out a sound and keep playing that muted
		/// <para>It has to be in the Sound asset referenced in the AudioManager instance</para>
		/// </summary>
		/// <param name="name" type="string">The name of the sound</param>
		/// <param name="duration">The duration of the fade out</param>
		public void FadeMutedOut(string name, float duration) {
			StartCoroutine(FadeMutedOutCoroutine(name, duration));
		}
		IEnumerator FadeMutedOutCoroutine(string name, float fadeTime) {
			Sounds.List s = Array.Find(soundList.sounds, sound => sound.name == name);
			if (s == null) {
				Debug.LogWarning("Sound: " + name + " not found!");
				yield break;
			}

			while (s.source.volume > 0) {
				s.source.volume -= Time.deltaTime / fadeTime;
				yield return null;
			}
			s.source.volume = 0;
		}
		#endregion
	}
}