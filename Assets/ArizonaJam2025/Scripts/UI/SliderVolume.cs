using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider slider;
    public AudioMixer audioMixer;
    public string audioGroupName;

    public void SaveValue(float newValue)
    {
        PlayerPrefs.SetFloat(audioGroupName, newValue);
        UpdateVolume();
    }

    public void UpdateVolume()
    {
        audioMixer.SetFloat(audioGroupName, Mathf.Max(-80f, Mathf.Log10(slider.value) * 20));
    }

    protected void Start()
    {
        if (slider == null) slider = GetComponent<Slider>();
        slider.value = PlayerPrefs.GetFloat(audioGroupName, slider.value);
        slider.onValueChanged.AddListener(SaveValue);
    }

#if UNITY_EDITOR
    protected void Reset()
	{
        if (slider == null) slider = GetComponent<Slider>();
	}
#endif
}
