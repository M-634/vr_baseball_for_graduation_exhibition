using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// UnityEngine.UIに含まれる各種クラスの拡張クラス
/// </summary>
public static class UGUIListnerExtension
{
    public static void SetValueChangedEvent(this Slider slider, UnityAction<float> sliderCallback)
    {
        if (slider.onValueChanged != null)
        {
            slider.onValueChanged.RemoveAllListeners();
        }
        slider.onValueChanged.AddListener(sliderCallback);
    }

    public static void SetSliderValue(this Slider slider, float value)
    {
        slider.value = value;
    }
}
