using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class PreloadSettings : MonoBehaviour
{
    Resolution[] resolutions;
    [SerializeField] private AudioMixer _generalMixer;
    [SerializeField] PancartaScriptableObject pancartaScriptableObject;

    private void Awake()
    {
        pancartaScriptableObject.LoadTexture();
        /*if (PlayerPrefs.HasKey("resolutionIndex"))
        {
            resolutions = Screen.resolutions;
            Resolution resolution = resolutions[PlayerPrefs.GetInt("resolutionIndex", resolutions.Length - 1)];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }*/

        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("qualityIndex", 3));

        QualitySettings.vSyncCount = PlayerPrefs.GetInt("isVsync", 1);
        Screen.fullScreen = PlayerPrefs.GetInt("isFullscreen", 1) == 1;

        _generalMixer.SetFloat("VolumeMaster", PlayerPrefs.GetFloat("VolumeMaster", 0));
        _generalMixer.SetFloat("VolumeEffect", PlayerPrefs.GetFloat("VolumeEffect", 0));
        _generalMixer.SetFloat("VolumeMusic", PlayerPrefs.GetFloat("VolumeMusic", 0));


        Destroy(gameObject);
    }
}
