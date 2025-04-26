using UnityEngine;
using UnityEngine.UI;

public class SaveSens : MonoBehaviour
{
    [SerializeField] Slider theSlider;
    CameraController controller;

    private void Awake()
    {
        theSlider.onValueChanged.AddListener(HandleSlider);
    }

    private void Start()
    {
        theSlider.value = PlayerPrefs.GetFloat("MouseSens", 500f);
        HandleSlider(theSlider.value);

        controller = FindObjectOfType<CameraController>();
    }

    void HandleSlider(float value)
    {
        PlayerPrefs.SetFloat("MouseSens", theSlider.value);
        PlayerPrefs.Save();

        if(controller != null)
        {
            controller.mouseSensitivity = theSlider.value;
        }
    }
}
