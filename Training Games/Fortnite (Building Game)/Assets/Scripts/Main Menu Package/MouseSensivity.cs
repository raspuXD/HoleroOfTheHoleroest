using UnityEngine;
using UnityEngine.UI;

public class MouseSensivity : MonoBehaviour
{
    public Slider sensitivitySlider;

    void Start()
    {
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensivity", 500);
        sensitivitySlider.value = savedSensitivity;
        sensitivitySlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnSliderValueChanged(float value)
    {
        PlayerPrefs.SetFloat("MouseSensivity", value);
    }
}
