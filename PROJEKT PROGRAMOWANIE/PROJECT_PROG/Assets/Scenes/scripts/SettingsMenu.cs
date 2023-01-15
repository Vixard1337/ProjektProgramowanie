using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    public AudioMixer audioMixer; //odniesienie do mixer audio w unity Windows>Audio>AudioMixer

    public TMPro.TMP_Dropdown resolutionDropdown;
    
    Resolution[] resolutions; // pobieranie jak¹ dostêpn¹ rozdzielczoœæ ma u¿ytkownik 

        void Start ()
        {

            resolutions = Screen.resolutions;

            resolutionDropdown.ClearOptions(); //czyœci opcje

            List<string> options = new List<string>(); // nowa lista string dla rozdzielczoœci

            int currentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
            resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }

            }

            resolutionDropdown.AddOptions(options); //dodaje nowe opcje
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();

        }
    
    
    public void SetResolution (int resolutionIndex) // funkcja ustawienia rozdzielczosci
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume); // volume tak nazwany mixer który bêdzie zarz¹dzany przez skrypt //volume nazwa w exposed parameters
        // Debug.Log(volume);
    }

    public void SetQuality(int qualityIndex) //ustawia jakoœæ grafiki
    {
        QualitySettings.SetQualityLevel(qualityIndex); //index 0 - very low 1 - low 2 - medium 3 - high 4 - hery high 5 - ultra
    }

    public void SetFullscreen(bool isFullscreen) //ustawiam Pe³ny ekran lub tryb w oknie
    {
        Screen.fullScreen = isFullscreen;
        //Debug.Log("Fullscreen");
    }

}
