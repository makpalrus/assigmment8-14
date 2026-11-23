using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;
    public string param = "MusicVol";

    void OnEnable()
    {
        SaveData data = SaveManager.Instance.Data;
        float value = param == "MasterVol" ? data.masterVolume : param == "MusicVol" ? data.musicVolume : data.soundVolume;
        slider.SetValueWithoutNotify(value);
    }

    public void Set(float value)
    {
        mixer.SetFloat(param, Mathf.Log10(Mathf.Max(value, .0001f)) * 20f);
        if (param == "MasterVol") SaveManager.Instance.Data.masterVolume = value;
        else if (param == "MusicVol") SaveManager.Instance.Data.musicVolume = value;
        else
        {
            SaveManager.Instance.Data.soundVolume = value;
            mixer.SetFloat("UIVol", Mathf.Log10(Mathf.Max(value, .0001f)) * 20f);
        }
        SaveManager.Instance.Save();
    }
}
