using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QSComponent : MonoBehaviour
{
    public enum ResponseMode
    {
        Slider,
        SingleSelect,
        MultiSelect,
        Text
    }

    public string StudyType;
    public string QuestionID;

    public ResponseMode ResponseType;

    [SerializeField] private Slider Slider;
    [SerializeField] private TMP_Text LeftText;
    [SerializeField] private TMP_Text RightText;
    [SerializeField] private TMP_Text HandleText;
    

    public void SetSlider(int min, int max, string left = "", string right = "")
    {
        Slider.minValue = min;
        Slider.maxValue = max;

        LeftText.text = left;
        RightText.text = right;
    }

    public void OnSliderValueChanged(float value)
    {
        HandleText.text = ((int)value).ToString(); 
    }
}
