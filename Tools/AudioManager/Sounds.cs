using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Sounds", menuName = "Tools/Sounds", order = 0)]
public class Sounds : ScriptableObject{

    [Tooltip("The music mixer.")]
    public AudioMixerGroup mainMixer = default;
    [Tooltip("The SFX mixer.")]
    public AudioMixerGroup sfxMixer = default;

    public List[] sounds;

    [System.Serializable] public class List{
        [Tooltip("Name of the sound. Each name has to be different between each other.")]
        public string name;

        public AudioClip clip;

        [System.Serializable] public enum Type { Music, SFX }
        [Space]
        [Tooltip("Is it part of the music or the SFX?")] public Type type;

        [Space]
        [Tooltip("Default volume of the sound."), Range(0f, 1f)] public float volume;
        [Tooltip("Variance percentage of the volume"), Range(0f, 1f)] public float volumeVariance;
        [Tooltip("Default pitch of the sound."), Range(.1f, 3f)] public float pitch;
        [Tooltip("Variance percentage of the pitch"), Range(0f, 1f)] public float pitchVariance;

        public bool loop;

        [HideInInspector] public AudioSource source;

        float randomVolume;
        public float RandomVolume{
            get{
                randomVolume = volume * (1f + Random.Range(-volumeVariance / 2f, volumeVariance / 2f));
                return randomVolume;
            }
        }

        float randomPitch;
        public float RandomPitch{
            get{
                randomPitch = pitch * (1f + Random.Range(-pitchVariance / 2f, pitchVariance / 2f));
                return randomPitch;
            }
        }
    }
}
