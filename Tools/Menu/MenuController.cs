using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

namespace SimpleTools.Menu {
    [System.Serializable] public class OnPlay : UnityEngine.Events.UnityEvent { }
    public class MenuController : MonoBehaviour {

        [Header("Audio")]
        [SerializeField] AudioMixer mainMixer = default;
        [Tooltip("The music volume the first time you start the game")][SerializeField, Range(0, 1)] float defaultMusicValue = .75f;
        [Tooltip("The SFX volume the first time you start the game")][SerializeField, Range(0, 1)] float defaultSfxValue = .75f;
        public Slider musicSlider = default;
        public Slider sfxSlider = default;

        [Header("Visual")]
        public TMP_Dropdown qualityDropdown = default;
        int qualitySelected;

        public TMP_Dropdown resolutionDropdown = default;
        Resolution[] resolutions;
        int currentResolutionIndex;

        [Space]
        [SerializeField] OnPlay onPlay = default;

        void Awake() {
            if (mainMixer) {
                float musicVolume = PlayerPrefs.GetFloat("MusicVolume", defaultMusicValue);
                float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", defaultSfxValue);
                mainMixer.SetFloat("Master", Mathf.Log10(musicVolume <= .0001f ? .0001f : musicVolume) * 20);
                mainMixer.SetFloat("SFX", Mathf.Log10(sfxVolume <= .0001f ? .0001f : sfxVolume) * 20);
            }

            resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();
            for (int i = 0; i < resolutions.Length; i++) {
                string option = resolutions[i].width + "x" + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) {
                    currentResolutionIndex = i;
                }
            }

            resolutions.Reverse();

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();

            qualityDropdown.ClearOptions();
            List<string> qualityNames = new List<string>();
            for (int i = 0; i < QualitySettings.names.Length; i++) {
                qualityNames.Add(QualitySettings.names[i]);
            }
            qualityDropdown.AddOptions(qualityNames);

            qualitySelected = PlayerPrefs.HasKey("QualitySelected") ? PlayerPrefs.GetInt("QualitySelected") : QualitySettings.GetQualityLevel();
            qualityDropdown.value = qualitySelected;
            QualitySettings.SetQualityLevel(qualitySelected);
            qualityDropdown.RefreshShownValue();
        }

        void OnValidate() {
            if (musicSlider) musicSlider.minValue = musicSlider.minValue < .0001f ? .0001f : musicSlider.minValue;
            if (sfxSlider) sfxSlider.minValue = sfxSlider.minValue < .0001f ? .0001f : sfxSlider.minValue;
        }

        void Start() {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", defaultMusicValue);
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", defaultSfxValue);
        }

        //Needs a slider between 0.0001 and 1
        public void SetMusicVolume(float sliderValue) {
            mainMixer.SetFloat("Master", Mathf.Log10(sliderValue) * 20);
            PlayerPrefs.SetFloat("MusicVolume", sliderValue);
        }

        //Needs a slider between 0.0001 and 1
        public void SetSfxVolume(float sliderValue) {
            mainMixer.SetFloat("SFX", Mathf.Log10(sliderValue) * 20);
            PlayerPrefs.SetFloat("SFXVolume", sliderValue);
        }

        public void SetQuality(int qualityIndex) {
            QualitySettings.SetQualityLevel(qualityIndex);
            PlayerPrefs.SetInt("QualitySelected", qualityIndex);
        }

        public void SetResolution(int resolutionIndex) {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void Play() {
            onPlay.Invoke();
        }

        public void Quit() {
            Application.Quit();
        }
    }
}